using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Models;
using Service;
using Data;
using PagedList;
using Service.Interface;

namespace HR_Web.Controllers
{
    [Authorize]
    public class EmployementController : Controller
    {
        int userId = 0;
        string userName = null;

        //service call
        private IEmployementService _IEmployementService;
        private ICityService _ICityService;
        private IStateService _IStateService;
        private ICountryService _ICountryService;
        private IUserService _IUserService;
        private IEmploymentCountService _IEmploymentCountService;

        //Data class
        EmploymentDetail _employement;
        Master_City _city;
        Master_State _state;
        Master_Country _country;


        public EmployementController(IEmployementService IEmployementService, ICityService ICityService, IStateService IStateService, ICountryService ICountryService, IUserService IUserService, IEmploymentCountService IEmploymentCountService)
        {
            this._IEmployementService = IEmployementService;
            _employement = new EmploymentDetail();
            this._ICityService = ICityService;
            _city = new Master_City();
            this._IStateService = IStateService;
            _state = new Master_State();
            this._ICountryService = ICountryService;
            _country = new Master_Country();
            this._IUserService = IUserService;
            this._IEmploymentCountService = IEmploymentCountService;
        }
        [HttpPost]
        public ActionResult SaveChangeRequestDetails(Employment model)
        {
            //get user details from db from logindetails table
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var LoginDetails = _IUserService.GetById(userId);
            if (model.NumberOfEmployments > 0)
            {
                if (LoginDetails.NoOfEmployments != model.NumberOfEmployments)
                {
                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "NoOfEmployments";
                    obj.FieldValue = model.NumberOfEmployments.ToString();
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = LoginDetails.NoOfEmployments.ToString();

                    var result = _IUserService.AddChangeRequestDetails(obj);

                }
            }



            return Json(new { result = true, Message = "Change request has been sent successfully!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckIfThereIsAnyChange(Employment model)
        {

            //get user details from db from logindetails table
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            //get login details
            var LoginDetails = _IUserService.GetById(userId);

            //All the fields on change request  fields should have values entered by Admin 
            if (LoginDetails.NoOfEmployments != model.NumberOfEmployments)
            {
                return Json(true, JsonRequestBehavior.AllowGet); //Change
            }
            else return Json(false, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OpenChangeRequestForm()
        {
           

            //Get logged in user details 
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var LoginDetails = _IUserService.GetById(userId);

            var Employment = new Employment();

            if (LoginDetails != null)
            {

                
                Employment.NumberOfEmployments = LoginDetails.NoOfEmployments;
             

            }

            

            return PartialView("_ChangeRequestForm", Employment);
        }
        public ActionResult Index()
        {
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value;
            var employmentCount = _IEmploymentCountService.GetEmploymentCountByUserId(userId);
            Employment employment = new Employment();

            ViewBag.NumberOfEmployments = userDetails.NoOfEmployments;
            employment.NumberOfEmployments = userDetails.NoOfEmployments;

            /* As per Change request EnBoard_Comments 01-11-2017.docx NoOfEmployments should update at admin side */
            /* 
               if (employmentCount != null)
               {
       
                    ViewBag.NumberOfEmployments = employmentCount.NumberOfEmployments;
                        employment.NumberOfEmployments = employmentCount.NumberOfEmployments;

                }
             */
            ViewData["Employment"] = employment;
            var employmentList = EmploymentDetailList(userId);
            EmployementModel employementModel = new EmployementModel();
            employementModel.IsCurrentEmployment = true;
            if (TempData["EmployementModel"] != null)
                employementModel = (EmployementModel)TempData["EmployementModel"];
            else if (employmentList.Any(x => x.IsCurrentEmployment == true))
            {
                employementModel.IsCurrentEmployment = false;
            }
            employementModel.NoOfEmployementAdmin = userDetails.NoOfEmployments == null ? 0 : Convert.ToInt32(userDetails.NoOfEmployments); 
            employementModel.NoOfEmployementAdded = employmentList.Count;
            employementModel.CurrencyList = GetCurrencies();
            employementModel.EmploymnetDetailsList = EmploymnetListPagedList(employmentList.OrderByDescending(p => p.ToDate).ToList()).ToList();
            return View(employementModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EmployementModel employementModel)
        {
            bool status = false;
            string userName = null;

            try
            {
                ModelState.Remove("ReasonForLeave");
                if (ModelState.IsValid)
                {
                    employementModel.UserId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                    userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                    var employmentList = EmploymentDetailList(Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]));

                    var numberOfEmployment = _IEmploymentCountService.GetEmploymentCountByUserId((int)employementModel.UserId);

                    #region commented Validation for Employee count 
                    //==================As Per Comment of EnBoard_ Comments 01-11-2017.docx=====================    
                    /*
                    if (numberOfEmployment != null)
                    {
                        if (numberOfEmployment.NumberOfEmployments == null || employmentList.Count >= numberOfEmployment.NumberOfEmployments)
                        {
                            TempData["EmployementModel"] = employementModel;
                            TempData["Message"] = new ErrorMessageModel()
                            {
                                MessageType = "error",
                                Message = "You cannot add employments more than you specified in Number Of Employments!"
                            };
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["EmployementModel"] = employementModel;
                        TempData["Message"] = new ErrorMessageModel()
                        {
                            MessageType = "error",
                            Message = "Please add Number Of Employemnts!"
                        };
                        return RedirectToAction("Index");
                    }
                    */
                    //==================End As Per Comment of EnBoard_ Comments 01-11-2017.docx=====================    
                    #endregion


                    if (employmentList.Count != 0 && employmentList.Any(x => x.IsCurrentEmployment == true) && employementModel.IsCurrentEmployment == true)
                    {
                        TempData["EmployementModel"] = employementModel;
                        TempData["Message"] = new ErrorMessageModel()
                        {
                            MessageType = "error",
                            Message = "Current employment is already added."
                        };
                        return RedirectToAction("Index");

                    }

                    if (employmentList.Any(x => x.CompanyName == employementModel.CompanyName))
                    {
                        TempData["EmployementModel"] = employementModel;
                        TempData["Message"] = new ErrorMessageModel()
                        {
                            MessageType = "error",
                            Message = "This employment is already exist."
                        };
                        return RedirectToAction("Index");

                    }

                    if (employementModel.FromDate.HasValue && employementModel.ToDate.HasValue)
                    {
                        if (employementModel.FromDate.Value >= employementModel.ToDate.Value)
                        {
                            TempData["EmployementModel"] = employementModel;
                            TempData["Message"] = new ErrorMessageModel()
                            {
                                MessageType = "error",
                                Message = "Relieving date cannot be less than or equal to date of joining."
                            };
                            return RedirectToAction("Index");
                        }

                    }

                    if (employmentList != null)
                    {
                        var overlappedRecord = employmentList.Where(u => Convert.ToDateTime(u.FromDate) <= Convert.ToDateTime(employementModel.ToDate) && Convert.ToDateTime(employementModel.FromDate) <= Convert.ToDateTime(u.ToDate) && u.IsActive == true).ToList();

                        if (overlappedRecord.Count > 0)
                        {
                            TempData["EmployementModel"] = employementModel;
                            TempData["Message"] = new ErrorMessageModel()
                            {
                                MessageType = "error",
                                Message = "Employment duration should not fall between previous or current employment duration."
                            };
                            return RedirectToAction("Index");
                        }
                    }

                    employementModel.UpdatedBy = userName;
                    employementModel.UpdatedDate = DateTime.UtcNow;

                    Mapper.CreateMap<EmployementModel, Data.EmploymentDetail>();
                    var EmployDetail = Mapper.Map<EmployementModel, Data.EmploymentDetail>(employementModel);


                    if (EmployDetail.CreatedBy == null || EmployDetail.CreatedBy == "")
                        EmployDetail.CreatedBy = userName;
                    if (EmployDetail.CreatedDate == DateTime.MinValue || EmployDetail.CreatedDate == null)
                        EmployDetail.CreatedDate = DateTime.UtcNow;

                    if (EmployDetail.UpdatedBy == null || EmployDetail.UpdatedBy == "")
                        EmployDetail.UpdatedBy = userName;
                    if (EmployDetail.UpdatedDate == DateTime.MinValue || EmployDetail.UpdatedDate == null)
                        EmployDetail.UpdatedDate = DateTime.UtcNow;

                    EmployDetail.EmployementNo = _IEmployementService.GetLatestEmploymentNo(Convert.ToInt32(employementModel.UserId));
                    EmployDetail.IsActive = true;

                    EmployDetail.CompanyCountryID = _ICountryService.GetAll(null, null, "").Where(i => i.Country == "Other").Select(x => x.CountryID).FirstOrDefault();
                    EmployDetail.CompanyStateID = _IStateService.GetAll(null, null, "").Where(i => i.State == "Other").Select(x => x.StateID).FirstOrDefault();
                    EmployDetail.CompanyCityID = _ICityService.GetAll(null, null, "").Where(i => i.City == "Other").Select(x => x.CityID).FirstOrDefault();

                    if (employementModel.IsCurrentEmployment == false)
                    {
                        EmployDetail.ReasonForLeave = employementModel.ReasonForLeave;
                    }

                    status = _IEmployementService.Insert(EmployDetail, null, "");

                    if (status != true)
                    {
                        TempData["Message"] = new ErrorMessageModel()
                        {
                            MessageType = "success",
                            Message = "Employement details added successfully for" + employementModel.CompanyName + "!!"
                        };
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    TempData["EmployementModel"] = employementModel;
                    TempData["Message"] = new ErrorMessageModel()
                    {
                        MessageType = "error",
                        Message = "Please fill mandetory details for " + employementModel.CompanyName
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["EmployementModel"] = employementModel;
                TempData["Message"] = new ErrorMessageModel()
                {
                    MessageType = "error",
                    Message = "Something went wrong for" + employementModel.CompanyName
                };

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(EmployementModel employementModel)
        {
            bool status = false;
            string userName = null;

            try
            {
                ModelState.Remove("ReasonForLeave");
                if (ModelState.IsValid)
                {
                    employementModel.UserId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                    userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                    var employmentList = EmploymentDetailList(Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]));

                    employementModel.UpdatedBy = userName;
                    employementModel.UpdatedDate = DateTime.UtcNow;

                    Mapper.CreateMap<EmployementModel, Data.EmploymentDetail>();
                    var EmployDetail = Mapper.Map<EmployementModel, Data.EmploymentDetail>(employementModel);

                    if (employementModel.FromDate.HasValue && employementModel.ToDate.HasValue)
                    {
                        if (employementModel.FromDate.Value >= employementModel.ToDate.Value)
                        {

                            TempData["Message"] = new ErrorMessageModel()
                            {
                                MessageType = "error",
                                Message = "Relieving date cannot be less than or equal to date of joining."
                            };
                            return RedirectToAction("Index");
                        }

                    }
                    if (employmentList.Where(y => y.EmploymentDetID != employementModel.EmploymentDetID).Any(x => (x.FromDate <= employementModel.FromDate && x.ToDate >= employementModel.FromDate) || (x.FromDate <= employementModel.ToDate && x.ToDate >= employementModel.ToDate)))
                    {

                        TempData["Message"] = new ErrorMessageModel()
                        {
                            MessageType = "error",
                            Message = "Employment duration should not fall between previous or current employment duration."
                        };
                        return RedirectToAction("Index");

                    }

                    if (EmployDetail.UpdatedBy == null || EmployDetail.UpdatedBy == "")
                        EmployDetail.UpdatedBy = userName;
                    if (EmployDetail.UpdatedDate == DateTime.MinValue || EmployDetail.UpdatedDate == null)
                        EmployDetail.UpdatedDate = DateTime.UtcNow;

                    EmployDetail.IsActive = true;
                    EmployDetail.CompanyCountryID = _ICountryService.GetAll(null, null, "").Where(i => i.Country == "Other").Select(x => x.CountryID).FirstOrDefault();
                    EmployDetail.CompanyStateID = _IStateService.GetAll(null, null, "").Where(i => i.State == "Other").Select(x => x.StateID).FirstOrDefault();
                    EmployDetail.CompanyCityID = _ICityService.GetAll(null, null, "").Where(i => i.City == "Other").Select(x => x.CityID).FirstOrDefault();

                    if (employementModel.IsCurrentEmployment == false)
                    {
                        EmployDetail.ReasonForLeave = employementModel.ReasonForLeave;
                    }

                    status = _IEmployementService.Update(EmployDetail, null, "");
                    TempData["Message"] = new ErrorMessageModel()
                    {
                        MessageType = "success",
                        Message = "Employment details updated successfully for " + employementModel.CompanyName + "!!"
                    };

                    if (status != true)
                    {
                        TempData["Message"] = new ErrorMessageModel()
                        {
                            MessageType = "error",
                            Message = "Something went wrong for" + employementModel.CompanyName
                        };
                        return RedirectToAction("Index");
                    }


                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorMessageModel()
                {
                    MessageType = "error",
                    Message = "Something went wrong for" + employementModel.CompanyName
                };
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteEmploymentDetails(int Id)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            bool data = _IEmployementService.DeleteEmploymnetDetail(Id, userName);
            TempData["Message"] = new ErrorMessageModel()
            {
                MessageType = "success",
                //Message = "Record deleted successfully."
                Message = "Employment details deleted successfully!!"
            };
            return RedirectToAction("Index");
            // return Json(new { result = data }, JsonRequestBehavior.AllowGet);

        }

        public List<EmploymentDetail> EmploymentDetailList(int userId)
        {
            return _IEmployementService.GetEmploymnetByUser(userId);
        }

        public List<EmploymetDetailsHistory> EmploymnetListPagedList(List<EmploymentDetail> list)
        {
            List<EmploymetDetailsHistory> ModelList = new List<EmploymetDetailsHistory>();
            ModelList = list
            .Select(x => new EmploymetDetailsHistory()
            {
                CompanyName = x.CompanyName,
                CompanyAddress = x.CompanyAddress,
                EmployementNo = x.EmployementNo,
                CTC = x.CTC,
                ReasonForLeave = x.ReasonForLeave,
                Designation = x.Designation,
                CurrencyID = x.CurrencyID.HasValue ? x.CurrencyID.Value : 0,
                EmploymentId = x.EmploymentDetID,
                FromDate = x.FromDate.HasValue ? x.FromDate.Value.ToShortDateString() : "",
                ToDate = x.ToDate.HasValue ? x.ToDate.Value.ToShortDateString() : "",
                SupervisiorName = x.SupervisorName,
                IsCurrentEmployment = x.IsCurrentEmployment.HasValue ? x.IsCurrentEmployment.Value : false

            })
            .ToList();

            return ModelList.ToList();
        }

        public ActionResult getPartialView(string EmployementNo = "EmployementNo1")
        {

            List<EmploymentDetail> emplymentList = new List<EmploymentDetail>();

            EmploymentDetail model;

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                var EmploymentDetails_lst = _IEmployementService.GetAll(null, null, "");
                Session["EmployDetails_lst"] = EmploymentDetails_lst;

                var obj = EmploymentDetails_lst.Where(u => u.UserID == userId).ToList();

                if (obj.Count < 5)
                {
                    for (int i = obj.Count; i < 5; ++i)
                    {
                        model = new EmploymentDetail();
                        model.EmployementNo = "EmployementNo" + (i + 1);
                        obj.Add(model);
                    }
                }
                emplymentList = obj;

            }
            var emplymentList1 = emplymentList.Where(u => u.UserID == userId && u.EmployementNo == EmployementNo).FirstOrDefault();

            Mapper.CreateMap<EmploymentDetail, EmployementModel>();
            var user = Mapper.Map<Data.EmploymentDetail, Models.EmployementModel>(emplymentList1);

            if (user != null)
            {
                user.CompanyCountryId = Convert.ToInt32(user.CompanyCountry);
                user.CompanyStateId = Convert.ToInt32(user.CompanyState);
                user.CompanyCityId = Convert.ToInt32(user.ComapnyCity);
            }
            else { user = new EmployementModel(); }
            return PartialView("_EmployementView", user);
        }

        public SelectList GetCurrencies()
        {
            List<Master_Currency> List = new List<Master_Currency>();

            List = _IEmployementService.GetCurrencies();

            IEnumerable<SelectListItem> selList = from l in List
                                                  select new SelectListItem
                                                  {
                                                      Value = l.CurrencyID.ToString(),
                                                      Text = l.CurrencyCode + " ( " + l.CurrencySymbol + " )"
                                                  };
            SelectList selectList = new SelectList(selList, "Value", "Text");

            return selectList;

        }


        /// <summary>
        /// Admin - View user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult GetEmploymnetDetailsGrid(int userId = 0)
        {
            Models.EmployementModel EmploymnetModel = new Models.EmployementModel();
            var employmentList = EmploymentDetailList(userId);
            var employmnetListPagedList = EmploymnetListPagedList1(employmentList);
            EmploymnetModel.EmploymnetDetailsList1 = employmnetListPagedList;
            return View("_GetEmploymentList", EmploymnetModel.EmploymnetDetailsList1);
        }

        /// <summary>
        /// Admin - View user details
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IPagedList<EmploymetDetailsHistory> EmploymnetListPagedList1(List<EmploymentDetail> list)
        {
            List<EmploymetDetailsHistory> ModelList = new List<EmploymetDetailsHistory>();
            ModelList = list
            .Select(x => new EmploymetDetailsHistory()
            {
                CompanyName = x.CompanyName,
                CompanyAddress = x.CompanyAddress,
                EmployementNo = x.EmployementNo,
                CTC = x.CTC,
                ReasonForLeave = x.ReasonForLeave,
                Designation = x.Designation,
                CurrencyID = x.CurrencyID.HasValue ? x.CurrencyID.Value : 0,
                EmploymentId = x.EmploymentDetID,
                FromDate = x.FromDate.HasValue ? x.FromDate.Value.ToShortDateString() : "",
                ToDate = x.ToDate.HasValue ? x.ToDate.Value.ToShortDateString() : "",
                SupervisiorName = x.SupervisorName,
                IsCurrentEmployment = x.IsCurrentEmployment.HasValue ? x.IsCurrentEmployment.Value : false

            })
            .ToList();
            int pageSize = 10;
            int pageIndex = 1;

            return ModelList.ToPagedList(pageIndex, pageSize);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddNumberOfEmployments(Employment model)
        {
            bool status = false;
            var UserId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var employmentCount = _IEmploymentCountService.GetEmploymentCountByUserId(UserId);

            if (employmentCount != null)
            {
                if (employmentCount.NumberOfEmployments > 0 && employmentCount.NumberOfEmployments >= model.NumberOfEmployments)
                {
                    TempData["Message"] = new ErrorMessageModel()
                    {
                        MessageType = "error",
                        Message = "Value should be greater than previous value of Number Of Employments!"
                    };
                    return RedirectToAction("Index");
                }

                employmentCount.NumberOfEmployments = model.NumberOfEmployments;
                status = _IEmploymentCountService.Update(employmentCount, null, "");
                if (status)
                {
                    TempData["Message"] = new ErrorMessageModel()
                    {
                        MessageType = "success",
                        Message = "Number Of Employments added successfully."
                    };
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = new ErrorMessageModel()
                    {
                        MessageType = "error",
                        Message = "Number Of Employments not added!"
                    };
                    return RedirectToAction("Index");
                }
            }
            else
            {
                EmploymentCount EmploymentCount = new Data.EmploymentCount();
                EmploymentCount.NumberOfEmployments = model.NumberOfEmployments;
                EmploymentCount.UserID = UserId;
                status = _IEmploymentCountService.Insert(EmploymentCount, null, "");
                if (status == true)
                {
                    TempData["Message"] = new ErrorMessageModel()
                    {
                        MessageType = "success",
                        Message = "Number Of Employments added successfully."
                    };
                }
                return RedirectToAction("Index");
            }
        }
    }
}