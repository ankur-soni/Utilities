using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using AutoMapper;
using Models;
using Service;
using Data;
using PagedList;
using System.Globalization;
using System.Data.Entity;
using Service.Interface;

namespace HR_Web.Controllers
{
    [Authorize]
    public class EducationController : Controller
    {
        int userId = 0;
        string userName = null;

        //service call
        private IEducationService _IEducationService;
        private IClassService _IClassService;
        private ICollegeService _ICollegeService;
        private IDisciplineService _IDisciplineService;
        private IEducationCategoryService _IEducationCategoryService;
        private ISpecializationService _ISpecializationService;
        private IUniversityService _IUniversityService;
        private IUserService _IUserService;
        private IEducationCategoryUniversityBoardMappingService _IEducationCategoryUniversityBoardMappingService;

        //Data class
        EmployeeEducationDetail _educationDetails;
        Master_Class _class;
        Master_College _college;
        Master_Discipline _discipline;
        Master_EducationCategory _educationCategory;
        Master_Specialization _specialization;
        Master_University _university;
        EducationCategoryUniversityBoardMapping _educationCategoryUniversityBoardMapping;

        public EducationController(IUserService IUserService, IEducationService IEducationService, IClassService IClassService, ICollegeService ICollegeService, IDisciplineService IDisciplineService,
            IEducationCategoryService IEducationCategoryService, ISpecializationService ISpecializationService, IUniversityService IUniversityService, IEducationCategoryUniversityBoardMappingService IEducationCategoryUniversityBoardMappingService)
        {
            this._IUserService = IUserService;
            this._IEducationService = IEducationService;
            _educationDetails = new EmployeeEducationDetail();
            this._IClassService = IClassService;
            _class = new Master_Class();
            this._ICollegeService = ICollegeService;
            _college = new Master_College();
            this._IDisciplineService = IDisciplineService;
            _discipline = new Master_Discipline();
            this._IEducationCategoryService = IEducationCategoryService;
            _educationCategory = new Master_EducationCategory();
            this._ISpecializationService = ISpecializationService;
            _specialization = new Master_Specialization();
            this._IUniversityService = IUniversityService;
            _university = new Master_University();
            this._IEducationCategoryUniversityBoardMappingService = IEducationCategoryUniversityBoardMappingService;
            _educationCategoryUniversityBoardMapping = new EducationCategoryUniversityBoardMapping();

        }
        public EducationController()
        {

        }

        /*New Code Education Details */
        /*Grid Code */


        [HttpGet]
        [Authorize]
        public ActionResult GetEducationalDetailsGrid(int userId)
        {
            Models.EducationDetails EducationModel = new EducationDetails();
            EducationModel.EducationalDetailsList = EducationListPagedList(userId);
            return View("_GetEducationList", EducationModel.EducationalDetailsList);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddEditEducationDetails(int EducationId = 0, bool IsReadOnly = false)
        {
            ViewBag.Classes = GetClasses();
            ViewBag.Discipline = GetDiscipline(0);
            ViewBag.Colleges = GetColleges();
            ViewBag.EducationCategory = GetEducationCategory();
            ViewBag.Specialization = GetSpecialization();
            ViewBag.University = GetUniversity();
            ViewBag.Months = GetMonths();
            ViewBag.Years = GetYears();

            ViewBag.IsReadOnly = IsReadOnly;
            ViewBag.IsEditPage = false;
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                //var res = user.Where(u => u.Password == details.Password && u.Email.ToLower() == details.Email.ToLower()).SingleOrDefault();
                var eduDetails_lst = _IEducationService.GetAll(null, null, "");
                Session["eduDetails_lst"] = eduDetails_lst;

                var obj = eduDetails_lst.Where(u => u.UserID == userId && u.EduDetID == EducationId).FirstOrDefault();
                if (obj != null)
                {
                    _educationDetails = obj;
                }

            }
            Models.EducationDetails model = new EducationDetails();
            if (EducationId != 0)
            {
                Mapper.CreateMap<Data.EmployeeEducationDetail, Models.EducationDetails>();
                model = Mapper.Map<Data.EmployeeEducationDetail, Models.EducationDetails>(_educationDetails);

                //By Sachin Khot
                //model.EducationCategoryId = Convert.ToInt32(model.EducationCategory);
                model.EducationCategoryId = Convert.ToInt32(model.EducationCategoryId);
                //model.InstituteNameId = Convert.ToInt32(model.InstituteName);
                model.InstituteNameId = Convert.ToInt32(model.CollegeID);
                //model.University_BoardNameId = Convert.ToInt32(model.University_BoardName);
                model.University_BoardNameId = Convert.ToInt32(model.UniversityID);
                //model.ClassId = Convert.ToInt32(model.Class);
                model.ClassId = Convert.ToInt32(model.ClassId);

                //ViewBag.Discipline = GetDiscipline(Convert.ToInt32(model.TypeofDegreeDeploma));
                ViewBag.Discipline = GetDiscipline(Convert.ToInt32(model.DisciplineID));

                //model.TypeofDegreeDeplomaId = Convert.ToInt32(model.TypeofDegreeDeploma);
                model.TypeofDegreeDeplomaId = Convert.ToInt32(model.DisciplineID);

                //model.SpecializationId = Convert.ToInt32(model.Specialization);
                model.SpecializationId = Convert.ToInt32(model.SpecializationId);
                ViewBag.IsEditPage = true;
            }
            //Mapper.CreateMap<List<GetEducationList_Result>, List<EducationDetailsHistory>>();            

            //user.EducationalDetailsList = EducationListPagedList(userId);
            return View("EducationalDetails", model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult EducationalDetails(string EducationCategoryId = "1")
        {
            ViewBag.Classes = GetClasses();
            ViewBag.Discipline = GetDiscipline(0);
            ViewBag.Colleges = GetColleges();
            ViewBag.EducationCategory = GetEducationCategory();
            ViewBag.Specialization = GetSpecialization();
            ViewBag.University = GetUniversity();
            ViewBag.Months = GetMonths();
            ViewBag.Years = GetYears();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                var eduDetails_lst = _IEducationService.GetAll(null, null, "");
                Session["eduDetails_lst"] = eduDetails_lst;

                var obj = eduDetails_lst.Where(u => u.UserID == userId && u.EducationCategoryID == Convert.ToInt16(EducationCategoryId)).FirstOrDefault();
                if (obj != null)
                {
                    _educationDetails = obj;
                }

            }

            Mapper.CreateMap<Data.EmployeeEducationDetail, Models.EducationDetails>();
            var user = Mapper.Map<Data.EmployeeEducationDetail, Models.EducationDetails>(_educationDetails);

            //Commented by Sachin khot
            //user.EducationCategoryId = Convert.ToInt32(user.EducationCategory);
            //user.InstituteNameId = Convert.ToInt32(user.InstituteName);
            //user.University_BoardNameId = Convert.ToInt32(user.University_BoardName);
            //user.ClassId = Convert.ToInt32(user.Class);
            //user.TypeofDegreeDeplomaId = Convert.ToInt32(user.TypeofDegreeDeploma);
            //user.SpecializationId = Convert.ToInt32(user.Specialization);

            //Update Code by Sachin Khot

            user.EducationCategoryId = Convert.ToInt32(user.EducationCategoryId);
            user.InstituteNameId = Convert.ToInt32(user.InstituteNameId);
            user.University_BoardNameId = Convert.ToInt32(user.University_BoardNameId);
            user.ClassId = Convert.ToInt32(user.ClassId);
            user.TypeofDegreeDeplomaId = Convert.ToInt32(user.TypeofDegreeDeplomaId);
            user.SpecializationId = Convert.ToInt32(user.SpecializationId);

            //Mapper.CreateMap<List<GetEducationList_Result>, List<EducationDetailsHistory>>();            

            //user.EducationalDetailsList = EducationListPagedList(userId);
            return View("NewEducationDetails", user);
        }

        [HttpGet]
        public ActionResult GetEducationalDetails()
        {
            ViewBag.Classes = GetClasses();
            ViewBag.Discipline = GetDiscipline(0);
            ViewBag.Colleges = GetColleges();
            ViewBag.EducationCategory = GetEducationCategory();
            ViewBag.Specialization = GetSpecialization();
            ViewBag.University = GetUniversity();
            ViewBag.Months = GetMonths();
            ViewBag.Years = GetYears();

            EducationDetails educationModel = new EducationDetails();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
                List<EmployeeEducationDetail> list = _IEducationService.GetEmployeeEducationDetailsByUserID(userId);
                educationModel.educationDetialslist = list
                .Select(x => new EducationDetails()
                {
                    EduDetID = x.EduDetID,
                    UserId = x.UserID,
                    FromDate = x.FromDate,
                    EducationCategoryId = x.EducationCategoryID,
                    OtherEducationCategory = x.OtherEducationCategory,
                    ToDate = x.ToDate,
                    BreaksDuringEducation = x.BreaksDuringEducation,
                    ClassId = x.ClassID,
                    TypeofDegreeDeplomaId = x.DisciplineID,
                    OtherDiscipline = x.OtherDiscipline,
                    PassingYear = x.PassingYear,
                    OtherSpecialization = x.OtherSpecialization,
                    InstituteNameId = x.CollegeID,
                    OtherCollegeName = x.OtherCollegeName,
                    University_BoardNameId = x.UniversityID,
                    OtherUniversityName = x.OtherUniversityName,
                    Percentage = x.Percentage,
                    UniversityList = GetUniversityList(x.EducationCategoryID)

                }).OrderByDescending(x => x.PassingYear).ToList();

                foreach (var item in educationModel.educationDetialslist)
                {
                    if (item.EducationCategoryId == null || item.EducationCategoryId == 0)
                    {
                        item.EducationCategory = item.OtherEducationCategory;
                    }
                    else
                    {
                        item.EducationCategory = (from pair in ViewBag.EducationCategory as SelectList
                                                  where pair.Value == item.EducationCategoryId.ToString()
                                                  select pair.Text).FirstOrDefault();
                    }
                }

            }
            ViewBag.EducationCategoryForUser = GetEducationCategoryByEducationId(userId);
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value == true;
            return View("NewEducationDetails", educationModel);
        }

        private SelectList GetUniversityList(int EducationCategoryID)
        {
            List<Master_University> List = new List<Master_University>();
            //List<Master_University> MappingList = new List<Master_University>();
            List<EducationCategoryUniversityBoardMapping> ListMapping = new List<EducationCategoryUniversityBoardMapping>();


            if (EducationCategoryID > 0)
            {
                int _EducationCategoryID = Convert.ToInt32(EducationCategoryID);

                List = _IUniversityService.GetAll(null, null, "").ToList();
                ListMapping = _IEducationCategoryUniversityBoardMappingService.GetAll(null, null, "").Where(x => x.EducationCategoryID == _EducationCategoryID).ToList();


            }

            var MappingList = (from A in List join B in ListMapping on A.UniversityID equals B.UniversityID select new { A.UniversityID, A.University }).ToList();
            SelectList selList = new SelectList(MappingList, "UniversityID", "University");

            return selList;

        }


        [HttpPost]
        public ActionResult LoadBoardUniverSityByEducationCategoryId(int EducationCategoryID)
        {
            try
            {

                SelectList selList = GetUniversityList(EducationCategoryID);

                return Json(selList);
                //Code change - EDMX Fix 

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IPagedList<EducationDetailsHistory> EducationListPagedList(int userid)
        {
            List<GetEducationList_Result> list = _IEducationService.GetEducationList(userid);
            List<EducationDetailsHistory> ModelList = new List<EducationDetailsHistory>();
            ModelList = list
           .Select(x => new EducationDetailsHistory() { EduDetID = x.EduDetID, DisciplineName = x.DisciplineName, EducationCategory = x.EducationCategory, PassingYear = x.PassingYear, Percentage = x.Percentage + " %", University = x.University })
           .ToList();
            int pageSize = 10;
            int pageIndex = 1;

            return ModelList.ToPagedList(pageIndex, pageSize);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EducationalDetails(EducationDetails empdetails)
        {
            try
            {

                EducationDetails details = new EducationDetails();
                if (empdetails.educationDetialslist != null && empdetails.educationDetialslist.Count == 1)
                {
                    details = (EducationDetails)empdetails.educationDetialslist[0];
                }
                else
                {
                    details = empdetails;
                }

                details.EducationCategoryId = details.EducationCategoryId;
                details.OtherEducationCategory = details.OtherEducationCategory;
                details.DisciplineID = details.TypeofDegreeDeplomaId;
                details.OtherDiscipline = details.OtherDiscipline;
                details.UniversityID = details.University_BoardNameId;
                details.OtherUniversityName = details.OtherUniversityName;
                details.ClassId = details.ClassId;
                details.CollegeID = details.InstituteNameId;
                details.OtherCollegeName = details.OtherCollegeName;
                details.SpecializationId = _ISpecializationService.GetAll(null, null, "").Where(i => i.Specialization == "Other").Select(x => x.SpecializationID).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(details.FromDate.ToString()) && !string.IsNullOrWhiteSpace(details.ToDate.ToString()))
                {
                    details.FromDate = details.FromDate;
                    details.ToDate = details.ToDate;
                }

                bool status = false;
                string userName = null;
                ModelState.Remove("PassingMonth");
                ModelState.Remove("PassingYear");
                ModelState.Remove("Percentage");
                ModelState.Remove("AttndedToMonth");
                ModelState.Remove("AttndedToYear");

                //if (ModelState.IsValid)
                //{
                details.UserId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                details.UpdatedBy = userName;
                details.UpdatedDate = DateTime.UtcNow;

                Mapper.CreateMap<EducationDetails, Data.EmployeeEducationDetail>();
                EmployeeEducationDetail eduDetail = Mapper.Map<EducationDetails, Data.EmployeeEducationDetail>(details);

                var obj = _IEducationService.GetEmployeeEducationDetailsByUserID(Convert.ToInt32(details.UserId)); //educationList.Where(u => u.UserID == details.UserId).ToList();
                var objUpdate = obj.Where(u => u.UserID == details.UserId).ToList();

                if (obj != null)
                {
                    var overlappedRecord = obj.Where(u => Convert.ToDateTime(u.FromDate) <= Convert.ToDateTime(details.ToDate) && Convert.ToDateTime(details.FromDate) <= Convert.ToDateTime(u.ToDate) && u.IsActive == true && u.EduDetID != details.EduDetID).ToList();

                    if (overlappedRecord.Count > 0)
                    {
                        return Json(new { result = false, Message = "Education duration should not fall between previous education duration." }, JsonRequestBehavior.AllowGet);
                    }

                    obj = obj.Where(u => u.UserID == details.UserId && u.EducationCategoryID == Convert.ToInt16(details.EducationCategoryId)
                                    && u.CollegeID == details.InstituteNameId
                                    && u.OtherSpecialization == Convert.ToString(details.OtherSpecialization)
                                    && u.PassingYear == details.PassingYear && u.IsActive == true).ToList();
                }

                //check if user have alredy added record for this category 
                if (obj.Count > 0 && details.EduDetID != obj.FirstOrDefault().EduDetID)
                {
                    return Json(new { result = false, Message = "Data for this Education Category is already Added" }, JsonRequestBehavior.AllowGet);
                }
                //if (eduDetail.CreatedBy == null || eduDetail.CreatedBy == null)
                //    eduDetail.CreatedBy = userName;
                //if (eduDetail.CreatedDate == DateTime.MinValue || eduDetail.CreatedDate == null)
                //    eduDetail.CreatedDate = DateTime.UtcNow;

                if (details.EduDetID == 0)
                {
                    eduDetail.CreatedBy = userName;
                    eduDetail.CreatedDate = DateTime.UtcNow;
                    eduDetail.UpdatedBy = userName;
                    eduDetail.UpdatedDate = DateTime.UtcNow;
                    eduDetail.IsActive = true;
                    status = _IEducationService.Insert(eduDetail, null, "");
                    return Json(new { result = false, Message = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    eduDetail.UpdatedBy = userName;
                    eduDetail.UpdatedDate = DateTime.UtcNow;
                    eduDetail.IsActive = true;
                    status = _IEducationService.Update(eduDetail, null, "");
                    return Json(new { result = false, Message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public SelectList GetClasses()
        {
            List<Master_Class> List = new List<Master_Class>();

            List = _IClassService.GetAll(null, null, "").ToList();
            // List = List.OrderBy(x => x.Class).ToList();
            SelectList selList = new SelectList(List, "ClassID", "Class");
            return selList;
        }

        public SelectList GetDiscipline(int catId)
        {
            List<Master_Discipline> disciplineList = new List<Master_Discipline>();
            disciplineList = _IDisciplineService.GetAll(null, null, "").ToList();

            List<Master_EducationCategory> categoryList = new List<Master_EducationCategory>();
            categoryList = _IEducationCategoryService.GetAll(null, null, "").ToList();

            int sscCatId = categoryList.Where(a => a.EducationCategory == "SSC").Select(a => a.EducationCategoryID).SingleOrDefault();
            int hscCatId = categoryList.Where(a => a.EducationCategory == "HSC").Select(a => a.EducationCategoryID).SingleOrDefault();

            int sscDisId = disciplineList.Where(a => a.DisciplineName == "SSC").Select(a => a.DisciplineID).SingleOrDefault();
            int hscDisId = disciplineList.Where(a => a.DisciplineName == "HSC").Select(a => a.DisciplineID).SingleOrDefault();

            if (catId == sscCatId)//SSC
            {
                Master_Discipline item = disciplineList.Where(m => m.DisciplineID == sscDisId).FirstOrDefault();
                disciplineList.Clear();
                disciplineList.Add(item);
            }
            else if (catId == hscCatId)//HSC
            {
                Master_Discipline item = disciplineList.Where(m => m.DisciplineID == hscDisId).FirstOrDefault();
                disciplineList.Clear();
                disciplineList.Add(item);
            }
            else if (catId != 0 && catId != sscCatId && catId != hscCatId)
            {
                disciplineList = disciplineList.Where(m => m.DisciplineID != sscDisId && m.DisciplineID != hscDisId).ToList();
            }
            SelectList selList = new SelectList(disciplineList, "DisciplineID", "DisciplineName");
            return selList;
        }

        public SelectList GetColleges()
        {
            List<Master_College> List = new List<Master_College>();

            List = _ICollegeService.GetAll(null, null, "").ToList();
            // List = List.OrderBy(x => x.College).ToList();
            SelectList selList = new SelectList(List, "CollegeID", "CollegeNAme");
            return selList;
        }

        public SelectList GetEducationCategory()
        {
            List<Master_EducationCategory> List = new List<Master_EducationCategory>();

            List = _IEducationCategoryService.GetAll(null, null, "").ToList();

            SelectList selList = new SelectList(List, "EducationCategoryID", "EducationCategory");
            return selList;
        }

        public SelectList GetSpecialization()
        {
            List<Master_Specialization> List = new List<Master_Specialization>();

            List = _ISpecializationService.GetAll(null, null, "").ToList();
            List = List.OrderBy(x => x.Specialization).ToList();

            Master_Specialization obj = new Master_Specialization();
            obj.SpecializationID = List.Count + 1;
            obj.Specialization = "Other";

            List.Add(obj);
            SelectList selList = new SelectList(List, "SpecializationID", "Specialization");
            return selList;
        }

        public SelectList GetUniversity()
        {
            List<Master_University> List = new List<Master_University>();

            List = _IUniversityService.GetAll(null, null, "").ToList();
            // List = List.OrderBy(x => x.University).ToList();
            SelectList selList = new SelectList(List, "UniversityID", "University");
            return selList;
        }

        public SelectList GetMonths()
        {
            List<Month> month = new List<Month>();
            Month m = new Month();
            m.MonthId = 1;
            m.MonthName = "January";
            month.Add(m);
            m = new Month();
            m.MonthId = 2;
            m.MonthName = "February";
            month.Add(m);
            m = new Month();
            m.MonthId = 3;
            m.MonthName = "March";
            month.Add(m);
            m = new Month();
            m.MonthId = 4;
            m.MonthName = "April";
            month.Add(m);
            m = new Month();
            m.MonthId = 5;
            m.MonthName = "May";
            month.Add(m);
            m = new Month();
            m.MonthId = 6;
            m.MonthName = "June";
            month.Add(m);
            m = new Month();
            m.MonthId = 7;
            m.MonthName = "July";
            month.Add(m);
            m = new Month();
            m.MonthId = 8;
            m.MonthName = "August";
            month.Add(m);
            m = new Month();
            m.MonthId = 9;
            m.MonthName = "September";
            month.Add(m);
            m = new Month();
            m.MonthId = 10;
            m.MonthName = "October";
            month.Add(m);
            m = new Month();
            m.MonthId = 11;
            m.MonthName = "November";
            month.Add(m);
            m = new Month();
            m.MonthId = 12;
            m.MonthName = "December";
            month.Add(m);

            SelectList selList = new SelectList(month, "MonthId", "MonthName");
            return selList;
        }

        private SelectList GetYears()
        {
            //List<int> Years = new List<int>();
            List<Year> yearlist = new List<Year>();
            DateTime startYear = new DateTime(1970, 1, 1);
            int yearid = 1;
            while (DateTime.UtcNow.Year >= startYear.Year)
            {
                Year year = new Year();
                year.YearName = Convert.ToString(startYear.Year);
                year.YearId = startYear.Year;
                //Years.Add(startYear.Year);
                yearlist.Add(year);
                startYear = startYear.AddYears(1);
            }
            return new SelectList(yearlist, "YearId", "YearName"); // new SelectList(Years);
        }


        private SelectList GetEducationCategoryByEducationId(int userID)
        {
            List<Master_EducationCategory> list = _IEducationService.GetEducationcategoryListByUserId(userID);

            SelectList selList = new SelectList(list, "EducationCategoryID", "EducationCategory");
            return selList;
        }

        //public SelectList GetYears()
        //{
        //    List<Year> year = new List<Year>();
        //    Year y = new Year();
        //    y.YearId = 1;
        //    y.YearName = "2016";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 2;
        //    y.YearName = "2015";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 3;
        //    y.YearName = "2014";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 4;
        //    y.YearName = "2013";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 5;
        //    y.YearName = "2012";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 6;
        //    y.YearName = "2011";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 7;
        //    y.YearName = "2010";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 8;
        //    y.YearName = "2009";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 9;
        //    y.YearName = "2008";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 10;
        //    y.YearName = "2007";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 11;
        //    y.YearName = "2006";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 12;
        //    y.YearName = "2005";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 13;
        //    y.YearName = "2004";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 14;
        //    y.YearName = "2003";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 15;
        //    y.YearName = "2002";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 16;
        //    y.YearName = "2001";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 17;
        //    y.YearName = "2000";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 18;
        //    y.YearName = "1999";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 19;
        //    y.YearName = "1998";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 20;
        //    y.YearName = "1997";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 21;
        //    y.YearName = "1996";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 22;
        //    y.YearName = "1995";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 23;
        //    y.YearName = "1994";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 24;
        //    y.YearName = "1993";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 25;
        //    y.YearName = "1992";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 26;
        //    y.YearName = "1991";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 27;
        //    y.YearName = "1990";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 28;
        //    y.YearName = "1989";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 29;
        //    y.YearName = "1988";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 30;
        //    y.YearName = "1987";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 31;
        //    y.YearName = "1986";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 32;
        //    y.YearName = "1985";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 33;
        //    y.YearName = "1984";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 34;
        //    y.YearName = "1983";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 35;
        //    y.YearName = "1982";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 36;
        //    y.YearName = "1981";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 37;
        //    y.YearName = "1980";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 38;
        //    y.YearName = "1979";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 39;
        //    y.YearName = "1978";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 40;
        //    y.YearName = "1977";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 41;
        //    y.YearName = "1976";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 42;
        //    y.YearName = "1975";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 43;
        //    y.YearName = "1974";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 44;
        //    y.YearName = "1973";
        //    year.Add(y);
        //    y = new Year();
        //    y.YearId = 45;
        //    y.YearName = "1972";
        //    year.Add(y);
        //    y = new Year();          
        //    y.YearId = 46;
        //    y.YearName = "1971";
        //    year.Add(y);
        //    y = new Year(); 
        //    y.YearId = 47;
        //    y.YearName = "1970";
        //    year.Add(y);



        //    SelectList selList = new SelectList(year, "YearId", "YearName");
        //    return selList;
        //}


        public ActionResult DeleteEducationDetails(int EducationID)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            bool data = _IEducationService.DeleteEducationDetail(EducationID, userName);
            return Json(new { result = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelevantDisciplines(int categoryId)
        {
            var selList = GetDiscipline(categoryId);
            return Json(selList, JsonRequestBehavior.AllowGet);
        }
    }
}