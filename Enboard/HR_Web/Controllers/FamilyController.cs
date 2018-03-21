using AutoMapper;
using Data;
using HR_Web.Utilities;
using Models;
using PagedList;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HR_Web.Controllers
{
    [Authorize]
    public class FamilyController : Controller
    {
        //Global Variables
        int userId = 0;
        string userName = null;

        //Service Call
        private IRelationService _IRelationService;
        private IFamilyDetailsService _IFamilyDetailsService;
        private IPersonalService _IPersonalService;
        private IUserService _IUserService;
        //Data Call
        Master_Relation _relation;
        EmployeeFamilyDetail _employeeFamilyDetail;



        public FamilyController(IRelationService IRelationService, IFamilyDetailsService IFamilyDetailsService, IPersonalService IPersonalService, IUserService IUserService)
        {
            _IRelationService = IRelationService;
            _relation = new Master_Relation();
            _IFamilyDetailsService = IFamilyDetailsService;
            _employeeFamilyDetail = new EmployeeFamilyDetail();
            _IPersonalService = IPersonalService;
            this._IUserService = IUserService;
        }

        // GET: Family
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Family details view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult FamilyDetails()
        {
            ViewBag.Relationship = GetRelationshipList();
            ViewBag.DependentList = GetDependentList();
            ViewBag.CountryCodeList = GetCountryCode();

            FamilyDetails familydetailsModel = new FamilyDetails();
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            familydetailsModel.FamilyHistoryList = FamilyDetailsPagedList(userId);
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value;
            return View("FamilyDetails", familydetailsModel.FamilyHistoryList);
        }

        /// <summary>
        /// Get family details list 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private IPagedList<FamilyHistory> FamilyDetailsPagedList(int userid)
        {
            List<EmployeeFamilyDetail> list = _IFamilyDetailsService.GetEmployeeFamilyDetailsByUserID(userid);
            var genderlist = GetGenderList();
            var dependentlist = GetDependentList();
            var countryCodeList = GetCountryCode();
            List<Master_Relation> relationShipList = _IRelationService.GetAll(null, null, "").ToList();
            List<FamilyHistory> ModelList = new List<FamilyHistory>();
            int id = 0;

            ModelList = list
            .Select(x => new FamilyHistory()
            {
                ID = 0,
                FamDetID = x.FamDetID,
                //FullName = x.FullName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DOB = x.DOB.HasValue ? x.DOB.Value.ToShortDateString() : string.Empty,
                Gender = (x.Gender != null) ? genderlist.SingleOrDefault(s => s.Text == x.Gender).Text : string.Empty,
                IsActive = x.IsActive.ToString(),
                RelationShipName = relationShipList.SingleOrDefault(s => s.RelationID == x.RelationshipID).RelationName,
                Dependent = (x.Dependent != null) ? dependentlist.SingleOrDefault(s => s.Value == x.Dependent).Text : string.Empty,
                RelationshipID = x.RelationshipID,
                UserID = x.UserID,
                CountryCode = x.CountryCode,
                ContactNumber = x.ContactNumber,
                IsEmergencyContact = x.IsEmergencyContact.HasValue ? x.IsEmergencyContact.Value : false
            })
            .ToList();

            var personalDetails = _IPersonalService.GetPersonalDetailsByUserId(userId);
            if (personalDetails != null && personalDetails.MaritalStatID == Constants.Single)
            {
                ModelList = ModelList.Where(m => m.RelationshipID != Constants.Child).ToList();
                ModelList = ModelList.Where(m => m.RelationshipID != Constants.Spouse).ToList();
            }

            int pageSize = 10; //not required
            int pageIndex = 1; //not required

            foreach (var item in ModelList)
            {
                item.ID = id + 1;
                id = item.ID;
            }

            return ModelList.ToPagedList(pageIndex, pageSize);
        }


        /// <summary>
        /// // For Dependent list //Not required
        /// </summary>
        /// <returns></returns>
        private SelectList GetDependentList()
        {
            List<dynamic> list = new List<dynamic>();
            var item1 = new { DependentID = true, DependentName = "Yes" };
            var item2 = new { DependentID = false, DependentName = "No" };
            list.Add(item1);
            list.Add(item2);
            SelectList selList = new SelectList(list, "DependentID", "DependentName");
            return selList;
        }


        /// <summary>
        ///   // For Gender list
        /// </summary>
        /// <returns></returns>
        private SelectList GetGenderList()
        {
            List<dynamic> list = new List<dynamic>();
            var item1 = new { GenderID = "1", GenderName = "Male" };
            var item2 = new { GenderID = "2", GenderName = "Female" };
            list.Add(item1);
            list.Add(item2);
            SelectList selList = new SelectList(list, "GenderID", "GenderName");
            return selList;
        }

        /// <summary>
        /// Get relationship list
        /// </summary>
        /// <returns></returns>
        private SelectList GetRelationshipList()
        {

            List<Master_Relation> AllRelationshipList = new List<Master_Relation>();
            AllRelationshipList = _IRelationService.GetAll(null, null, "").ToList();
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);

            var personalDetails = _IPersonalService.GetPersonalDetailsByUserId(userId);

            if (personalDetails != null && personalDetails.MaritalStatID == Constants.Single)
            {
                AllRelationshipList = AllRelationshipList.Where(r => r.RelationID != Constants.Spouse && r.RelationID != Constants.Child).ToList();
            }
            else
            {
                AllRelationshipList = AllRelationshipList.OrderBy(x => x.RelationName).ToList();
            }

            List<EmployeeFamilyDetail> list = _IFamilyDetailsService.GetEmployeeFamilyDetailsByUserID(userId);
            List<Master_Relation> AddedRelationshipList = new List<Master_Relation>();
            List<Master_Relation> MasterRelationShipList = _IRelationService.GetAll(null, null, "").ToList();
            foreach (var item in list)
            {
                AddedRelationshipList.Add(new Master_Relation()
                {
                    RelationID = item.RelationshipID,
                    RelationName = MasterRelationShipList.SingleOrDefault(s => s.RelationID == item.RelationshipID).RelationName,
                });
            }

            //Common in all reltionships and which are already added relationships
            var commonRelationshipList = AllRelationshipList.Where(p => AddedRelationshipList.Any(p2 => p2.RelationID == p.RelationID)).ToList();
            var remainingRelationshipList = AllRelationshipList.Where(p => !commonRelationshipList.Any(p2 => p2.RelationID == p.RelationID)).ToList();
            //If married add , child item
            if (personalDetails != null && personalDetails.MaritalStatID == Constants.Married)
            {
                if (!remainingRelationshipList.Any(o => o.RelationID == Constants.Child))
                    remainingRelationshipList.AddRange(AllRelationshipList.Where(i => i.RelationID == Constants.Child));
            }

            SelectList selList = new SelectList(remainingRelationshipList.ToList(), "RelationID", "RelationName");

            return selList;
        }

        /// <summary>
        /// Get country code list
        /// </summary>
        /// <returns></returns>
        public SelectList GetCountryCode()
        {
            List<CountryCode> CountryCodeList = new List<CountryCode>();
            CountryCode obj = new CountryCode();
            obj.Text = "+ 91";
            obj.Value = "91";
            CountryCodeList.Add(obj);

            obj = new CountryCode();
            obj.Text = "+ 44";
            obj.Value = "44";
            CountryCodeList.Add(obj);

            obj = new CountryCode();
            obj.Text = "+ 1";
            obj.Value = "1";
            CountryCodeList.Add(obj);

            SelectList sellist = new SelectList(CountryCodeList, "Value", "Text");
            return sellist;
        }

        /// <summary>
        /// Post newly added family details
        /// </summary>
        /// <param name="familyDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFamilyDetails(FamilyDetails familyDetails)
        {
            bool status = false;
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    familyDetails.UserID = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                    userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                    Mapper.CreateMap<FamilyDetails, Data.EmployeeFamilyDetail>();
                    var EmpfamilyDetails = Mapper.Map<FamilyDetails, Data.EmployeeFamilyDetail>(familyDetails);


                    if (EmpfamilyDetails.CreatedBy == null || EmpfamilyDetails.CreatedBy == "")
                        EmpfamilyDetails.CreatedBy = userName;
                    if (EmpfamilyDetails.CreatedDate == DateTime.MinValue || EmpfamilyDetails.CreatedDate == null)
                        EmpfamilyDetails.CreatedDate = DateTime.UtcNow;

                    if (EmpfamilyDetails.UpdatedBy == null || EmpfamilyDetails.UpdatedBy == "")
                        EmpfamilyDetails.UpdatedBy = userName;
                    if (EmpfamilyDetails.UpdatedDate == DateTime.MinValue || EmpfamilyDetails.UpdatedDate == null)
                        EmpfamilyDetails.UpdatedDate = DateTime.UtcNow;

                    var EmployeeFamilyList = _IFamilyDetailsService.GetAll(null, null, "");
                    if (EmployeeFamilyList != null)
                    {
                        var obj = EmployeeFamilyList.Where(u => u.UserID == familyDetails.UserID).ToList();

                        var lstEmployeeFamilyDetails = obj;
                        if (obj != null)
                        {
                            obj = obj.Where(u => u.UserID == familyDetails.UserID && u.FamDetID == Convert.ToInt32(familyDetails.FamDetID)).ToList();
                        }

                        if (obj.Count == 0)
                        {

                            EmpfamilyDetails.IsActive = true;
                            status = _IFamilyDetailsService.Insert(EmpfamilyDetails, null, "");
                            var addedFamilyDetail = EmpfamilyDetails;

                            #region Make only one emergency contact
                            if (addedFamilyDetail.IsEmergencyContact == true)
                            {
                                foreach (EmployeeFamilyDetail employeeFamilyDetail in lstEmployeeFamilyDetails.Where(p => p.IsEmergencyContact == true))
                                {
                                    employeeFamilyDetail.IsEmergencyContact = false;
                                    var isSuccess = _IFamilyDetailsService.Update(employeeFamilyDetail, null, "");
                                    if (!isSuccess)
                                    {
                                        return Json(new { success = false, response = "Unable to add family detail." });
                                    }
                                }
                            }
                            #endregion

                            if (status == true)
                                return Json(status);
                        }
                    }
                }
                else
                {
                    ViewBag.Relationship = GetRelationshipList();
                    ViewBag.DependentList = GetDependentList();
                    ViewBag.GenderList = GetGenderList();

                    var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                    foreach (var modelStateError in modelStateErrors)
                    {
                        message += modelStateError.ErrorMessage + Environment.NewLine;
                    }
                    return Json(new { success = false, response = message });
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="familyDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFamilyDetails(FamilyHistory familyDetails)
        {
            try
            {
                bool status = false;

                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    familyDetails.UserID = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                    userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                    Mapper.CreateMap<FamilyHistory, Data.EmployeeFamilyDetail>();
                    var EmpfamilyDetails = Mapper.Map<FamilyHistory, Data.EmployeeFamilyDetail>(familyDetails);


                    #region Make only one emergency contact
                    if (familyDetails.IsEmergencyContact == true)
                    {
                        var lstEmployeeFamilyDetails = _IFamilyDetailsService.GetEmployeeFamilyDetailsByUserID((int)familyDetails.UserID).Where(p => p.IsEmergencyContact == true);
                        foreach (EmployeeFamilyDetail employeeFamilyDetail in lstEmployeeFamilyDetails)
                        {
                            employeeFamilyDetail.IsEmergencyContact = false;
                            var isSuccess = _IFamilyDetailsService.Update(employeeFamilyDetail, null, "");
                            if (!isSuccess)
                            {
                                return Json(new { success = false, response = "Unable to add family detail." });
                            }
                        }
                    }
                    #endregion


                    if (EmpfamilyDetails.UpdatedBy == null || EmpfamilyDetails.UpdatedBy == "")
                        EmpfamilyDetails.UpdatedBy = userName;
                    if (EmpfamilyDetails.UpdatedDate == DateTime.MinValue || EmpfamilyDetails.UpdatedDate == null)
                        EmpfamilyDetails.UpdatedDate = DateTime.UtcNow;
                    EmpfamilyDetails.IsActive = true;
                    status = _IFamilyDetailsService.Update(EmpfamilyDetails, null, "");

                    if (status == true)
                    {
                        return Json(status);
                    }
                    return Json(status);
                }

                else
                {
                    ViewBag.Relationship = GetRelationshipList();
                    ViewBag.DependentList = GetDependentList();
                    ViewBag.GenderList = GetGenderList();

                    var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                    foreach (var modelStateError in modelStateErrors)
                    {
                        message += modelStateError.ErrorMessage + Environment.NewLine;
                    }
                    return Json(new { success = false, response = message });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Soft Delete family details 
        /// </summary>
        /// <param name="FamDetID"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteFamilyDetails(long FamDetID)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            var result = _IFamilyDetailsService.DeleteEmployeeFamilyDetails(FamDetID, userName);
            return result;
        }


        /// <summary>
        /// For admin side show family list in grid
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult FamilyDetailsGridForAdmin(int Id = 0)
        {
            ViewBag.CountryCodeList = GetCountryCode();
            FamilyDetails familydetailsModel = new FamilyDetails();
            familydetailsModel.FamilyHistoryList = FamilyDetailsPagedList(Id);
            return View("_GetFamilyList", familydetailsModel.FamilyHistoryList);
        }
    }
}