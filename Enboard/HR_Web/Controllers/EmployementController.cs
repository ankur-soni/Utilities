﻿using AutoMapper;
using Data;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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



        public ActionResult Index()
        {
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value;
            var employmentCount = _IEmploymentCountService.GetEmploymentCountByUserId(userId);

            var employmentList = EmploymentDetailList(userId);
            EmployementModel employementModel = new EmployementModel();
            employementModel.IsCurrentEmployment = false;
            if (TempData["EmployementModel"] != null)
                employementModel = (EmployementModel)TempData["EmployementModel"];
            else if (employmentList.Any(x => x.IsCurrentEmployment == true))
            {
                employementModel.IsCurrentEmployment = false;
            }

            employementModel.NoOfEmployementAdded = employmentList.Count;
            employementModel.CurrencyList = GetCurrencies();
            employementModel.IsFresher = _IEmployementService.IsFresher(userId);
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

                    #region Employee Can join same company more than one time 
                    /*
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
                    */
                    #endregion Employee Can join same company more than one time 
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
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value;
            var employmentCount = _IEmploymentCountService.GetEmploymentCountByUserId(userId);
            var employmentList = EmploymentDetailList(userId);
            EmployementModel employementModel = new EmployementModel();
            employementModel.NoOfEmployementAdded = employmentList.Count;
            employementModel.CurrencyList = GetCurrencies();
            employementModel.IsFresher = _IEmployementService.IsFresher(userId);
            employementModel.EmploymnetDetailsList = EmploymnetListPagedList(employmentList.OrderByDescending(p => p.ToDate).ToList()).ToList();
            return View("_GetEmploymentList", employementModel);
        }
    }
}