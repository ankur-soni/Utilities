using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Models;
using Service;
using Data;
using System.Web.Security;
using System.Net.Mail;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using AutoMapper;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using HR_Web.Utilities;
using PagedList;
using System.Threading;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using HR_Web.CustomFilters;
using Service.Interface;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using HR_Web.ViewModel;
using Silicus.FrameWorx.Logger;

namespace HR_Web.Controllers
{
    //Code change - EDMX Fix 
    [Authorize]
    public class UserController : Controller
    {
        int userId = 0;
        string userName = null;

        //service call
        private IUserService _IUserService;
        private IPersonalService _IPersonalService;
        private ILanguageservice _ILanguageservice;
        private ICityService _ICityService;
        private IStateService _IStateService;
        private ICountryService _ICountryService;
        private IContactService _IContactService;
        private IEducationService _IEducationService;
        private IMaritalStatusService _IMaritalStatusService;
        private IRoleService _IRoleService;
        private IEmployeeService _IEmployeeService;
        private IDocumentDetailsService _IDocumentDetailsService;
        private IProfessionalDetailsService _IProfessionalDetailsService;
        private IBloodGroupService _IBloodGroupService;
        private IEducationCategoryService _IEducationCategoryService;
        private ICandidateProgressDetailService _ICandidateProgressDetailService;
        private IRelationService _IRelationService;
        private IEmploymentCountService _IEmploymentCountService;
        private IDesignationService _IDesignationService;
        //Data clases
        LoginDetail _Logindetails;
        EmployeePersonalDetail _personalDetails;
        Master_Language _language;
        Master_City _city;
        Master_State _state;
        Master_Country _country;
        EmployeeContactDetail _contactDetails;
        EmployeeEducationDetail _educationDetails;
        Master_MaritalStatus _maritalStatus;
        Master_Role _roleMaster;
        EmployeeMaster _employeeMaster;
        EmployeeProfessionalDetail _employeeProffesionlaDetails;
        Master_Bloodgroup _bloodgroup;
        Master_EducationCategory _educationCategory;
        private readonly ILogger _logger;

        public UserController(IUserService IUserService, IPersonalService IPersonalService,
            ILanguageservice ILanguageservice, ICityService ICityService, IStateService IStateService,
            ICountryService ICountryService, IContactService IContactService, IEducationService IEducationService,
            IMaritalStatusService IMaritalStatusService, IDocumentDetailsService IDocumentDetailsService,
            IRoleService IRoleService, IEmployeeService IEmployeeService, IProfessionalDetailsService IProffesionalDetailsService
            , IEducationCategoryService IEducationCategoryService, ICandidateProgressDetailService ICandidateProgressDetailService, IRelationService IRelationService, IEmploymentCountService IEmploymentCountService, IDesignationService IDesignationService)
        {
            this._IUserService = IUserService;
            this._IPersonalService = IPersonalService;
            _Logindetails = new LoginDetail();
            _personalDetails = new EmployeePersonalDetail();
            _ILanguageservice = ILanguageservice;
            _language = new Master_Language();
            _ICityService = ICityService;
            _city = new Master_City();
            _IStateService = IStateService;
            _state = new Master_State();
            _ICountryService = ICountryService;
            _country = new Master_Country();
            _IContactService = IContactService;
            _contactDetails = new EmployeeContactDetail();
            _IEducationService = IEducationService;
            _educationDetails = new EmployeeEducationDetail();
            _IMaritalStatusService = IMaritalStatusService;
            _maritalStatus = new Master_MaritalStatus();
            _IDocumentDetailsService = IDocumentDetailsService;
            _IRoleService = IRoleService;
            _roleMaster = new Master_Role();
            _employeeMaster = new EmployeeMaster();
            _IEmployeeService = IEmployeeService;
            _IProfessionalDetailsService = IProffesionalDetailsService;
            _employeeProffesionlaDetails = new EmployeeProfessionalDetail();
            this._IEducationCategoryService = IEducationCategoryService;
            _educationCategory = new Master_EducationCategory();
            _ICandidateProgressDetailService = ICandidateProgressDetailService;
            _IRelationService = IRelationService;
            _IEmploymentCountService = IEmploymentCountService;
            _IDesignationService = IDesignationService;
            _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty),
                (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
        }

        public UserController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Login Get Method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            Mapper.CreateMap<Data.LoginDetail, Models.LoginDetails>();
            var user = Mapper.Map<Data.LoginDetail, Models.LoginDetails>(_Logindetails);
            _logger.Log("Login");
            return View(user);
        }
        /// <summary>
        /// Login Post Method
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginDetails details)
        {
            if (ModelState.IsValid)
            {

                var user = _IUserService.GetAll(null, null, "").ToList();

                var chkDeactivateduser = user.Where(u => u.Password == details.Password && u.Email.ToLower().Trim() == details.Email.ToLower().Trim() && Convert.ToInt32(u.IsActive) == 2).FirstOrDefault();
                if (chkDeactivateduser != null)
                {
                    ModelState.AddModelError("", "Your Login has been Deactivated");
                    return View(details);
                }

                string EncryptedEmail = SessionManager.EncryptData(details.Email.Trim());
                var res = user.Where(u => u.Email.ToLower().Trim() == details.Email.ToLower().Trim() && Convert.ToInt32(u.IsActive) == 1).FirstOrDefault();
                if (res != null)
                {
                    //Change request - fixed timezone issue 
                    SessionManager.LastLogin = TimeZoneInfo.ConvertTimeFromUtc(res.LastLogin.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                    res.LastLogin = DateTime.UtcNow;
                    _IUserService.Update(res, null, "");

                    // add or update  UserReminder Table

                    UserReminder userReminder = new UserReminder
                    {
                        UserId = res.UserID,
                        UserName = res.FirstName + " " + res.LastName,
                        EmailID = res.Email,
                        LastLogin = res.LastLogin.Value
                    };

                    if (SessionManager.DecryptData(res.Password).ToLower() == details.Password.ToLower().Trim())
                    {
                        if (res != null)
                        {
                            FormsAuthentication.SetAuthCookie(res.FirstName + "|" + res.UserID, false);
                            if (res.ActivatedDate == null)
                            {
                                res.ActivatedDate = DateTime.UtcNow;
                                _IUserService.Update(res, null, "");
                            }

                            var rolechk = _IRoleService.GetAll(null, null, "").Where(x => x.UserName.ToLower() == res.Email.ToLower().Trim()).FirstOrDefault();
                            if (rolechk != null)
                            {
                                if (rolechk.RoleName == "SUPER ADMIN" || rolechk.RoleName == "ADMIN" || rolechk.RoleName == "ADMIN READER")
                                {
                                    SessionManager.RoleId = 1;
                                    SessionManager.UserId = res.UserID;
                                    ViewBag.RoleId = 1;
                                    return RedirectToAction("UserList");
                                }
                                else if (rolechk.RoleName == "DEV USER")
                                {
                                    SessionManager.RoleId = 0;
                                    SessionManager.UserId = res.UserID;
                                    var EmployeeMaster = _IEmployeeService.GetAll(null, null, "").Where(T => T.UserId == res.UserID).ToList();
                                    if (EmployeeMaster.Count() != 0)
                                    {
                                        SessionManager.IsOnBoarded = true;
                                    }
                                    else
                                    {
                                        SessionManager.IsOnBoarded = false;
                                    }
                                    ViewBag.RoleId = 0;
                                    return RedirectToAction("PersonalDetails");
                                }
                            }
                            else
                            {
                                #region Commented because rolemaster is not having entry of candiddate email TBD
                                //SessionManager.RoleId = 0;
                                //SessionManager.UserId = res.UserId;
                                //ViewBag.RoleId = 0;
                                //return RedirectToAction("PersonalDetails");
                                #endregion

                                #region Added instead of existing block

                                SessionManager.RoleId = 0;
                                SessionManager.UserId = res.UserID;
                                var EmployeeMaster = _IEmployeeService.GetAll(null, null, "").Where(T => T.UserId == res.UserID).ToList();
                                if (EmployeeMaster.Count() != 0)
                                {
                                    SessionManager.IsOnBoarded = true;
                                }
                                else
                                {
                                    SessionManager.IsOnBoarded = false;
                                }
                                ViewBag.RoleId = 0;

                                // Code change - Modified default redirection to Home welcome pages
                                return RedirectToAction("Welcome", "Home");
                                //return RedirectToAction("PersonalDetails");

                                #endregion
                            }
                            //Assign values to session veriables
                            //SessionManager.RoleId = res.RoleId;
                            //SessionManager.UserId = res.UserId;
                            //ViewBag.RoleId = res.RoleId;

                            //roleId 0 - for General user
                            //if (res.RoleId == 0)
                            //    return RedirectToAction("PersonalDetails");
                            //// roleId 1 - for Admin user
                            //else if (res.RoleId == 1)
                            //    return RedirectToAction("UserList");
                        }
                    }

                }
            }


            ModelState.AddModelError("", "Invalid Login");
            return View(details);

        }
        [HttpGet]
        public ActionResult LoginAs()
        {
            return View();
        }
        Master_Designation UpdateDesignationList(string designationName)
        {
            var designationList = GetDesignationList();
            //  var count = 0;
            var status = false;
            Master_Designation masterDesignation = null;
            if (designationList != null)
            {
                var designations = designationList.Where(d => d.Text == designationName);
                if (designations.Count() == 0)
                {
                    Master_Designation master_Designation = new Master_Designation();
                    master_Designation.Designation = designationName;
                    master_Designation.IsActive = true;
                    status = _IDesignationService.Insert(master_Designation, null, "");
                    masterDesignation = _IDesignationService.GetAll(null, null, "").LastOrDefault();
                }


            }
            return masterDesignation;
        }
        public ActionResult GetDecryptString()
        {
            string Encrypt = "U19TYXRoZV8xMjM=";
            string DeCryptString = SessionManager.DecryptData(Encrypt);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoginAs(string email)
        {
            var details = new ImpersonateUser() {Email = email};
            var user = _IUserService.GetAll(null, null, "").ToList();
            var chkDeactivateduser = user.Where(u => u.Email.ToLower().Trim() == details.Email.ToLower().Trim() && Convert.ToInt32(u.IsActive) == 2).FirstOrDefault();
            if (chkDeactivateduser != null)
            {
                ModelState.AddModelError("", "Your Login has been Deactivated");
                return View(details);
            }

            var res = user.Where(u => u.Email.ToLower().Trim() == details.Email.ToLower().Trim() && Convert.ToInt32(u.IsActive) == 1).FirstOrDefault();
            if (res != null)
            {
                UserImpersonation.ImpersonateUser(res.FirstName + "|" + res.UserID, string.Empty);
                return RedirectToAction("Welcome", "Home");
            }

            ModelState.AddModelError("", "User does not exist");
            return View(details);
        }




        /// <summary>
        /// Displays List of user for the application
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult UserList(int? page, string sortOrder, string searchString = "")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameParm = sortOrder == "Name_ASC" ? "Name_DESC" : "Name_ASC";
            ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";
            ViewBag.JoiningDateParm = sortOrder == "JoiningDate_ASC" ? "JoiningDate_DESC" : "JoiningDate_ASC";

            var userList = new List<LoginDetails>();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var user = _IUserService.GetAll(null, null, "").Where(m => (m.UserID != SessionManager.UserId) && (m.IsDelete == false) && (m.Email.ToLower().Contains(searchString) ||
                       m.FirstName.ToLower().Contains(searchString) || m.LastName.ToLower().Contains(searchString))).ToList();

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    if (sortOrder.Contains("Name_DESC"))
                        user = user.OrderByDescending(m => m.FirstName).ToList();
                    if (sortOrder.Contains("Name_ASC"))
                        user = user.OrderBy(m => m.FirstName).ToList();
                    if (sortOrder.Contains("Status_DESC"))
                    {
                        var user1 = user.Where(m => m.IsActive == 1).ToList();
                        var user2 = user.Where(m => m.IsActive == 0 || m.IsActive == 2).ToList();
                        var user3 = new List<LoginDetail>();
                        user3.AddRange(user2);
                        user3.AddRange(user1);
                        user.RemoveAll(m => m.IsActive == 1 || m.IsActive == 2 || m.IsActive == 0);
                        user.AddRange(user3);
                    }
                    if (sortOrder.Contains("Status_ASC"))
                    {

                        var user1 = user.Where(m => m.IsActive == 1).ToList();
                        var user2 = user.Where(m => m.IsActive == 0 || m.IsActive == 2).ToList();
                        var user3 = new List<LoginDetail>();
                        user3.AddRange(user1);
                        user3.AddRange(user2);
                        user.RemoveAll(m => m.IsActive == 1 || m.IsActive == 2 || m.IsActive == 0);
                        user.AddRange(user3);
                    }
                    if (sortOrder.Contains("JoiningDate_DESC"))
                        user = user.OrderByDescending(m => m.JoiningDate).ToList();
                    if (sortOrder.Contains("JoiningDate_ASC"))
                        user = user.OrderBy(m => m.JoiningDate).ToList();
                }
                else
                {
                    user = user.OrderByDescending(m => m.JoiningDate.Value).ToList();
                }


                var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

                LoginDetail empobj;
                foreach (var empitem in AdminEmployeeList)
                {
                    empobj = new LoginDetail();
                    empobj = user.Where(m => m.Email.ToUpper().Trim() == empitem.UserName.ToUpper().Trim()).FirstOrDefault();
                    if (empobj != null)
                    {
                        user.Remove(empobj);
                    }
                }

                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);  //5; // Code change - Minimize page size temporarily //Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
                int pageNumber = (page ?? 1);

                ViewBag.PageIndex = pageNumber;
                ViewBag.SearchString = searchString;
                ViewModel.CandidateProgressDetails candidateProgressDetails = new ViewModel.CandidateProgressDetails(_IUserService, _IRelationService, _ICandidateProgressDetailService, _IEmploymentCountService);
                foreach (var item in user)
                {
                    var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).FirstOrDefault();
                    var designation = _IUserService.GetDesignationName(item.DesignationID);
                    userList.Add(new LoginDetails()
                    {
                        //Code change - Added employee number for tooltip on onboarding status 
                        EmpNo = Employee == null ? string.Empty : Employee.EmpNo,
                        ActivatedDate = item.ActivatedDate,
                        Active = item.IsActive,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        DOB = (item.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(item.DOB)) : (DateTime?)null, //Change request - Null check, as DOB set null while adding user
                        JoiningDate = item.JoiningDate,
                        ShortDOB = (item.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))) : string.Empty,//convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))), //Change request - Null check, as DOB set null while adding user
                        ShortJoiningDate = convertDateToShort(item.JoiningDate),
                        UserId = item.UserID,
                        IsOnboarded = Employee == null ? false : true,
                        ContactNumber = item.ContactNumber,
                        DesignationID = item.DesignationID,
                        JoiningLocation = item.JoiningLocation,
                        Designation = designation,
                        OverAllUploadPecentage = candidateProgressDetails.SaveCandidateProgressDetails(Convert.ToInt32(item.UserID)).AverragePercentage
                    });
                }
                return View(userList.ToPagedList(pageNumber, pageSize));
            }
            return View(userList);
        }


        [HttpPost]
        [Authorize]
        [ValidateRole]
        public ActionResult ChangeRequestAction(long CandChangeReqID, bool Action)
        {
            string ErrorMessage = "";
            string strWebUrl = ConfigurationManager.AppSettings["WebUrl"];
            try
            {



                CandidateChangeRequestsDetail result = new CandidateChangeRequestsDetail();
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
                    result = _IUserService.ChangeRequestAction(CandChangeReqID, Action, userName);
                    if (result != null)
                    {
                        var status = (result.IsApproved.HasValue && result.IsApproved.Value);
                        string htmlBody = @"<html><body><font face='Cambria' size= '3' color ='black'> Dear " + result.LoginDetail.FirstName + ",<br><br>Your request for <strong>" + getUIValue(result.FieldName) + "</strong> change on the Enboard Portal has been " + ( status ? "Approved" : "denied.<br/>Please write to <a href='mailto:onboarding-india@silicus.com'>onboarding-india@silicus.com</a> for further information or contact the Onboarding team") + ".<br><br> Regards, <br/> Onboarding Team @ Silicus <br/><img "+"src='"+strWebUrl+"/Content/NewUI/images/sign/email_sign.png'"+"/><br/><font face='Cambria' Size= '3' color ='#2C567C'>Pune IT Park, 6th & 7th Floor, 34 Aundh Road,<br/>Bhau Patil Marg, Pune 411020<br/> Tel: +91.20.3020 4000<br/></font></body></html>";
                        string subject = "Enboard: Change Request " + ((result.IsApproved.HasValue && result.IsApproved.Value) ? "Approved" : "Denied");
                        SendMailToUser(result.LoginDetail, htmlBody, subject);
                        return this.Json(new { Sucess = true });
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;


            }
            return this.Json(new { Sucess = false, ErrorMessage = ErrorMessage });

        }

        private string getUIValue(string dbValue)
        {
            switch (dbValue)
            {
                case "FirstName":
                    return "First Name";
                    break;
                case "LastName":
                    return "Last Name";
                    break;
                case "DOB":
                    return "Date of Birth";
                    break;
                case "Email":
                    return "Email";
                    break;
                case "ContactNumber":
                    return "Contact Number";
                    break;

                case "CountryCode":
                    return "Country Code";
                    break;
                //case "NoOfEmployments":
                //    return "No. of Employment(s)";
                //    break;
                default:
                    return string.Empty;
                    break;
            }
        }
        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult CandidateChangeRequests(int? page, string sortOrder, string searchString = "")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameParm = sortOrder == "Name_ASC" ? "Name_DESC" : "Name_ASC";
            ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";
            ViewBag.JoiningDateParm = sortOrder == "JoiningDate_ASC" ? "JoiningDate_DESC" : "JoiningDate_ASC";
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);  //5; // Code change - Minimize page size temporarily //Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var result = _IUserService.GetCandidateChangeRequest(searchString);
                result.ForEach((x) =>
                {
                    x.FieldName = this.getUIValue(x.FieldName);
                });

                if (!string.IsNullOrWhiteSpace(sortOrder))
                {
                    if (sortOrder.Equals("Name_ASC", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = result.OrderBy(x => x.LoginDetail.FirstName).ToList();
                    }
                    else
                    {
                        result = result.OrderByDescending(x => x.LoginDetail.FirstName).ToList();

                    }


                }

                if (Request.IsAjaxRequest())
                {
                    return this.PartialView("_partialCandidateChangeRequestsDetail", result.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(result.ToPagedList(pageNumber, pageSize));
                }

            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult ReminderList(int? page)
        {
            var userList = new List<LoginDetails>();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var user = _IUserService.GetAll(null, null, "").Where(m => m.UserID != SessionManager.UserId && m.IsDelete == false && m.IsActive == 1).OrderByDescending(m => m.CreatedDate).ToList();

                var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

                LoginDetail empobj;
                foreach (var empitem in AdminEmployeeList)
                {
                    empobj = new LoginDetail();
                    empobj = user.Where(m => m.Email.ToUpper().Trim() == empitem.UserName.ToUpper().Trim()).FirstOrDefault();
                    if (empobj != null)
                    {
                        user.Remove(empobj);
                    }
                }

                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
                int pageNumber = (page ?? 1);

                foreach (var item in user)
                {
                    var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).FirstOrDefault();
                    userList.Add(new LoginDetails()
                    {
                        ActivatedDate = item.ActivatedDate,
                        Active = item.IsActive,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        DOB = (item.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(item.DOB)) : (DateTime?)null,//Convert.ToDateTime(SessionManager.DecryptData(item.DOB)),
                        JoiningDate = item.JoiningDate,
                        ShortDOB = (item.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))) : string.Empty,//convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))),
                        ShortJoiningDate = convertDateToShort(item.JoiningDate),
                        UserId = item.UserID,
                        IsOnboarded = Employee == null ? false : true,
                        LastLogin = item.LastLogin.Value
                    });
                }
                return View(userList.ToPagedList(pageNumber, pageSize));
            }
            return View(userList);
        }

        public ActionResult ShowReport()
        {
            return Redirect("../ReportPage.aspx");
        }

        /// <summary>
        /// Public function which will return string value from datetime field 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string convertDateToShort(DateTime? dt)
        {
            string shortdt = "";
            if (dt != null && dt.ToString() != "01/01/1900 00:00:00")
            {
                shortdt = Convert.ToDateTime(dt).ToString("dd/M/yyyy");
            }
            return shortdt;
        }
        /// <summary>
        /// GET for Personal Details Form For user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public JsonResult UpdateMasterResignation(string OtherDesignation)
        {
            return Json("");
        }

        public ActionResult PersonalDetails()
        {
            ViewBag.Languages = GetLanguages();
            ViewBag.BloodGroupList = GetBloodGroup();
            ViewBag.MaritalStatus = GetMaritalStatus();
            ViewBag.Countries = GetCountries();
            ViewBag.CountryCodeList = GetCountryCode();
            ViewBag.States = GetStates();
            ViewBag.Cities = GetCities();

            //Code change - EDMX Fix 
            var employeePersonalDetails = new Models.PersonalDetails();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {

                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                var PersonalDetails_lst = _IPersonalService.GetAll(null, null, "");
                Session["PersonalDetails_lst"] = PersonalDetails_lst;

                //Code change - EDMX Fix 
                var LoginDetails = _IUserService.GetById(userId);

                var PersonalDetails = PersonalDetails_lst.Where(u => u.UserID == userId).FirstOrDefault();


                if (PersonalDetails != null && LoginDetails != null)
                {
                    {
                        _personalDetails = PersonalDetails;
                        _Logindetails = LoginDetails;
                        employeePersonalDetails.FinalStatus = _Logindetails.IsSubmitted == null ? false : _Logindetails.IsSubmitted.Value;
                        employeePersonalDetails.FirstName = _Logindetails.FirstName;
                        employeePersonalDetails.LastName = _Logindetails.LastName;
                        employeePersonalDetails.ContactNumber = _Logindetails.ContactNumber;
                        employeePersonalDetails.EmpEmail = _Logindetails.Email;
                        employeePersonalDetails.DesignationID = _Logindetails.DesignationID;
                        employeePersonalDetails.Designation = _IUserService.GetDesignationName(_Logindetails.DesignationID);
                        employeePersonalDetails.DepartmentID = _Logindetails.DepartmentID;
                        employeePersonalDetails.Department = _IUserService.GetDepartmentName(_Logindetails.DepartmentID);
                        employeePersonalDetails.JoiningDate = (_Logindetails.JoiningDate != null) ? _Logindetails.JoiningDate.ToString() : string.Empty;
                        employeePersonalDetails.JoiningLocation = _Logindetails.JoiningLocation;
                        employeePersonalDetails.DateofBirth = (_Logindetails.DOB != null) ? SessionManager.DecryptData(_Logindetails.DOB) : string.Empty;//SessionManager.DecryptData(_Logindetails.DOB);
                        employeePersonalDetails.Gender = _Logindetails.Gender;
                        employeePersonalDetails.CountryCode = _Logindetails.CountryCode;
                        //advanced fields

                        employeePersonalDetails.FatherName = _personalDetails.FatherName;
                        employeePersonalDetails.Nationality = _personalDetails.Nationality;
                        employeePersonalDetails.BloodGroup = _personalDetails.BloodGroup;
                        employeePersonalDetails.MaritalStatID = _personalDetails.MaritalStatID;
                        employeePersonalDetails.MotherTongue = _personalDetails.MotherTongue;
                        employeePersonalDetails.PassportNumber = _personalDetails.PassportNumber;
                        employeePersonalDetails.PlaceofBirth = _personalDetails.PlaceofBirth;
                        if (!string.IsNullOrEmpty(_personalDetails.PlaceofBirth) && _personalDetails.PlaceofBirth.Split('-').Count() > 1)
                        {
                            employeePersonalDetails.BirthState = _personalDetails.PlaceofBirth.Split('-')[0];
                            employeePersonalDetails.BirthCity = _personalDetails.PlaceofBirth.Split('-')[1];
                        }
                        if (!string.IsNullOrEmpty(_personalDetails.OtherPlaceOfBirth) && _personalDetails.OtherPlaceOfBirth.Split('-').Count() > 1)
                        {
                            employeePersonalDetails.OtherBirthState = _personalDetails.OtherPlaceOfBirth.Split('-')[0];
                            employeePersonalDetails.OtherBirthCity = _personalDetails.OtherPlaceOfBirth.Split('-')[1];
                        }
                        else if(!string.IsNullOrEmpty(_personalDetails.OtherPlaceOfBirth))
                        {
                            employeePersonalDetails.OtherBirthCity = _personalDetails.OtherPlaceOfBirth;
                        }
                        employeePersonalDetails.OtherPlaceOfBirth = _personalDetails.OtherPlaceOfBirth;
                        employeePersonalDetails.AadharCardNumber = _personalDetails.AadharCardNumber;
                        employeePersonalDetails.PANNumber = _personalDetails.PANNumber;
                        employeePersonalDetails.UANNumber = _personalDetails.UANNumber;
                        employeePersonalDetails.PerDetID = _personalDetails.PerDetID;
                        employeePersonalDetails.SpouseName = _personalDetails.SpouseName;
                        employeePersonalDetails.HavePassport = _personalDetails.HavePassport;
                        employeePersonalDetails.NameOnPassport = _personalDetails.NameOnPassport;
                        employeePersonalDetails.PassportExpiryDate = _personalDetails.PassportExpiryDate;
                        employeePersonalDetails.OtherPlaceOfBirth = _personalDetails.OtherPlaceOfBirth;
                        //employeePersonalDetails.NumberOfEmployments = _personalDetails.NumberOfEmployments;
                    }
                }
                else
                {
                    _Logindetails = LoginDetails;

                    employeePersonalDetails.FirstName = _Logindetails.FirstName;
                    employeePersonalDetails.LastName = _Logindetails.LastName;
                    employeePersonalDetails.ContactNumber = _Logindetails.ContactNumber;
                    employeePersonalDetails.EmpEmail = _Logindetails.Email;
                    employeePersonalDetails.DesignationID = _Logindetails.DesignationID;
                    employeePersonalDetails.Designation = _IUserService.GetDesignationName(_Logindetails.DesignationID);
                    employeePersonalDetails.DepartmentID = _Logindetails.DepartmentID;
                    employeePersonalDetails.Department = _IUserService.GetDepartmentName(_Logindetails.DepartmentID);
                    employeePersonalDetails.JoiningDate = (_Logindetails.JoiningDate != null) ? _Logindetails.JoiningDate.ToString() : string.Empty;
                    employeePersonalDetails.JoiningLocation = _Logindetails.JoiningLocation;
                    employeePersonalDetails.DateofBirth = (_Logindetails.DOB != null) ? SessionManager.DecryptData(_Logindetails.DOB) : string.Empty;//SessionManager.DecryptData(_Logindetails.DOB);
                    employeePersonalDetails.Gender = _Logindetails.Gender;
                    employeePersonalDetails.CountryCode = _Logindetails.CountryCode;
                    employeePersonalDetails.ContactNumber = _Logindetails.ContactNumber;
                }
                var userDetails = _IUserService.GetById(userId);
                ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value == true;
            }

            Mapper.CreateMap<Models.PersonalDetails, Models.PersonalDetails>();
            var user = Mapper.Map<Models.PersonalDetails, Models.PersonalDetails>(employeePersonalDetails);
            user.DateofBirth = (user.DateofBirth != "") ? Convert.ToDateTime(user.DateofBirth).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            user.JoiningDate = (user.JoiningDate != "") ? Convert.ToDateTime(user.JoiningDate).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            return View(user);
        }
        [HttpGet]
        [Authorize]
        public ActionResult GetPersonalDetails(int userId)
        {
            ViewBag.Languages = GetLanguages();
            ViewBag.BloodGroupList = GetBloodGroup();
            ViewBag.MaritalStatus = GetMaritalStatus();
            ViewBag.Countries = GetCountries();
            ViewBag.CountryCodeList = GetCountryCode();
            ViewBag.CityList = GetCities();


            //Code change - EDMX Fix 
            var employeePersonalDetails = new Models.PersonalDetails();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var PersonalDetails_lst = _IPersonalService.GetAll(null, null, "");
                Session["PersonalDetails_lst"] = PersonalDetails_lst;

                //Code change - EDMX Fix 
                var LoginDetails = _IUserService.GetById(userId);

                var PersonalDetails = PersonalDetails_lst.Where(u => u.UserID == userId).FirstOrDefault();


                if (PersonalDetails != null && LoginDetails != null)
                {
                    {
                        _personalDetails = PersonalDetails;
                        _Logindetails = LoginDetails;
                        System.Web.HttpContext.Current.Session["CandidateEmail"] = _Logindetails.Email;
                        System.Web.HttpContext.Current.Session["UserID"] = _Logindetails.UserID;
                        employeePersonalDetails.FirstName = _Logindetails.FirstName;
                        employeePersonalDetails.LastName = _Logindetails.LastName;
                        employeePersonalDetails.ContactNumber = _Logindetails.ContactNumber;
                        employeePersonalDetails.EmpEmail = _Logindetails.Email;
                        employeePersonalDetails.DesignationID = _Logindetails.DesignationID;
                        employeePersonalDetails.Designation = _IUserService.GetDesignationName(_Logindetails.DesignationID);
                        employeePersonalDetails.DepartmentID = _Logindetails.DepartmentID;
                        employeePersonalDetails.Department = _IUserService.GetDepartmentName(_Logindetails.DepartmentID);
                        employeePersonalDetails.JoiningDate = (_Logindetails.JoiningDate != null) ? _Logindetails.JoiningDate.ToString() : string.Empty;
                        employeePersonalDetails.JoiningLocation = _Logindetails.JoiningLocation;
                        employeePersonalDetails.DateofBirth = (_Logindetails.DOB != null) ? SessionManager.DecryptData(_Logindetails.DOB) : string.Empty;//SessionManager.DecryptData(_Logindetails.DOB);
                        employeePersonalDetails.Gender = _Logindetails.Gender;
                        employeePersonalDetails.CountryCode = _Logindetails.CountryCode;
                        //advanced fields

                        employeePersonalDetails.FatherName = _personalDetails.FatherName;
                        employeePersonalDetails.Nationality = _personalDetails.Nationality;
                        employeePersonalDetails.BloodGroup = _personalDetails.BloodGroup;
                        employeePersonalDetails.MaritalStatID = _personalDetails.MaritalStatID;
                        employeePersonalDetails.MotherTongue = _personalDetails.MotherTongue;
                        employeePersonalDetails.PassportNumber = _personalDetails.PassportNumber;
                        employeePersonalDetails.PlaceofBirth = _personalDetails.PlaceofBirth;
                        employeePersonalDetails.OtherPlaceOfBirth = _personalDetails.OtherPlaceOfBirth;
                        employeePersonalDetails.AadharCardNumber = _personalDetails.AadharCardNumber;
                        employeePersonalDetails.PANNumber = _personalDetails.PANNumber;
                        employeePersonalDetails.UANNumber = _personalDetails.UANNumber;
                        employeePersonalDetails.PerDetID = _personalDetails.PerDetID;
                        employeePersonalDetails.SpouseName = _personalDetails.SpouseName;
                        employeePersonalDetails.HavePassport = _personalDetails.HavePassport;
                        employeePersonalDetails.NameOnPassport = _personalDetails.NameOnPassport;
                        employeePersonalDetails.PassportExpiryDate = _personalDetails.PassportExpiryDate;
                        //employeePersonalDetails.NumberOfEmployments = _personalDetails.NumberOfEmployments;

                    }
                }
                else
                {
                    _Logindetails = LoginDetails;
                    System.Web.HttpContext.Current.Session["CandidateEmail"] = _Logindetails.Email;
                    System.Web.HttpContext.Current.Session["UserID"] = _Logindetails.UserID;
                    employeePersonalDetails.FirstName = _Logindetails.FirstName;
                    employeePersonalDetails.LastName = _Logindetails.LastName;
                    employeePersonalDetails.ContactNumber = _Logindetails.ContactNumber;
                    employeePersonalDetails.EmpEmail = _Logindetails.Email;
                    employeePersonalDetails.DesignationID = _Logindetails.DesignationID;
                    employeePersonalDetails.Designation = _IUserService.GetDesignationName(_Logindetails.DesignationID);
                    employeePersonalDetails.DepartmentID = _Logindetails.DepartmentID;
                    employeePersonalDetails.Department = _IUserService.GetDepartmentName(_Logindetails.DepartmentID);
                    employeePersonalDetails.JoiningDate = (_Logindetails.JoiningDate != null) ? _Logindetails.JoiningDate.ToString() : string.Empty;
                    employeePersonalDetails.JoiningLocation = _Logindetails.JoiningLocation;
                    employeePersonalDetails.DateofBirth = (_Logindetails.DOB != null) ? SessionManager.DecryptData(_Logindetails.DOB) : string.Empty;//SessionManager.DecryptData(_Logindetails.DOB);
                }

            }
            //Change request 2 - fixed personal details issue , added null check for DOB
            Mapper.CreateMap<Models.PersonalDetails, Models.PersonalDetails>();
            var user = Mapper.Map<Models.PersonalDetails, Models.PersonalDetails>(employeePersonalDetails);
            user.DateofBirth = (user.DateofBirth != "") ? Convert.ToDateTime(user.DateofBirth).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            user.JoiningDate = (user.JoiningDate != "") ? Convert.ToDateTime(user.JoiningDate).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            return View("GetPersonalDetails", user);

        }
        /// <summary>
        ///  POST for Personal Details Form For user
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult PersonalDetails(PersonalDetails details)
        {
            bool status = false;

            if (ModelState.IsValid)
            {

                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                if (details.CreatedBy == null || details.CreatedBy == null)
                    details.CreatedBy = userName;
                if (details.CreatedDate == DateTime.MinValue || details.CreatedDate == null)
                    details.CreatedDate = DateTime.UtcNow;

                details.UpdatedBy = userName;
                details.UpdatedDate = DateTime.UtcNow;
                details.UserID = userId;
               // var p = !string.IsNullOrEmpty(details.BirthState);
                //var q = !string.IsNullOrEmpty(details.BirthCity);
                //var r = details.BirthState + "-" + details.BirthCity;
                if(!string.IsNullOrEmpty(details.BirthState) || !string.IsNullOrEmpty(details.BirthCity))
                {
                    details.PlaceofBirth = details.BirthState + "-" + details.BirthCity;
                }
                else
                {
                    details.PlaceofBirth = null;
                }
                

                if(!string.IsNullOrEmpty(details.OtherBirthState))
                {
                    details.OtherPlaceOfBirth = details.OtherBirthState.Trim() + "-" + details.OtherBirthCity.Trim();
                }
                else
                {
                    if(!string.IsNullOrEmpty(details.OtherBirthCity))
                    {
                        details.OtherPlaceOfBirth = details.OtherBirthCity;
                    }
                   
                }
                //  var c = details.OtherBirthState.Trim() + "-" + details.OtherBirthCity.Trim();

                //details.OtherPlaceOfBirth = !string.IsNullOrEmpty(details.OtherBirthState) || !string.IsNullOrEmpty(details.OtherBirthCity) ? details.OtherBirthState.Trim() + "-" + details.OtherBirthCity.Trim() : null;
                details.PlaceofBirth = !string.IsNullOrEmpty(details.BirthState) || !string.IsNullOrEmpty(details.BirthCity) ? details.BirthState + "-" + details.BirthCity : null;
                if (details.MotherTongue == "Other" && (!string.IsNullOrEmpty(details.SpecificLanguage)))
                {
                    details.MotherTongue = details.SpecificLanguage;
                }

                Mapper.CreateMap<PersonalDetails, Data.EmployeePersonalDetail>();
                var personalDetail = Mapper.Map<PersonalDetails, Data.EmployeePersonalDetail>(details);
                personalDetail.IsActive = true;
                if (personalDetail.PerDetID == 0)
                {
                    status = _IPersonalService.Insert(personalDetail, null, "");
                    if (status == true)
                    {
                        TempData["PDsucc"] = "Personal details added sucesfully";
                    }

                    return RedirectToAction("PersonalDetails");
                }
                else
                {
                    status = _IPersonalService.Update(personalDetail, null, "");
                    if (status == true)
                    {
                        TempData["PDsucc"] = "Personal details updated sucesfully";
                    }
                }
            }
            ViewBag.Languages = GetLanguages();
            ViewBag.BloodGroup = GetBloodGroup();
            ViewBag.MaritalStatus = GetMaritalStatus();
            var loginDetails = _IUserService.GetById(userId);
            if(loginDetails != null)
            {
                loginDetails.HasPassport = details.PassportNumber;
                loginDetails.HasPassport = "kjksfj";
                _IUserService.Update(loginDetails, null, "");
            }
        
       
            return Json(new { result = true, Message = "Personal details updated sucesfully" }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Check If user Personal details exists
        /// </summary>
        /// <returns></returns>
        public EmployeePersonalDetail checkPDExistforUser()
        {
            int userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            string userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var PersonalDetails_lst = _IPersonalService.GetAll(null, null, "");
            //EDMX Fix - 
            var obj = PersonalDetails_lst.Where(u => u.UserID == userId).SingleOrDefault();
            return obj;
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new LostPasswordModel());
        }

        // POST: Account/LostPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(LostPasswordModel model)
        {
            string strWebUrl = ConfigurationManager.AppSettings["WebUrl"];
            if (String.IsNullOrEmpty(model.Email))
            {
                ModelState.Remove("FirstName");
            }

            if (ModelState.IsValid)
            {
                var Users = _IUserService.GetAll(null, null, "").ToList();
                LoginDetail loginDetail = null;
                if (!string.IsNullOrEmpty(model.Email))
                {
                    loginDetail = Users.Where(u => u.Email.ToLower() == model.Email.ToLower()).SingleOrDefault();
                }
                else
                {
                    loginDetail = Users.Where(u => u.FirstName == model.FirstName
                                              && u.LastName == model.LastName
                                              && DateTime.Compare(Convert.ToDateTime(SessionManager.DecryptData(u.DOB)), Convert.ToDateTime(model.DOB.Value.ToShortDateString())) == 0).SingleOrDefault();
                }

                if (loginDetail != null)
                {
                    try
                    {
                        string content = "<font face='Cambria' size= '3' color ='black'> "
                                         + "Dear " + loginDetail.FirstName + ", <br><br> "
                                         + "Your login credentials to the Enboard Portal have been reset.You may now proceed to complete the"
                                         + "<br/> remaining process in the tool."
                                         + "<br/> Details are as below:<ul><br/><br/>"
                                         + "<li>UserName: <font color ='blue'>" + loginDetail.Email + " </font> </li>"
                                         + "<li>Password: <font color = 'blue'>" + SessionManager.DecryptData(loginDetail.Password) + " </font> </li></ul><br/>"
                                         + "<br/>Kindly feel free to reach out to the Onboarding team at Silicus in case of any questions. You can write to us at <a href='mailto:onboarding-india@silicus.com'>onboarding-india@silicus.com</a>."
                                         + "<font face='Cambria' size= '3'><br> <br><br> Regards, " +
                                         " <br/> Onboarding Team @ Silicus " +
                                         "<br/><img " + "src='"+strWebUrl+"/Content/NewUI/images/sign/email_sign.png'"+"/>" +
                                         " <br/><font face='Cambria' Size= '3' color ='#2C567C'>Pune IT Park, 6th & 7th Floor, 34 Aundh Road,<br/>Bhau Patil Marg, Pune 411020" +
                                         " <br/> Tel: +91.20.3020 4000<br/></font>" +
                                         "</body></html>";
                        SendMailToUser(loginDetail, content, "Enboard (Onboarding Portal) Credential Reset: Silicus Technologies");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Issue sending email: " + e.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No user found.");
                }
            }
            return View(model);
        }

        private bool SendMailToUser(LoginDetail model, string body, string subject, string strto = null, Boolean isAttachment = false, string zipFilePath = null)
        {
            SmtpClient client = new SmtpClient();
            _logger.Log("SendEmailToUser-Start");
            try
            {
                string SMTPUserName = ConfigurationManager.AppSettings["SMTPUserName"];
                string SMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"];

                NetworkCredential basicCredential = new NetworkCredential(SMTPUserName, SMTPPassword);
                string From = ConfigurationManager.AppSettings["EmailFrom"];
                _logger.Log("SendEmailToUser-From"+ From  );
                string To = model.Email;
                _logger.Log("SendEmailToUser-To"+To);
                if (strto != null)
                    To = strto;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(From, To, subject, body);
                mailMessage.From = new MailAddress(From, From);
                mailMessage.IsBodyHtml = true;
                if (isAttachment)
                {
                    Attachment data = new Attachment(zipFilePath);
                    mailMessage.Attachments.Add(data);
                }


                client.Host = ConfigurationManager.AppSettings["HostName"];
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                _logger.Log("SendEmailToUser-before-QueueUserWorkItem");
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ThreadPool.QueueUserWorkItem(t =>
                {
                    client.SendCompleted += (s, e) =>
                    {
                        client.Dispose();
                        mailMessage.Dispose();

                        if (e.Cancelled)
                        {
                            _logger.Log("SendEmailToUser-Cancelled");
                            // prompt user with "send cancelled" message 
                        }
                        if (e.Error != null)
                        {
                            _logger.Log("SendEmailToUser-error"+e.Error.Message);
                            // prompt user with error message 
                        }
                        else
                        {
                            _logger.Log("SendEmailToUser-Sent");
                            // prompt user with message sent!
                            // as we have the message object we can also display who the message
                            // was sent to etc 
                        }
                    };
                    client.SendMailAsync(mailMessage);
                    _logger.Log("SendEmailToUser-QueueUserWorkItem");
                });

                TempData["emailsucc"] = "Password is sent to your Email address.";
            }
            catch (Exception ex)
            {
                _logger.Log("Before Dispose");
                client.Dispose();
                _logger.Log(ex);
                _logger.Log("SendEmailToUser-"+ex.Message);
                throw ex;

            }
            _logger.Log("SendEmailToUser-End");
            return true;
        }


        private bool SendMailToUserAsReminder(LoginDetail model, string body, string subject, string strto = null, string strCC = null, Boolean isAttachment = false, string zipFilePath = null)
        {

            string From = ConfigurationManager.AppSettings["EmailFrom"];
            string To = model.Email;
            if (strto != "")
                To = strto;
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(From, To, subject, body);
            mailMessage.IsBodyHtml = true;
            if (strCC != "")
                mailMessage.CC.Add(strCC);

            if (isAttachment)
            {
                Attachment data = new Attachment(zipFilePath);
                mailMessage.Attachments.Add(data);
            }
            SmtpClient client = new SmtpClient();

            client.Host = ConfigurationManager.AppSettings["HostName"];
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
            client.UseDefaultCredentials = true;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ThreadPool.QueueUserWorkItem(t =>
                {
                    client.SendCompleted += (s, e) =>
                    {
                        client.Dispose();
                        mailMessage.Dispose();

                        if (e.Cancelled)
                        {
                            // prompt user with "send cancelled" message 
                        }
                        if (e.Error != null)
                        {
                            // prompt user with error message 
                        }
                        else
                        {
                            // prompt user with message sent!
                            // as we have the message object we can also display who the message
                            // was sent to etc 
                        }
                    };
                    client.SendMailAsync(mailMessage);

                });

                TempData["emailsucc"] = "Password is sent to your Email address.";
            }
            catch (Exception ex)
            {
                client.Dispose();
            }
            return true;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ContactDetails()
        {
            bool? detailsExist = null;
            ViewBag.States = GetStates();
            ViewBag.Cities = GetCities();
            ViewBag.Countries = GetCountries();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];


                var ContactDetails_lst = _IContactService.GetAll(null, null, "");

                var obj = ContactDetails_lst.Where(u => u.UserID == userId).ToList();
                if (obj.Count != 0)
                {
                    _contactDetails = obj.FirstOrDefault();
                    detailsExist = true;
                }
                else
                {
                    detailsExist = false;
                    var log = _IUserService.GetById(userId);
                    if (log != null)
                    {
                        _contactDetails.Email = log.Email;
                    }

                }

            }

            Mapper.CreateMap<Data.EmployeeContactDetail, Models.ContactDetails>();
            var user = Mapper.Map<Data.EmployeeContactDetail, Models.ContactDetails>(_contactDetails);

            user.PermanantCountryID = Convert.ToInt32(user.PermanantCountryID);
            user.PermanantStateID = Convert.ToInt32(user.PermanantStateID);
            user.PermanantCityID = Convert.ToInt32(user.PermanantCityID);

            user.CurrentCountryID = Convert.ToInt32(user.CurrentCountryID);
            user.CurrentStateID = Convert.ToInt32(user.CurrentStateID);
            user.CurrentCityID = Convert.ToInt32(user.CurrentCityID);
            user.IsBothAddSame = (detailsExist != null && detailsExist == false) ? true : user.IsBothAddSame;
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value == true;
            return View(user);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetContactDetails(int userId)
        {
            ViewBag.States = GetStates();
            ViewBag.Cities = GetCities();
            ViewBag.Countries = GetCountries();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {

                var ContactDetails_lst = _IContactService.GetAll(null, null, "");

                var obj = ContactDetails_lst.Where(u => u.UserID == userId).ToList();
                if (obj.Count != 0)
                {
                    _contactDetails = obj.FirstOrDefault();
                }
                else
                {
                    var log = _IUserService.GetById(userId);
                    if (log != null)
                    {
                        _contactDetails.Email = log.Email;
                    }

                }

            }

            Mapper.CreateMap<Data.EmployeeContactDetail, Models.ContactDetails>();
            var user = Mapper.Map<Data.EmployeeContactDetail, Models.ContactDetails>(_contactDetails);

            user.PermanantCountryID = Convert.ToInt32(user.PermanantCountryID);
            user.PermanantStateID = Convert.ToInt32(user.PermanantStateID);
            user.PermanantCityID = Convert.ToInt32(user.PermanantCityID);

            user.CurrentCountryID = Convert.ToInt32(user.CurrentCountryID);
            user.CurrentStateID = Convert.ToInt32(user.CurrentStateID);
            user.CurrentCityID = Convert.ToInt32(user.CurrentCityID);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ContactDetails(ContactDetails details)
        {

            if (details.IsBothAddSame)
            {
                details.PermanantAddLine1 = details.CurrentAddLine1;
                details.PermanantAddLine2 = details.CurrentAddLine2;
                details.PermanantAddLine3 = details.CurrentAddLine3;

                details.PermanantCountryID = details.CurrentCountryID;
                details.PermanantStateID = details.CurrentStateID;
                details.PermanantCityID = details.CurrentCityID;
                ModelState.Remove("PermanantCityID");
                ModelState.Remove("PermanantStateID");
                ModelState.Remove("PermanantCountryID");
            }

            bool status = false;
            string userName = null;

            if (ModelState.IsValid)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                if (details.CreatedBy == null || details.CreatedBy == null)
                    details.CreatedBy = userName;
                if (details.CreatedDate == DateTime.MinValue || details.CreatedDate == null)
                    details.CreatedDate = DateTime.UtcNow;

                details.UpdatedBy = userName;
                details.UpdatedDate = DateTime.UtcNow;//DateTime.UtcNow;
                details.UserID = userId;


                Mapper.CreateMap<ContactDetails, Data.EmployeeContactDetail>();
                var contactDetail = Mapper.Map<ContactDetails, Data.EmployeeContactDetail>(details);

                if (contactDetail.ConDetID == 0)
                {

                    status = _IContactService.Insert(contactDetail, null, "");
                    if (status == true)
                    {
                        TempData["CDsucc"] = "Contact details added sucesfully";
                    }

                    return RedirectToAction("ContactDetails");
                }
                else
                {
                    status = _IContactService.Update(contactDetail, null, "");
                    if (status == true)
                    {
                        TempData["CDsucc"] = "Contact details updated sucesfully";
                    }
                }
            }
            ViewBag.States = GetStates();
            ViewBag.Cities = GetCities();
            ViewBag.Countries = GetCountries();

            return View(details);
        }

        public SelectList GetLanguages()
        {
            List<Master_Language> langList = new List<Master_Language>();

            langList = _ILanguageservice.GetAll(null, null, "").ToList();
            SelectList selList = new SelectList(langList, "Language", "Language");
            return selList;
        }



        public SelectList GetBloodGroup()
        {
            List<Master_Bloodgroup> List = new List<Master_Bloodgroup>();

            List = _IUserService.GetBloodGroupList().ToList();
            List = List.OrderBy(x => x.BloodgGroup).ToList();
            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "BoodGroupID", "BloodgGroup");
            return selList;
            //return List;

            //List<BloodGroup> BloodgrpList = new List<BloodGroup>();
            //BloodGroup obj = new BloodGroup();
            //obj.Text = "A+";
            //obj.Value = 1;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "A-";
            //obj.Value = 2;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "B+";
            //obj.Value = 3;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "B-";
            //obj.Value = 4;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "AB+";
            //obj.Value = 5;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "AB-";
            //obj.Value = 6;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "O+";
            //obj.Value = 7;
            //BloodgrpList.Add(obj);

            //obj = new BloodGroup();
            //obj.Text = "O-";
            //obj.Value = 8;
            //BloodgrpList.Add(obj);

            //SelectList sellist = new SelectList(BloodgrpList, "Text", "Text");
            //return sellist;
        }

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

        public SelectList GetCities()
        {
            List<Master_City> List = new List<Master_City>();

            List = _ICityService.GetAll(null, null, "").ToList();

            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "CityID", "City");
            return selList;
        }

        public SelectList GetStates()
        {
            List<Master_State> List = new List<Master_State>();

            List = _IStateService.GetAll(null, null, "").ToList();

            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "StateID", "State");
            return selList;
        }

        public SelectList GetCountries()
        {
            List<Master_Country> List = new List<Master_Country>();

            List = _ICountryService.GetAll(null, null, "").ToList();

            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "CountryID", "Country");
            return selList;
        }

        public SelectList GetMaritalStatus()
        {
            List<Master_MaritalStatus> List = new List<Master_MaritalStatus>();

            List = _IMaritalStatusService.GetAll(null, null, "").ToList();
            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "MaritalStatID", "MaritalStatus");
            return selList;
        }

        [HttpPost]
        public ActionResult LoadStateByCountryId(string CountryId)
        {

            List<Master_State> List = new List<Master_State>();

            if (!string.IsNullOrEmpty(CountryId))
            {
                List = _IStateService.GetAll(null, null, "").ToList();
                List = List.Where(x => x.CountryID == Convert.ToInt32(CountryId)).OrderBy(x => x.State).ToList();
            }

            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "StateID", "State");

            return Json(selList);
        }

        [HttpPost]
        public ActionResult LoadCityByStateId(string StateId)
        {

            List<Master_City> List = new List<Master_City>();

            if (!string.IsNullOrEmpty(StateId))
            {
                List = _ICityService.GetAll(null, null, "").ToList();
                List = List.Where(x => x.StateID == Convert.ToInt32(StateId)).OrderBy(x => x.City).ToList();
            }

            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "CityID", "City");

            return Json(selList);
        }

        public ActionResult ActivateDeactivateUser(int userId)
        {
            LoginDetail user = _IUserService.GetById(userId);
            if (user != null)
            {
                string htmlBody = string.Empty;
                string subject = string.Empty;
                string strWebUrl = ConfigurationManager.AppSettings["WebUrl"];
                if (user.IsActive == 0)
                {
                    user.IsActive = 1;
                    DateTime dt = Convert.ToDateTime(user.JoiningDate);
                    var monthName = dt.ToString("MMMM", CultureInfo.InvariantCulture);
                    string strJoiningDate = dt.DayOfWeek.ToString() + " " + dt.Day + " " + monthName + " " + dt.Year + " ";
                    string hrEmailID = ConfigurationManager.AppSettings["HREmailId"];
                    htmlBody = @"<html><body><font face='Cambria' size= '3' color ='black'> Dear " 
                                + user.FirstName + ",<br><br>" +
                               "Congratulations on your selection at Silicus! We look forward towards your joining on <strong>" +strJoiningDate+"</strong>"+
                               ".<br/>Please login to the Silicus Onboarding Portal – <strong>Enboard</strong> – to initiate the pre-joining process" +
                              "<br><br>The credentials are as below:<ul><li><a href="
                               + strWebUrl + " target='_blank'>Click</a> here to login </li><li> UserName: <font color ='blue'>"
                               + user.Email.Trim() + "</font>  </li><li> Password: <font color ='blue'>" + SessionManager.DecryptData(user.Password).Trim()
                               + "</font> </li></ul> <br/> " 
                               + "You are required to fill the necessary forms and upload all the documents as listed in the ‘Joining Document Checklist’ issued to you along with the Offer letter." 
                               +
                               " <br/> Kindly feel free to reach out to the Onboarding team at Silicus in case of any questions." +
                               " You can write to us at <a href='mailto:onboarding-india@silicus.com'>onboarding-india@silicus.com</a>. " +
                               " <br><br> Regards, <br/> Onboarding Team @ Silicus <br/><img " + "src='"+strWebUrl+"/Content/NewUI/images/sign/email_sign.png'" + "/><br/><font face='Cambria' Size= '3' color ='#2C567C'> <br/>Pune IT Park, 6th & 7th Floor, 34 Aundh Road,<br/>Bhau Patil Marg, Pune 411020 <br/> Tel: +91.20.3020 4000 <br/></font>" +
                               "</body></html>";
                    subject = "Enboard (Onboarding Portal) login credentials: Silicus Technologies";
                }
                else if (user.IsActive == 1)
                {
                    user.IsActive = 2;
                }
                else if (user.IsActive == 2)
                {
                    user.IsActive = 1;
                    htmlBody = @"<html><body><font face='Cambria' size= '3' color ='black'> Dear " + user.FirstName + ",<br><br>Your login to Enboard has been reactivated. Please login and complete the remaining process in the tool.<br><br>" +
                               "Your login credentials are as below:<ul>" +
                               "<li><a href=" + strWebUrl + " target='_blank'>Click</a> to open the portal </li>" +
                               "<li> UserName: <font color ='blue'>" + user.Email.Trim() + "</font>  </li>" +
                               "<li> Password: <font color ='blue'>" + SessionManager.DecryptData(user.Password).Trim() + "</font></li>" +
                               "</ul><br/>Kindly feel free to reach out to the Onboarding team at Silicus in case of any questions.<br/> " +
                               "You can write to us at <a href='mailto:onboarding-india@silicus.com'>onboarding-india@silicus.com</a>.<br><br> Regards," +
                               "<br/> Onboarding Team @ Silicus<br/>" +
                               "<img" + "src='"+strWebUrl+"/Content/NewUI/images/sign/email_sign.png'" + "/>" +
                               "<br/> <font face='Cambria' Size= '3' color ='#2C567C'> Pune IT Park, 6th & 7th Floor, 34 Aundh Road," +
                               "<br/>Bhau Patil Marg, Pune 411020 <br/> Tel: +91.20.3020 4000 <font/></body></html>";
                    subject = "Enboard (Onboarding Portal) Reactivation credentials: Silicus Technologies";
                }
                _IUserService.Update(user, null, null);

                if (user.IsActive == 1)
                    SendMailToUser(user, htmlBody, subject);

                return RedirectToAction("UserList", "User", new { redirectCall = true });
            }
            return Json(-1);
        }

        [HttpGet]
        [CLSCompliant(false)]
        [ValidateRole]
        public ActionResult GetUserList(string searchString, string sortOrder, int? pageIndex = null, bool redirectCall = false)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameParm = String.IsNullOrEmpty(sortOrder) ? "Name_DESC" : ""; //
            ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";
            ViewBag.JoiningDateParm = sortOrder == "JoiningDate_ASC" ? "JoiningDate_DESC" : "JoiningDate_ASC";

            try
            {
                var userList = new List<LoginDetails>();
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    // Get filted user on text
                    var user = _IUserService.GetAll(null, null, "").Where(m => (m.UserID != SessionManager.UserId) && (m.IsDelete == false) && (m.Email.ToLower().Contains(searchString) ||
                        (m.FirstName.ToLower() + " " + m.LastName.ToLower()).Contains(searchString))).ToList();

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        if (sortOrder.Contains("Name_DESC"))
                            user = user.OrderByDescending(m => m.FirstName).ToList();
                        if (sortOrder.Contains("Name_ASC"))
                            user = user.OrderBy(m => m.FirstName).ToList();
                        if (sortOrder.Contains("Status_DESC"))
                        {
                            var user1 = user.Where(m => m.IsActive == 1).ToList();
                            var user2 = user.Where(m => m.IsActive == 0 || m.IsActive == 2).ToList();
                            var user3 = new List<LoginDetail>();
                            user3.AddRange(user2);
                            user3.AddRange(user1);
                            user.RemoveAll(m => m.IsActive == 1 || m.IsActive == 2 || m.IsActive == 0);
                            user.AddRange(user3);
                        }
                        if (sortOrder.Contains("Status_ASC"))
                        {

                            var user1 = user.Where(m => m.IsActive == 1).ToList();
                            var user2 = user.Where(m => m.IsActive == 0 || m.IsActive == 2).ToList();
                            var user3 = new List<LoginDetail>();
                            user3.AddRange(user1);
                            user3.AddRange(user2);
                            user.RemoveAll(m => m.IsActive == 1 || m.IsActive == 2 || m.IsActive == 0);
                            user.AddRange(user3);
                        }
                        if (sortOrder.Contains("JoiningDate_DESC"))
                            user = user.OrderByDescending(m => m.JoiningDate).ToList();
                        if (sortOrder.Contains("JoiningDate_ASC"))
                            user = user.OrderBy(m => m.JoiningDate).ToList();
                    }
                    else
                    {
                        user = user.OrderByDescending(m => m.JoiningDate).ToList();
                    }

                    int page;

                    var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

                    LoginDetail empobj;
                    foreach (var empitem in AdminEmployeeList)
                    {
                        empobj = new LoginDetail();
                        empobj = user.Where(m => m.Email.Trim().ToUpper() == empitem.UserName.Trim().ToUpper()).FirstOrDefault();
                        if (empobj != null)
                        {
                            user.Remove(empobj);
                        }
                    }


                    if (Request.HttpMethod != "GET")
                    {
                        page = 1;
                    }

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);
                    int pageNumber = (pageIndex ?? 1);
                    ViewBag.PageIndex = pageNumber;
                    ViewBag.SearchString = searchString;
                    ViewModel.CandidateProgressDetails candidateProgressDetails = new ViewModel.CandidateProgressDetails(_IUserService, _IRelationService, _ICandidateProgressDetailService, _IEmploymentCountService);
                    // Assign data to model
                    foreach (var item in user)
                    {
                        var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).FirstOrDefault();
                        var designation = _IUserService.GetDesignationName(item.DesignationID);
                        userList.Add(new LoginDetails()
                        {
                            //Code Change - Added employee number for tooltip on onboarding status
                            EmpNo = Employee == null ? string.Empty : Employee.EmpNo,
                            ActivatedDate = item.ActivatedDate,
                            Active = item.IsActive,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            DOB = (item.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(item.DOB)) : (DateTime?)null, //Change request - Null check, as DOB set null while adding user
                            JoiningDate = item.JoiningDate,
                            ShortDOB = (item.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))) : string.Empty,//convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))), //Change request - Null check, as DOB set null while adding user
                            ShortJoiningDate = convertDateToShort(item.JoiningDate),
                            UserId = item.UserID,
                            IsOnboarded = Employee == null ? false : true,
                            ContactNumber = item.ContactNumber,
                            DesignationID = item.DesignationID,
                            JoiningLocation = item.JoiningLocation,
                            Designation = designation,
                            OverAllUploadPecentage = candidateProgressDetails.SaveCandidateProgressDetails(Convert.ToInt32(item.UserID)).AverragePercentage
                        });
                    }
                    return PartialView("~/Views/user/_partialUserList.cshtml", userList.ToPagedList(pageNumber, pageSize));
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpGet]
        [CLSCompliant(false)]
        [ValidateRole]
        public ActionResult GetUserListSorted(string sortOrder, int pageIndex, string searchString = "")
        {
            try
            {

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameParm = sortOrder == "Name_ASC" ? "Name_DESC" : "Name_ASC";
                ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";
                ViewBag.JoiningDateParm = sortOrder == "JoiningDate_ASC" ? "JoiningDate_DESC" : "JoiningDate_ASC";
                var userList = new List<LoginDetails>();
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    // Get filted user on text
                    var user = _IUserService.GetAll(null, null, "").Where(m => (m.UserID != SessionManager.UserId) && (m.IsDelete == false) && (m.Email.ToLower().Contains(searchString) ||
                        m.FirstName.ToLower().Contains(searchString) || m.LastName.ToLower().Contains(searchString))).ToList();

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        if (sortOrder.Contains("Name_DESC"))
                            user = user.OrderByDescending(m => m.FirstName).ToList();
                        if (sortOrder.Contains("Name_ASC"))
                            user = user.OrderBy(m => m.FirstName).ToList();
                        if (sortOrder.Contains("Status_DESC"))
                        {
                            var user1 = user.Where(m => m.IsActive == 1).ToList();
                            var user2 = user.Where(m => m.IsActive == 0 || m.IsActive == 2).ToList();
                            var user3 = new List<LoginDetail>();
                            user3.AddRange(user2);
                            user3.AddRange(user1);
                            user.RemoveAll(m => m.IsActive == 1 || m.IsActive == 2 || m.IsActive == 0);
                            user.AddRange(user3);
                        }

                        if (sortOrder.Contains("Status_ASC"))
                        {

                            var user1 = user.Where(m => m.IsActive == 1).ToList();
                            var user2 = user.Where(m => m.IsActive == 0 || m.IsActive == 2).ToList();
                            var user3 = new List<LoginDetail>();
                            user3.AddRange(user1);
                            user3.AddRange(user2);
                            user.RemoveAll(m => m.IsActive == 1 || m.IsActive == 2 || m.IsActive == 0);
                            user.AddRange(user3);
                        }
                        if (sortOrder.Contains("JoiningDate_DESC"))
                            user = user.OrderByDescending(m => m.JoiningDate).ToList();
                        if (sortOrder.Contains("JoiningDate_ASC"))
                            user = user.OrderBy(m => m.JoiningDate).ToList();
                    }
                    else
                    {
                        user = user.OrderBy(m => m.FirstName).ToList();
                    }

                    int page;
                    var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

                    LoginDetail empobj;
                    foreach (var empitem in AdminEmployeeList)
                    {
                        empobj = new LoginDetail();
                        empobj = user.Where(m => m.Email.Trim().ToUpper() == empitem.UserName.Trim().ToUpper()).FirstOrDefault();
                        if (empobj != null)
                        {
                            user.Remove(empobj);
                        }
                    }


                    if (Request.HttpMethod != "GET")
                    {
                        page = 1;
                    }

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);
                    int pageNumber = 1;
                    pageNumber = pageIndex != 1 ? pageIndex : 1;

                    ViewBag.PageIndex = pageNumber;
                    ViewBag.SearchString = searchString;
                    ViewModel.CandidateProgressDetails candidateProgressDetails = new ViewModel.CandidateProgressDetails(_IUserService, _IRelationService, _ICandidateProgressDetailService, _IEmploymentCountService);
                    // Assign data to model
                    foreach (var item in user)
                    {
                        var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).FirstOrDefault();

                        userList.Add(new LoginDetails()
                        {
                            //Code Change - Added employee number for tooltip on onboarding status
                            EmpNo = Employee == null ? string.Empty : Employee.EmpNo,
                            ActivatedDate = item.ActivatedDate,
                            Active = item.IsActive,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            DOB = (item.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(item.DOB)) : (DateTime?)null, //Convert.ToDateTime(SessionManager.DecryptData(item.DOB)),
                            JoiningDate = item.JoiningDate,
                            ShortDOB = (item.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))) : string.Empty,//convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))),
                            ShortJoiningDate = convertDateToShort(item.JoiningDate),
                            UserId = item.UserID,
                            IsOnboarded = Employee == null ? false : true,

                            //code change 
                            ContactNumber = item.ContactNumber,
                            OverAllUploadPecentage = candidateProgressDetails.SaveCandidateProgressDetails(Convert.ToInt32(item.UserID)).AverragePercentage


                        });
                    }
                    return PartialView("~/Views/user/_partialUserList.cshtml", userList.ToPagedList(pageNumber, pageSize));
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw;
            }

        }




        [HttpGet]
        [CLSCompliant(false)]
        [ValidateRole]
        public ActionResult GetReminderList(string searchString, int? pageIndex = null, bool redirectCall = false)
        {
            try
            {
                var userList = new List<LoginDetails>();
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    // Get filted user on text
                    var user = _IUserService.GetAll(null, null, "").Where(m => (m.UserID != SessionManager.UserId) && (m.IsDelete == false) && (m.IsActive == 1) && (m.Email.ToLower().Contains(searchString) ||
                        m.FirstName.ToLower().Contains(searchString) || m.LastName.ToLower().Contains(searchString))).OrderByDescending(m => m.CreatedDate).ToList();

                    int page;


                    var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

                    LoginDetail empobj;
                    foreach (var empitem in AdminEmployeeList)
                    {
                        empobj = new LoginDetail();
                        empobj = user.Where(m => m.Email.Trim().ToUpper() == empitem.UserName.Trim().ToUpper()).FirstOrDefault();
                        if (empobj != null)
                        {
                            user.Remove(empobj);
                        }
                    }


                    if (Request.HttpMethod != "GET")
                    {
                        page = 1;
                    }

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);
                    int pageNumber = 1;


                    // Assign data to model
                    foreach (var item in user)
                    {
                        var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).FirstOrDefault();
                        userList.Add(new LoginDetails()
                        {
                            ActivatedDate = item.ActivatedDate,
                            Active = item.IsActive,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            DOB = (item.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(item.DOB)) : (DateTime?)null,//Convert.ToDateTime(SessionManager.DecryptData(item.DOB)),
                            JoiningDate = item.JoiningDate,
                            ShortDOB = (item.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))) : string.Empty,//convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))),
                            ShortJoiningDate = convertDateToShort(item.JoiningDate),
                            UserId = item.UserID,
                            IsOnboarded = Employee == null ? false : true,
                            LastLogin = item.LastLogin.Value
                        });
                    }
                    return PartialView("~/Views/user/_partialReminderList.cshtml", userList.ToPagedList(pageNumber, pageSize));
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult DocList(int? page = 1, string sortOrder = "", string searchString = "")
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CandidateNameParm = sortOrder == "CandidateName_ASC" ? "CandidateName_DESC" : "CandidateName_ASC";
            ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";

            List<DocumentStatus_Result> list;
            var user = _IUserService.GetAll(null, null, "").Where(m => (m.UserID != SessionManager.UserId) && (m.Email.ToLower().Contains(searchString) || m.FirstName.ToLower().Contains(searchString) || m.LastName.ToLower().Contains(searchString)));
            if (string.IsNullOrEmpty(searchString))
                list = _IUserService.DocumentStatusList("0"); // Passing 0 for all users
            else
            {
                searchString = searchString.ToLower();
                list = _IUserService.DocumentStatusList("0").Where(m => m.CandidateName.ToLower().Contains(searchString) || m.DocumentName.ToLower().Contains(searchString) || m.Category.ToLower().Contains(searchString) || m.Document.ToLower().Contains(searchString)).ToList(); // Passing 0 for all users
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.Contains("CandidateName_DESC"))
                    list = list.OrderByDescending(m => m.CandidateName).ToList();
                if (sortOrder.Contains("CandidateName_ASC"))
                    list = list.OrderBy(m => m.CandidateName).ToList();
                if (sortOrder.Contains("Status_DESC"))
                    list = list.OrderByDescending(m => m.IsVerify).ToList();
                if (sortOrder.Contains("Status_ASC"))
                    list = list.OrderBy(m => m.IsVerify).ToList();
            }
            else
            {
                list = list.OrderBy(m => m.CandidateName).ToList();
            }
            // var intMandatoryDocs = _ISubDocumentCategoryService.GetAll(null, null, "").Where(x => x.IsNeeded == true).Count();

            List<DocDetailsModel> model = new List<DocDetailsModel>();

            string strWebUrl = ConfigurationManager.AppSettings["DocumentPath"];

            model = list
           .Select(x => new DocDetailsModel()
           {
               Id = x.DocDetID,
               UserId = x.UserId,
               CandidateName = x.CandidateName,
               DocumentName = x.DocumentName,
               FilePath = strWebUrl + x.FilePath,
               Maincategory = x.Category,//x.DocumentCategoryName, 
               SubCategory = x.Document,//x.SubDocCategoryName,
               IsVerify = x.IsVerify,
               Status = x.Status,
               // MandatoryDocs = intMandatoryDocs
           })
           .ToList();

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //create group by on candidate 
            var GroupCandidate = (from obj in model
                                  group obj by obj.CandidateName into empg
                                  select new
                                  {
                                      NAME = empg.Key,
                                      DocumentCount = empg.Count(x => x.Id > 0 ? true : false),
                                      ApprovedDocs = empg.Count(x => (x.IsVerify.HasValue ? x.IsVerify.Value == true : false))
                                      ,
                                      RejectedDocs = empg.Count(x => x.IsVerify.HasValue ? x.IsVerify.Value == false : false)
                                  }).ToList();

            List<DocDetailsModel> data = new List<DocDetailsModel>();

            foreach (var item in GroupCandidate)
            {
                if (item.DocumentCount == item.ApprovedDocs)
                {
                    data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        CandidateName = x.CandidateName,
                        DocumentName = x.DocumentName,
                        FilePath = strWebUrl + x.FilePath,
                        Maincategory = x.Maincategory,
                        SubCategory = x.SubCategory,
                        IsVerify = x.IsVerify,
                        Status = x.Status,
                        // MandatoryDocs = intMandatoryDocs,
                        isAllApproved = true
                    }).ToList());
                }
                else if (item.DocumentCount == item.RejectedDocs)
                {
                    data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        CandidateName = x.CandidateName,
                        DocumentName = x.DocumentName,
                        FilePath = strWebUrl + x.FilePath,
                        Maincategory = x.Maincategory,
                        SubCategory = x.SubCategory,
                        IsVerify = x.IsVerify,
                        Status = x.Status,
                        //MandatoryDocs = intMandatoryDocs,
                        isAllRejected = true
                    }).ToList());
                }
                else
                {
                    data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        CandidateName = x.CandidateName,
                        DocumentName = x.DocumentName,
                        FilePath = strWebUrl + x.FilePath,
                        Maincategory = x.Maincategory,
                        SubCategory = x.SubCategory,
                        IsVerify = x.IsVerify,
                        Status = x.Status,
                        //MandatoryDocs = intMandatoryDocs                      
                    }).ToList());
                }
            }


            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
            int pageNumber = 1;
            pageNumber = page.Value != 1 ? page.Value : 1;
            ViewBag.PageIndex = pageNumber;
            ViewBag.SearchString = searchString;

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [CLSCompliant(false)]
        [ValidateRole]
        public ActionResult GetDocListSorted(string sortOrder, int pageIndex = 1, string searchString = "")
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.CandidateNameParm = sortOrder == "CandidateName_ASC" ? "CandidateName_DESC" : "CandidateName_ASC";
                ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";
                List<DocumentStatus_Result> list;
                var userList = new List<LoginDetails>();
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    // Get filted user on text

                    if (string.IsNullOrEmpty(searchString))
                        list = _IUserService.DocumentStatusList("0").ToList(); // Passing 0 for all users
                    else
                    {
                        searchString = searchString.ToLower();
                        list = _IUserService.DocumentStatusList("0").Where(m => m.CandidateName.ToLower().Contains(searchString) || m.DocumentName.ToLower().Contains(searchString) || m.Category.ToLower().Contains(searchString) || m.Document.ToLower().Contains(searchString)).ToList(); // Passing 0 for all users
                    }

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        if (sortOrder.Contains("CandidateName_DESC"))
                            list = list.OrderByDescending(m => m.CandidateName).ToList();
                        if (sortOrder.Contains("CandidateName_ASC"))
                            list = list.OrderBy(m => m.CandidateName).ToList();
                        if (sortOrder.Contains("Status_DESC"))
                            list = list.OrderByDescending(m => m.IsVerify).ToList();
                        if (sortOrder.Contains("Status_ASC"))
                            list = list.OrderBy(m => m.IsVerify).ToList();

                    }
                    else
                    {
                        list = list.OrderBy(m => m.CandidateName).ToList();
                    }

                    //var intMandatoryDocs = _ISubDocumentCategoryService.GetAll(null, null, "").Where(x => x.IsNeeded == true).Count();

                    List<DocDetailsModel> model = new List<DocDetailsModel>();

                    string strWebUrl = ConfigurationManager.AppSettings["DocumentPath"];

                    model = list
                   .Select(x => new DocDetailsModel()
                   {
                       Id = x.DocDetID,
                       UserId = x.UserId,
                       CandidateName = x.CandidateName,
                       DocumentName = x.DocumentName,
                       FilePath = strWebUrl + x.FilePath,
                       Maincategory = x.Category,
                       SubCategory = x.Document,
                       IsVerify = x.IsVerify,
                       Status = x.Status,
                       //MandatoryDocs = intMandatoryDocs
                   })
                   .ToList();

                    if (Request.HttpMethod != "GET")
                    {
                        pageIndex = 1;
                    }
                    //create group by on candidate 
                    var GroupCandidate = (from obj in model
                                          group obj by obj.CandidateName into empg
                                          select new
                                          {
                                              NAME = empg.Key,
                                              DocumentCount = empg.Count(x => x.Id > 0 ? true : false),
                                              ApprovedDocs = empg.Count(x => (x.IsVerify.HasValue ? x.IsVerify.Value == true : false))
                                              ,
                                              RejectedDocs = empg.Count(x => x.IsVerify.HasValue ? x.IsVerify.Value == false : false)
                                          }).ToList();

                    List<DocDetailsModel> data = new List<DocDetailsModel>();

                    foreach (var item in GroupCandidate)
                    {
                        //var listmodel = model.Where(x => x.CandidateName == item.NAME).ToList();
                        if (item.DocumentCount == item.ApprovedDocs)
                        {
                            data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                CandidateName = x.CandidateName,
                                DocumentName = x.DocumentName,
                                FilePath = strWebUrl + x.FilePath,
                                Maincategory = x.Maincategory,
                                SubCategory = x.SubCategory,
                                IsVerify = x.IsVerify,
                                Status = x.Status,
                                //MandatoryDocs = intMandatoryDocs,
                                isAllApproved = true
                            }).ToList());
                        }
                        else if (item.DocumentCount == item.RejectedDocs)
                        {
                            data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                CandidateName = x.CandidateName,
                                DocumentName = x.DocumentName,
                                FilePath = strWebUrl + x.FilePath,
                                Maincategory = x.Maincategory,
                                SubCategory = x.SubCategory,
                                IsVerify = x.IsVerify,
                                Status = x.Status,
                                // MandatoryDocs = intMandatoryDocs,
                                isAllRejected = true
                            }).ToList());
                        }
                        else
                        {
                            data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                CandidateName = x.CandidateName,
                                DocumentName = x.DocumentName,
                                FilePath = strWebUrl + x.FilePath,
                                Maincategory = x.Maincategory,
                                SubCategory = x.SubCategory,
                                IsVerify = x.IsVerify,
                                Status = x.Status,
                                // MandatoryDocs = intMandatoryDocs
                            }).ToList());
                        }
                    }

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
                    int pageNumber = 1;
                    pageNumber = pageIndex != 1 ? pageIndex : 1;
                    ViewBag.PageIndex = pageNumber;
                    ViewBag.SearchString = searchString;

                    return PartialView("~/Views/user/_partialDocList.cshtml", data.ToPagedList(pageNumber, pageSize));
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult GetDocList(string sortOrder, int? pageIndex = 1, string searchString = "", bool redirectCall = false)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.CandidateNameParm = String.IsNullOrEmpty(sortOrder) ? "CandidateName_DESC" : "";
                ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";
                List<DocumentStatus_Result> list;
                var userList = new List<LoginDetails>();
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    if (string.IsNullOrEmpty(searchString))
                        list = _IUserService.DocumentStatusList("0").ToList(); // Passing 0 for all users
                    else
                    {
                        searchString = searchString.ToLower();
                        list = _IUserService.DocumentStatusList("0").Where(m => m.CandidateName.ToLower().Contains(searchString) || m.DocumentName.ToLower().Contains(searchString) || m.Category.ToLower().Contains(searchString) || m.Document.ToLower().Contains(searchString)).ToList(); // Passing 0 for all users
                    }

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        if (sortOrder.Contains("CandidateName_DESC"))
                            list = list.OrderByDescending(m => m.CandidateName).ToList();
                        if (sortOrder.Contains("CandidateName_ASC"))
                            list = list.OrderBy(m => m.CandidateName).ToList();
                        if (sortOrder.Contains("Status_DESC"))
                            list = list.OrderByDescending(m => m.IsVerify).ToList();
                        if (sortOrder.Contains("Status_ASC"))
                            list = list.OrderBy(m => m.IsVerify).ToList();
                    }
                    else
                    {
                        list = list.OrderBy(m => m.CandidateName).ToList();
                    }

                    //var intMandatoryDocs = _ISubDocumentCategoryService.GetAll(null, null, "").Where(x => x.IsNeeded == true).Count();

                    List<DocDetailsModel> model = new List<DocDetailsModel>();

                    string strWebUrl = ConfigurationManager.AppSettings["DocumentPath"];

                    model = list
                   .Select(x => new DocDetailsModel()
                   {
                       Id = x.DocDetID,
                       UserId = x.UserId,
                       CandidateName = x.CandidateName,
                       DocumentName = x.DocumentName,
                       FilePath = strWebUrl + x.FilePath,
                       Maincategory = x.Category,
                       SubCategory = x.Document,
                       IsVerify = x.IsVerify,
                       Status = x.Status,
                       // MandatoryDocs = intMandatoryDocs
                   })
                   .ToList();

                    if (Request.HttpMethod != "GET")
                    {
                        pageIndex = 1;
                    }
                    //create group by on candidate 
                    var GroupCandidate = (from obj in model
                                          group obj by obj.CandidateName into empg
                                          select new
                                          {
                                              NAME = empg.Key,
                                              DocumentCount = empg.Count(x => x.Id > 0 ? true : false),
                                              ApprovedDocs = empg.Count(x => (x.IsVerify.HasValue ? x.IsVerify.Value == true : false))
                                              ,
                                              RejectedDocs = empg.Count(x => x.IsVerify.HasValue ? x.IsVerify.Value == false : false)
                                          }).ToList();

                    List<DocDetailsModel> data = new List<DocDetailsModel>();

                    foreach (var item in GroupCandidate)
                    {
                        if (item.DocumentCount == item.ApprovedDocs)
                        {
                            data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                CandidateName = x.CandidateName,
                                DocumentName = x.DocumentName,
                                FilePath = strWebUrl + x.FilePath,
                                Maincategory = x.Maincategory,
                                SubCategory = x.SubCategory,
                                IsVerify = x.IsVerify,
                                Status = x.Status,
                                //  MandatoryDocs = intMandatoryDocs,
                                isAllApproved = true
                            }).ToList());
                        }
                        else if (item.DocumentCount == item.RejectedDocs)
                        {
                            data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                CandidateName = x.CandidateName,
                                DocumentName = x.DocumentName,
                                FilePath = strWebUrl + x.FilePath,
                                Maincategory = x.Maincategory,
                                SubCategory = x.SubCategory,
                                IsVerify = x.IsVerify,
                                Status = x.Status,
                                // MandatoryDocs = intMandatoryDocs,
                                isAllRejected = true
                            }).ToList());
                        }
                        else
                        {
                            data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                CandidateName = x.CandidateName,
                                DocumentName = x.DocumentName,
                                FilePath = strWebUrl + x.FilePath,
                                Maincategory = x.Maincategory,
                                SubCategory = x.SubCategory,
                                IsVerify = x.IsVerify,
                                Status = x.Status,
                                //   MandatoryDocs = intMandatoryDocs
                            }).ToList());
                        }
                    }

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
                    int pageNumber = (pageIndex ?? 1);
                    ViewBag.SearchString = searchString;
                    ViewBag.PageIndex = pageNumber;
                    return PartialView("~/Views/user/_partialDocList.cshtml", data.ToPagedList(pageNumber, pageSize));
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public FileContentResult getImg(int id)
        {
            byte[] byteArray;
            DocumentDetail Doc = _IDocumentDetailsService.GetById(id);
            byteArray = Doc.Data;
            string strContentType = string.Empty;
            if (Doc.ContentType == ".pdf")
                strContentType = "application/pdf";
            else
                strContentType = "image/jpg";
            return byteArray != null ? new FileContentResult(byteArray, strContentType) : null;
        }

        public ActionResult Download(string id)
        {
            DocumentDetail Doc = _IDocumentDetailsService.GetById(Convert.ToInt32(id));

            byte[] fileByte = Doc.Data;

            string strContentType = string.Empty;
            string strFileName = string.Empty;
            if (Doc.ContentType == ".pdf")
            {
                strContentType = "application/pdf";
                strFileName = "attachment;filename=" + Doc.DocumentName;
            }
            else
            {
                strContentType = "image/jpg"; //+  Doc.ContentType.Remove('.');
                strFileName = "attachment;filename=" + Doc.DocumentName;
            }
            Response.Clear();
            MemoryStream ms = new MemoryStream(fileByte);
            Response.ContentType = strContentType;
            Response.AddHeader("content-disposition", strFileName);
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();

            string path = "";
            path = Path.Combine(Server.MapPath("~/UploadedDocuments/"), "gajutest.jpg");

            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            ms.WriteTo(file);
            file.Close();
            ms.Close();

            return new FileStreamResult(Response.OutputStream, strContentType);
        }

        public ActionResult ZipDownload(string userId)
        {
            var userDetails = _IUserService.GetById(Convert.ToInt32(userId));
            string filename = userDetails.FirstName + "_" + userDetails.LastName;
            string folderPath = Path.Combine(Server.MapPath("~/UploadedDocuments/"), filename);
            string zipPath = folderPath + ".zip"; // @"c:\result.zip";
            ConvertByteStreemToZip(Convert.ToInt32(userId));
            return File(zipPath, "application/zip", filename + ".zip");
        }

        private void ConvertByteStreemToZip(int userId)
        {
            var docDetails_lst = _IDocumentDetailsService.GetAll(null, null, "").Where(m => m.UserID == userId && m.IsActive == true);
            var userDetails = _IUserService.GetById(userId);
            string filename = userDetails.FirstName + "_" + userDetails.LastName;
            string folderPath = Path.Combine(Server.MapPath("~/UploadedDocuments/"), filename);
            string zipPath = folderPath + ".zip";
            DeleteAllZipFilesAndFolders();
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
            int docCount = 1;
            foreach (DocumentDetail doc in docDetails_lst)
            {
                byte[] fileByte = doc.Data;
                string strContentType = string.Empty;
                string strFileName = string.Empty;
                strFileName = "Doc" + docCount.ToString() + "_" + doc.DocumentName;
                Response.Clear();
                MemoryStream ms = new MemoryStream(fileByte);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string path = "";
                path = Path.Combine(folderPath, strFileName);
                FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();
                docCount++;
            }
            ZipFile.CreateFromDirectory(folderPath, zipPath);
        }

        private void ConvertByteStreamFileToZip(int userId,int docId)
        {
           // var docDetails_lst = _IDocumentDetailsService.GetAll(null, null, "").Where(m => m.UserID == userId && m.IsActive == true);
            var document = _IDocumentDetailsService.GetById(docId);
            var userDetails = _IUserService.GetById(userId);
            //string filename = userDetails.FirstName + "_" + userDetails.LastName;
            string folderPath = Path.Combine(Server.MapPath("~/UploadedDocuments/"), document.DocumentName);
            //string zipPath = folderPath + ".zip";
            //DeleteAllZipFilesAndFolders();
            //if (Directory.Exists(folderPath))
            //    Directory.Delete(folderPath, true);
                byte[] fileByte = document.Data;
                string strContentType = string.Empty;
                string strFileName = string.Empty;
                strFileName = "Doc" +  "_" + document.DocumentName;
                Response.Clear();
                MemoryStream ms = new MemoryStream(fileByte);

                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}
                //string path = "";
                //path = Path.Combine(folderPath);
                FileStream file = new FileStream(folderPath, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();                          
           // ZipFile.CreateFromDirectory(folderPath, zipPath);
        }


        // this methode is used to delete existing all zip and folders
        public void DeleteAllZipFilesAndFolders()
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/UploadedDocuments/"));
            FileInfo[] files = di.GetFiles("*.zip")
                                 .Where(p => p.Extension == ".zip").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    file.Delete();
                }
                catch
                {

                }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

        }
        [HttpPost]
        public ActionResult SendEmail(string to, string sub, string body, string isAttach, string userId)
        {
            LoginDetail userDetails = _IUserService.GetById(Convert.ToInt32(userId));
            ConvertByteStreemToZip(Convert.ToInt32(userId));
            string filename = userDetails.FirstName + "_" + userDetails.LastName;
            string folderPath = Path.Combine(Server.MapPath("~/UploadedDocuments/"), filename);

            SendMailToUser(userDetails, body, sub, to, Convert.ToBoolean(isAttach), folderPath + ".zip");

            return Json("sucess");
        }
        [HttpPost]
        public ActionResult SendEmailForRejectDocumment(string to, string sub, string body, string isAttach, string userId,string documentId)
        {
            DocumentDetail Doc = _IDocumentDetailsService.GetById(Convert.ToInt32(documentId));
            // string folderPath = Doc.DocumentName;

            LoginDetail userDetails = _IUserService.GetById(Convert.ToInt32(userId));
            ConvertByteStreamFileToZip(Convert.ToInt32(userId), Convert.ToInt32(documentId));
           
            //string filename = userDetails.FirstName + "_" + userDetails.LastName;
            string filePath = Path.Combine(Server.MapPath("~/UploadedDocuments/"), Doc.DocumentName);

            SendMailToUser(userDetails, body, sub, to, Convert.ToBoolean(isAttach), filePath);
            userDetails.IsSubmitted = false;
            _IUserService.Update(userDetails,null, "");

            return Json("sucess");
        }

        [HttpPost]
        public ActionResult SendEmailAsReminder(string to, string cc, string sub, string body, string isAttach, string userId)
        {
            LoginDetail userDetails = _IUserService.GetById(Convert.ToInt32(userId));
            ConvertByteStreemToZip(Convert.ToInt32(userId));
            string filename = userDetails.FirstName + "_" + userDetails.LastName;
            string folderPath = Path.Combine(Server.MapPath("~/UploadedDocuments/"), filename);

            SendMailToUserAsReminder(userDetails, body, sub, to, cc, Convert.ToBoolean(isAttach), folderPath + ".zip");

            return Json("sucess");
        }


        [HttpPost]
        public ActionResult VerifyDoc(string Id, Boolean Val)
        {
            DocumentDetail Doc = _IDocumentDetailsService.GetById(Convert.ToInt32(Id));

            Doc.IsVerify = Val;

            _IDocumentDetailsService.Update(Doc, null, null);

            return Json("sucess");
        }

        /*User Add Edit*/
        [HttpGet]
        [Authorize]
        public ActionResult AddEditUserDetails(int Id = 0)
        {
            var silicusLocations = ConfigurationManager.AppSettings["SilicusLocation"].Split(',').ToList().Select(a => new SelectListItem() { Text = a, Value = a });
            ViewBag.Departments = GetDepartments();
            ViewBag.Designations = GetDesignationList();
            ViewBag.CountryCodeList = GetCountryCode();
            ViewBag.silicusLocations = new SelectList(silicusLocations, "Value", "Text");
            //ViewBag.EducationSubCategories = GetEducationCategories();

            AddEditUserModel model = new AddEditUserModel();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            }

            if (Id != 0)
            {
                var userDetails = _IUserService.GetAll(null, null, "");

                var obj = userDetails.Where(u => u.UserID == Id).FirstOrDefault();
                if (obj != null)
                {
                    //Edit user details
                    model.FirstName = obj.FirstName;
                    model.UserID = obj.UserID;
                    model.LastName = obj.LastName;
                    model.Email = obj.Email;
                    model.JoiningDate = obj.JoiningDate;
                    model.DOB = (obj.DOB != null) ? Convert.ToString(SessionManager.DecryptData(obj.DOB)) : string.Empty;//Convert.ToString(SessionManager.DecryptData(obj.DOB));//Change request - Null check, as DOB set null while adding user
                    model.ContactNumber = obj.ContactNumber;
                    model.DepartmentID = obj.DepartmentID;
                    model.DesignationID = obj.DesignationID;
                    model.JoiningLocation = obj.JoiningLocation;
                    model.OnboardingSPOCName = obj.OnboardingSPOCName;
                    model.PrimarySkill = obj.PrimarySkill;
                    model.ProjectName = obj.ProjectName;
                    model.RecruiterName = obj.RecruiterName;
                    model.RequisitionID = obj.RequisitionID;
                  //  model.DesignationOther= obj.
                    //code change 
                    // model.SubDocCatID = obj.SubDocCatID;
                    model.CountryCode = obj.CountryCode;
                    model.Gender = obj.Gender;
                    model.IsFresher = obj.IsFresher??false;
                    //model.NoOfEmployments = obj.NoOfEmployments == null ? 0 : (int)obj.NoOfEmployments;
                    //add for education categories

                    //set selected values from AdminEducationCategoryForUser table
                    var selectedEducationCategories = new List<EducationCategory>();

                    //setup a view model
                    model.AvailableEducationCategories = GetEducationCategories();
                    model.SelectedEducationCategories = GetSelectedCategories(Id);
                }
            }
            else
            {
                //Add new user

                model.JoiningDate = Convert.ToDateTime(DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture));
                var selectedEducationCategories = new List<EducationCategory>();
                //setup a view model
                model.AvailableEducationCategories = GetEducationCategories();
                model.SelectedEducationCategories = GetSelectedCategories(Id);
            }
            return PartialView("_AddEditUser", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddEditUserDetails(AddEditUserModel model)
        {
            bool status = false;

            if (ModelState.IsValid)
            {

                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                    userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                }

                LoginDetail userDetails = new LoginDetail();
                if (model.UserID != 0)
                {
                    var userlist = _IUserService.GetAll(null, null, "");


                    var obj = userlist.Where(u => u.UserID == model.UserID).FirstOrDefault();

                    if (obj != null)
                    {
                        //update

                        obj.FirstName = model.FirstName;
                        obj.LastName = model.LastName;
                        obj.DOB = Convert.ToString(SessionManager.EncryptData(model.DOB));
                        obj.JoiningDate = Convert.ToDateTime(model.JoiningDate);
                        obj.Email = model.Email.ToLower();

                        //code change - edmx fix
                        // obj.SubDocCatID = model.SubDocCatID;
                        obj.DepartmentID = model.DepartmentID;
                        obj.DesignationID = model.DesignationID;
                        obj.Email = model.Email.ToLower();
                        obj.JoiningLocation = model.JoiningLocation;
                        obj.PrimarySkill = model.PrimarySkill;
                        obj.ProjectName = model.ProjectName;
                        obj.RecruiterName = model.RecruiterName;
                        obj.RequisitionID = model.RequisitionID;
                        obj.ContactNumber = model.ContactNumber;
                        //newly added
                        obj.CountryCode = model.CountryCode;
                        obj.Gender = model.Gender;
                        obj.IsFresher = model.IsFresher;
                        //obj.NoOfEmployments = model.NoOfEmployments;

                        try
                        {

                           if (model.OtherDesignation != null)
                            {
                             var designation=   UpdateDesignationList(model.OtherDesignation);
                                if(designation != null)
                                {
                                    obj.DesignationID = designation.DesignationID;
                                }
                                
                            }


                            status = _IUserService.Update(obj, null, "");

                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                        //Update existing education caetgrories for that user
                        _IUserService.UpdateEducationCategoryDetails(obj.UserID, userName);

                        var educationCategoryDetails = new AdminEducationCategoryForUser();
                        //Add new education categpries for that user 
                        //null check 
                        if (model.PostedEducationCategories.EducationCategoryIds != null)
                        {
                            foreach (string item in model.PostedEducationCategories.EducationCategoryIds)
                            {
                                educationCategoryDetails.UserID = obj.UserID;
                                educationCategoryDetails.EducationCategoryId = Convert.ToInt32(item);
                                educationCategoryDetails.IsActive = true;
                                educationCategoryDetails.CreatedBy = userName;
                                educationCategoryDetails.CreatedDate = DateTime.UtcNow;

                                var stat = _IUserService.AddEducationCategoryDetails(educationCategoryDetails, userName);
                            }
                        }


                    }

                }
                else
                {
                    //insert
                    userDetails.CreatedDate = DateTime.UtcNow;
                    userDetails.FirstName = model.FirstName;
                    userDetails.LastName = model.LastName;
                    userDetails.DOB = Convert.ToString(SessionManager.EncryptData(model.DOB)); //SessionManager.EncryptData(Convert.ToString(model.DOB.Value));

                    string strDate = model.DOB; //Format – dd/MM/yyyy
                    //split string date by separator, here I’m using ‘/’
                    string[] arrDate = strDate.Split('/');
                    //now use array to get specific date object
                    string day = arrDate[0].ToString();
                    string month = arrDate[1].ToString();
                    string year = arrDate[2].ToString();
                    string pwd = day + month + year;

                    userDetails.Password = SessionManager.EncryptData(pwd);
                    userDetails.JoiningDate = Convert.ToDateTime(model.JoiningDate);
                    userDetails.Email = model.Email.ToLower(); //SessionManager.EncryptData(;               
                    //userDetails.RoleID = 0;
                    userDetails.RoleID = 16; // Code change as per the New Candidate Role in Master_Role table
                    userDetails.IsActive = 0;
                    userDetails.IsDelete = false;

                    //code change - edmx fix
                    // userDetails.SubDocCatID = model.SubDocCatID;
                    userDetails.DepartmentID = model.DepartmentID;
                    userDetails.DesignationID = model.DesignationID;
                    userDetails.Email = model.Email.ToLower();
                    userDetails.JoiningLocation = model.JoiningLocation;
                    userDetails.PrimarySkill = model.PrimarySkill;
                    userDetails.ProjectName = model.ProjectName;
                    userDetails.RecruiterName = model.RecruiterName;
                    userDetails.RequisitionID = model.RequisitionID;
                    userDetails.ContactNumber = model.ContactNumber;
                    //newly added
                    userDetails.CountryCode = model.CountryCode;
                    userDetails.Gender = model.Gender;
                    //userDetails.NoOfEmployments = model.NoOfEmployments;
                    status = _IUserService.AddUserDetails(userDetails);

                    //Update existing education caetgrories for that user
                    _IUserService.UpdateEducationCategoryDetails(userDetails.UserID, userName);

                    var educationCategoryDetails = new AdminEducationCategoryForUser();
                    //Add new education categpries for that user 
                    if (model.PostedEducationCategories.EducationCategoryIds != null)
                    {
                        foreach (string item in model.PostedEducationCategories.EducationCategoryIds)
                        {
                            educationCategoryDetails.UserID = userDetails.UserID;
                            educationCategoryDetails.EducationCategoryId = Convert.ToInt32(item);
                            educationCategoryDetails.IsActive = true;
                            educationCategoryDetails.CreatedBy = userName;
                            educationCategoryDetails.CreatedDate = DateTime.UtcNow;

                            var stat = _IUserService.AddEducationCategoryDetails(educationCategoryDetails, userName);
                        }
                    }
                }
            }
            if (status)
            {
                return Json(new { result = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, Message = "Fail" }, JsonRequestBehavior.AllowGet);
            }
        }


        // [HttpPost]
        public async Task<ActionResult> SyncCandidates(int? page, string sortOrder, string searchString = "")
        {

            var userList = new List<AddEditUserModel>();

            StringBuilder syncLog = new StringBuilder();
            string JobViteBaseURL = ConfigurationManager.AppSettings["JobViteBaseURL"];
            string JobViteUserId = ConfigurationManager.AppSettings["JobViteUserId"];
            string JobVitesc = ConfigurationManager.AppSettings["JobVitesc"];
            string JobViteCandidateSelecttionStatus = ConfigurationManager.AppSettings["JobViteCandidateSelecttionStatus"];

            string baseAddress = JobViteBaseURL + "=" + JobViteUserId + "&sc=" + JobVitesc + "&wflowstate=" + JobViteCandidateSelecttionStatus + "&format=json";


            try
            {
                using (var httpClient = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    HttpResponseMessage response = await httpClient.GetAsync(baseAddress);
                    response.EnsureSuccessStatusCode();
                    syncLog.AppendLine("JobVite Connection Established");
                    string result = await response.Content.ReadAsStringAsync();
                    APIResponse businessunits = JsonConvert.DeserializeObject<APIResponse>(result);
                    var candidateList = (from candidate in businessunits.candidates

                                         select new AddEditUserModel()
                                         {
                                             FirstName = candidate.firstName,
                                             LastName = candidate.lastName,
                                             Email = candidate.email,
                                             //Change request - Showing only above required fields in Add jobivite user list grid
                                             //JoiningLocation = candidate.location == ",  " ? "" : candidate.location,
                                             //ContactNumber = candidate.mobile,
                                             //RequisitionID = candidate.application.job.requisitionId,
                                             //DepartmentName = candidate.application.job.department,
                                             //CountryCode= candidate.countryCode

                                         }).ToList();

                    if (Request.HttpMethod != "GET")
                    {
                        page = 1;
                    }
                    //int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); 
                    int pageSize = 10000;
                    int pageNumber = (page ?? 1);
                    ViewBag.PageIndex = pageNumber;
                    // ViewBag.SearchString = searchString;

                    return PartialView("_popupJobViteCandidateList", candidateList.ToPagedList(pageNumber, pageSize));
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetEmployeesForLoginAs(int? page, string sortOrder, string searchString = "")
        {

           var userList = new List<AddEditUserModel>();
            try
            {
                    var activeUsers = _IUserService.GetActiveUsers();
                    var candidateList = (from candidate in activeUsers

                                         select new AddEditUserModel()
                                         {
                                             FirstName = candidate.FirstName,
                                             LastName = candidate.LastName,
                                             Email = candidate.Email,

                                         }).ToList();

                    if (Request.HttpMethod != "GET")
                    {
                        page = 1;
                    }
                    int pageSize = 10000;
                    int pageNumber = (page ?? 1);
                    ViewBag.PageIndex = pageNumber;
                    return View("_employeesForLoginAs", candidateList.ToPagedList(pageNumber, pageSize));
                
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult GetLoginAsPopup()
        //{

        //    return View("UserListForLoginAs");
        //}
        [HttpPost]
        public ActionResult SaveJobeViteDetails(List<AddEditUserModel> jobViteCandidateList)
        {
            List<LoginDetail> list = new List<LoginDetail>();
            bool result = false;
            bool status = false;
            string firstname = "";
            string lastname = "";



            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            }
            if (jobViteCandidateList != null)
            {
                foreach (AddEditUserModel model in jobViteCandidateList)
                {
                    LoginDetail details = new LoginDetail();

                    firstname = model.FirstName;
                    lastname = model.LastName;
                    details.FirstName = Regex.Replace(firstname, @"\s+", "");
                    lastname = model.LastName;
                    details.LastName = Regex.Replace(lastname, @"\s+", "");
                    //concatination of string to get password
                    string s1 = details.FirstName;
                    string s = s1.Substring(0, 1);
                    string pwd = s + "_" + details.LastName + "_" + "123";
                    details.Password = SessionManager.EncryptData(pwd);

                    //string date = "10/3/2017"; /*Change Request - Removing unwanted details while adding user*/
                    //details.DOB =SessionManager.EncryptData(Convert.ToString(date)); //SessionManager.EncryptData(Convert.ToString(model.DOB.Value));

                    details.Email = (model.Email != null) ? model.Email : string.Empty;
                    details.JoiningDate = DateTime.UtcNow;

                    //if (model.ContactNumber != null)
                    //{
                    //    if (model.ContactNumber.Contains("+"))
                    //    {
                    //        var newString = string.Empty;
                    //        string str = model.ContactNumber.Substring(0, 3);
                    //        newString = model.ContactNumber.Remove(0, model.ContactNumber.IndexOf(' ') + 1);
                    //        //  newString= model.ContactNumber.Split(new char[] { ' ' }, 2)[1];
                    //        if (model.ContactNumber.Contains("-"))
                    //        {
                    //            newString = model.ContactNumber.Split(new char[] { '-' }, 2)[1];
                    //            //   newString = model.ContactNumber.Remove(0, str.IndexOf(" - ") + 1);
                    //        }
                    //        details.ContactNumber = newString;
                    //        details.CountryCode = str;
                    //    }

                    //    else
                    //    {
                    //        details.ContactNumber = model.ContactNumber;
                    //    }
                    //}

                    details.DepartmentID = 1; /*Change Request - Adding default Department as first value in the list */
                    //if (model.DepartmentName == "")
                    //{
                    //    details.DepartmentID = 14;
                    //}
                    //else {
                    //    Master_Department dpt  = _IUserService.GetDepartmentId(model.DepartmentName);
                    //    if (dpt != null)
                    //    {
                    //        details.DepartmentID = dpt.DepartmentID;
                    //    }
                    //    else {
                    //        details.DepartmentID = 14;
                    //    }

                    //}


                    //details.RequisitionID = model.RequisitionID; /*Change Request - Removing unwanted  details while adding user*/
                    //if (model.JoiningLocation != null)
                    //{
                    //    details.JoiningLocation = model.JoiningLocation;
                    //}
                    //else {
                    //    details.JoiningLocation = "Pune";
                    //}

                    details.JoiningLocation = "Pune";   /*Change Request - Adding default Designation as first value in the list */


                    details.DesignationID = 1; /*Change Request - Adding default Designation as first value in the list */
                    details.RoleID = 16;

                    //details.ActivatedDate = DateTime.UtcNow; /*Change Request - Removing unwanted  details while adding user*/

                    details.CreatedBy = userName; /*Change Request - Added created by as username for logged in user */
                    details.CreatedDate = DateTime.UtcNow;
                    details.IsSubmitted = false;
                    details.IsActive = 0;
                    details.IsDelete = false;
                    details.LastLogin = DateTime.UtcNow;

                    status = _IUserService.AddUserDetails(details);

                    //Update existing education caetgrories for that user
                    _IUserService.UpdateEducationCategoryDetails(details.UserID, userName);
                    //Update existing education caetgrories for that user

                    var educationCategoryDetails = new AdminEducationCategoryForUser();                    //Add new education categpries for that user 
                    educationCategoryDetails.UserID = details.UserID;
                    educationCategoryDetails.EducationCategoryId = 3;
                    educationCategoryDetails.IsActive = true;
                    educationCategoryDetails.CreatedBy = userName;
                    educationCategoryDetails.CreatedDate = DateTime.UtcNow;
                    var stat = _IUserService.AddEducationCategoryDetails(educationCategoryDetails, userName);

                }

            }
            if (status)
            {
                return Json(new { result = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, Message = "Fail" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult CheckDuplicateDesignation( string designation)
        {
            var status = false;
            var desginationList = _IDesignationService.GetAll(null, null, "");/*.Any(d=>d.Designation == designation).FirstOrDefault();*/

            foreach(var des in desginationList)
            {
                if(des.Designation == designation)
                {
                    status = true;
                }
            }
           
           
            if (status)
            {
                return Json(new { result = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, Message = "Fail" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult IsFirstNameLastNameEmailDuplicateCheck(List<AddEditUserModel> jobViteCandidateList)
        {
            bool ifRecordExist = false;
            try
            {

                var employeedata = string.Empty;
                var firstname = string.Empty;
                var lastname = string.Empty;
                var email = string.Empty;
                foreach (AddEditUserModel model in jobViteCandidateList)
                {

                    lastname = model.LastName;
                    firstname = Regex.Replace(model.FirstName, @"\s+", "");
                    lastname = model.LastName;
                    lastname = Regex.Replace(model.LastName, @"\s+", "");
                    email = model.Email;

                    LoginDetail login = _IUserService.GetUserExists(firstname, lastname, email);
                    ifRecordExist = login != null ? true : false;
                }

                var roleData = _IRoleService.GetAll(null, null, "").Where(p => p.UserName.ToLower() == email.ToLower());
                if (roleData != null && roleData.Count() > 0)
                {
                    ifRecordExist = true;
                }

                return Json(ifRecordExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteUserDetails(int UserID)
        {

            bool data = _IUserService.DeleteUser(UserID);
            return Json(new { result = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult AddOfferCandidateToEmployee(int? page)
        {
            AddEmployeeModel model = new AddEmployeeModel();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var user = _IUserService.GetAll(null, null, "").Where(m => m.UserID != SessionManager.UserId && m.IsDelete == false).ToList();

                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]); //5;
                int pageNumber = (page ?? 1);

                var OfferCandidatesList = new List<OfferCandidateModel>();

                var EmployeeList = new List<AddEmployeeModelList>();

                model.OfferCandidateList = OfferCandidatePagedList(1);
                model.EmployeeDetailsList = EmployeePagedList(1, pageSize);
                return View("AddOffertoEmployeeIndex", model);

            }


            return View("AddOffertoEmployeeIndex", model);
        }

        public IPagedList<OfferCandidateModel> OfferCandidatePagedList(int? pageNo)
        {
            var list = _IUserService.GetAll(null, null, "").OrderByDescending(m => m.UserID).ToList();

            var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

            LoginDetail empobj;
            foreach (var empitem in AdminEmployeeList)
            {
                empobj = new LoginDetail();
                empobj = list.Where(m => m.Email.Trim().ToUpper() == empitem.UserName.Trim().ToUpper()).FirstOrDefault();
                if (empobj != null)
                {
                    list.Remove(empobj);
                }
            }
            var ExistingEmployeesList = _IEmployeeService.GetAll(null, null, "").ToList();
            foreach (var empitem in ExistingEmployeesList)
            {
                empobj = new LoginDetail();
                empobj = list.Where(m => m.UserID == empitem.UserId).FirstOrDefault();
                if (empobj != null)
                {
                    list.Remove(empobj);
                }
            }


            list = list.Where(m => m.RoleID != 1 && m.IsActive == 1 && m.IsDelete == false).ToList(); //to ignore admin role candidates
            List<OfferCandidateModel> ModelList = new List<OfferCandidateModel>();
            ModelList = list
            .Select(x => new OfferCandidateModel()
            {
                EmployeeName = x.FirstName + " " + x.LastName,
                ShortDOB = (x.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(x.DOB))) : string.Empty,//convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(x.DOB))),
                EmailAddress = x.Email,
                UserID = x.UserID
            })
            .ToList();

            if (Request.HttpMethod != "GET")
            {
                pageNo = 1;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]); //5;
            int pageNumber = (pageNo ?? 1);

            return ModelList.ToPagedList(pageNumber, pageSize);
        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult OfferCandidateList(int? pageNo)
        {

            var list = _IUserService.GetAll(null, null, "").ToList();

            var AdminEmployeeList = _IRoleService.GetAll(null, null, "").ToList();

            LoginDetail empobj;
            foreach (var empitem in AdminEmployeeList)
            {
                empobj = new LoginDetail();
                empobj = list.Where(m => m.Email.Trim().ToUpper() == empitem.UserName.Trim().ToUpper()).FirstOrDefault();
                if (empobj != null)
                {
                    list.Remove(empobj);
                }
            }
            var ExistingEmployeesList = _IEmployeeService.GetAll(null, null, "").ToList();
            foreach (var empitem in ExistingEmployeesList)
            {
                empobj = new LoginDetail();
                empobj = list.Where(m => m.UserID == empitem.UserId).FirstOrDefault();
                if (empobj != null)
                {
                    list.Remove(empobj);
                }
            }


            list = list.Where(m => m.RoleID != 1 && m.IsActive == 1 && m.IsDelete == false).ToList(); //to ignore admin role candidates
            List<OfferCandidateModel> ModelList = new List<OfferCandidateModel>();
            ModelList = list
            .Select(x => new OfferCandidateModel()
            {
                EmployeeName = x.FirstName + " " + x.LastName,
                ShortDOB = (x.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(x.DOB))) : string.Empty, //convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(x.DOB))),
                EmailAddress = x.Email,
                UserID = x.UserID
            })
            .ToList();

            if (Request.HttpMethod != "GET")
            {
                pageNo = 1;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]); //5;
            int pageNumber = (pageNo ?? 1);
            return PartialView("_OfferCandidateList", ModelList.ToPagedList(pageNumber, pageSize));


        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult EmployeeList(int? pageNo, string searchString = "")
        {

            var EmployeeList = _IEmployeeService.GetAll(null, null, "").ToList();

            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
                EmployeeList = EmployeeList.Where(m => m.Active == 1).ToList().Where(m => m.EmployeeName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();

            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();
            ModelList = EmployeeList
            .Select(x => new AddEmployeeModelList()
            {
                DOB = (x.DOB != null) ? SessionManager.DecryptData(x.DOB) : string.Empty,//SessionManager.DecryptData(x.DOB),
                Email = SessionManager.DecryptData(x.Email),
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                JoiningDate = x.JoiningDate,
                ReasonforLeaving = x.ReasonForleaving,
                ID = x.Id,
                isActive = x.Active == 1 ? true : false
            }).ToList();

            if (Request.HttpMethod != "GET")
            {
                pageNo = 1;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]); //5;
            int pageNumber = (pageNo ?? 1);
            return PartialView("_EmployeeList", ModelList.ToPagedList(pageNumber, pageSize));
        }


        public IPagedList<AddEmployeeModelList> EmployeePagedList(int? pageNo, int? pageSize)
        {

            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();


            var EmployeeList = _IEmployeeService.GetAll(null, null, "").ToList();

            EmployeeList = EmployeeList.Where(m => m.Active == 1).ToList();

            ModelList = EmployeeList
            .Select(x => new AddEmployeeModelList()
            {
                DOB = (x.DOB != null) ? SessionManager.DecryptData(x.DOB) : string.Empty,//SessionManager.DecryptData(x.DOB),
                Email = SessionManager.DecryptData(x.Email),
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                JoiningDate = x.JoiningDate,
                ReasonforLeaving = x.ReasonForleaving,
                ID = x.Id,
                isActive = x.Active == 1 ? true : false
            })
            .ToList();
            pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]);
            return ModelList.ToPagedList(pageNo.Value, pageSize.Value);
        }


        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult PastEmployee(int? page)
        {
            AddEmployeeModel model = new AddEmployeeModel();

            if (model.FromDate == DateTime.MinValue)
            {
                model.FromDate = null;
            }

            if (model.ToDate == DateTime.MinValue)
            {
                model.ToDate = null;
            }

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var user = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId != SessionManager.UserId && m.Active == 0).ToList();

                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
                int pageNumber = (page ?? 1);

                var EmployeeList = new List<AddEmployeeModelList>();

                model.EmployeeDetailsList = PastEmployeePagedList(1, 10);


                return View("PastEmployeePagedList", model);

            }


            return View("PastEmployeePagedList", model);
        }

        public IPagedList<AddEmployeeModelList> PastEmployeePagedList(int? pageNo, int? pageSize)
        {
            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();

            var EmployeeList = _IEmployeeService.GetAll(null, null, "").ToList();

            ModelList = EmployeeList
            .Select(x => new AddEmployeeModelList()
            {
                DOB = (x.DOB != null) ? SessionManager.DecryptData(x.DOB) : string.Empty,//SessionManager.DecryptData(x.DOB),
                Email = SessionManager.DecryptData(x.Email),
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                JoiningDate = x.JoiningDate,
                ReasonforLeaving = x.ReasonForleaving,
                ID = x.Id,
                isActive = x.Active == 1 ? true : false,
                LeavingDate = convertDateToShort(x.LeavingDate)
            }).Where((x => x.isActive == false))
            .ToList();

            return ModelList.ToPagedList(pageNo.Value, pageSize.Value);

        }


        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult PastEmployeeList(int? pageNo, string searchString = "", string FromDate = "", string ToDate = "")
        {
            var EmployeeList = _IEmployeeService.GetAll(null, null, "").ToList();

            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
                EmployeeList = EmployeeList.Where(m => m.Active == 0).ToList().Where(m => m.EmployeeName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();


            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();
            ModelList = EmployeeList
            .Select(x => new AddEmployeeModelList()
            {
                DOB = (x.DOB != null) ? SessionManager.DecryptData(x.DOB) : string.Empty,//SessionManager.DecryptData(x.DOB),
                Email = SessionManager.DecryptData(x.Email),
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                JoiningDate = x.JoiningDate,
                ReasonforLeaving = x.ReasonForleaving,
                ID = x.Id,
                isActive = x.Active == 1 ? true : false,
                LeavingDate = convertDateToShort(x.LeavingDate)
            }).Where((x => x.isActive == false))
            .ToList();

            if (!string.IsNullOrEmpty(FromDate))
            {
                ModelList = ModelList.Where(x => Convert.ToDateTime(x.LeavingDate) >= Convert.ToDateTime(FromDate)).ToList();
            }

            if (!string.IsNullOrEmpty(ToDate))
            {
                ModelList = ModelList.Where(x => Convert.ToDateTime(x.LeavingDate) <= Convert.ToDateTime(ToDate)).ToList();
            }

            if (Request.HttpMethod != "GET")
            {
                pageNo = 1;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
            int pageNumber = (pageNo ?? 1);
            return PartialView("_PastEmployeeList", ModelList.ToPagedList(pageNumber, pageSize));
        }
        /// <summary>
        /// Assign Pno is executed in two scenarios
        /// 1. While assiging employee No to the employee
        /// 2.While editing the employee from employee details page.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="EmpNo">EmpNo used only in employee edit mode</param>
        /// <param name="isEmployeeEditMode">Specifies if employee is edited by default it is false, used only in employee edit mode</param>
        /// <returns></returns>
        [HttpGet]
        [ValidateRole]
        public ActionResult AssignPNo(int userID, string EmpNo = null, bool isEmployeeEditMode = false)
        {

            AddEmployeeModel Model = new AddEmployeeModel();

            if (!isEmployeeEditMode)
            {
                //Executed when employee is onboarded from employee details page
                var list = _IUserService.GetAll(null, null, "").ToList();
                LoginDetail data = list.Where(x => x.UserID == userID).FirstOrDefault();
                if (list != null)
                {
                    Model.DOB = (data.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(data.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;//Convert.ToDateTime(SessionManager.DecryptData(data.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    Model.EmpName = data.FirstName + " " + data.LastName;
                    Model.JoiningDate = convertDateToShort(data.JoiningDate);
                    Model.UserID = userID;
                    Model.IsEmployeeEditMode = isEmployeeEditMode;
                    return View("_AddOfferCandidateToEmployee", Model);

                }
            }
            else
            {
                //Executed when employee is edited from employee details page
                var employeeList = _IEmployeeService.GetAll(null, null, "").ToList();
                EmployeeMaster employeeData = employeeList.Where(x => x.Id == Convert.ToInt32(userID)).FirstOrDefault();
                Model.DOB = (employeeData.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(employeeData.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;//Convert.ToDateTime(SessionManager.DecryptData(employeeData.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                Model.EmpName = employeeData.EmployeeName;

                if (employeeData.JoiningDate != new DateTime())
                {
                    Model.JoiningDate = convertDateToShort(employeeData.JoiningDate);
                }
                Model.UserID = userID;
                Model.EmployeeMasterId = Convert.ToInt32(userID);
                Model.EmpNo = employeeData.EmpNo;
                Model.Email = SessionManager.DecryptData(employeeData.Email);
                Model.IsEmployeeEditMode = isEmployeeEditMode;

            }



            return View("_AddOfferCandidateToEmployee", Model);

        }

        [HttpPost]
        [ValidateRole]
        public ActionResult AssignPNo(AddEmployeeModel model)
        {

            if (ModelState.ContainsKey("ReasonforLeaving"))
                ModelState["ReasonforLeaving"].Errors.Clear();

            if (ModelState.ContainsKey("LeavingDate"))
                ModelState["LeavingDate"].Errors.Clear();

            if (ModelState.IsValid)
            {
                if (!model.IsEmployeeEditMode)//checked a flag if employee is edited or onboarded
                {
                    //Executed when employee is onboarded from employee details page
                    EmployeeMaster obj = new EmployeeMaster();
                    obj.EmployeeName = model.EmpName;
                    obj.EmpNo = model.EmpNo;
                    obj.Email = SessionManager.EncryptData(model.Email);
                    obj.DOB = SessionManager.EncryptData(String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(model.DOB)));
                    obj.JoiningDate = Convert.ToDateTime(model.JoiningDate);
                    obj.Active = 1;
                    obj.UserId = model.UserID;
                    bool status = _IUserService.AddOfferCandidateToEmployee(obj);
                    return Json(new { result = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Executed when employee is edited from employee details page
                    EmployeeMaster obj = new EmployeeMaster();
                    obj.EmployeeName = model.EmpName;
                    obj.Id = model.EmployeeMasterId;
                    obj.EmpNo = model.EmpNo;
                    obj.Email = SessionManager.EncryptData(model.Email);
                    obj.DOB = SessionManager.EncryptData(String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(model.DOB)));
                    if (model.JoiningDate != null)
                    {
                        obj.JoiningDate = Convert.ToDateTime(model.JoiningDate);
                    }
                    obj.UserId = model.UserID;
                    bool status = _IUserService.UpdateEmployeeDetails(obj);
                    return Json(new { result = status }, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);


        }


        [HttpGet]
        [ValidateRole]
        public ActionResult ExitEmployee(int ID)
        {

            AddEmployeeModel Model = new AddEmployeeModel();
            var list = _IEmployeeService.GetAll(null, null, "").ToList();
            EmployeeMaster data = list.Where(x => x.Id == ID).FirstOrDefault();
            if (data != null)
            {
                Model.DOB = (data.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(data.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;//Convert.ToDateTime(SessionManager.DecryptData(data.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                Model.EmpName = data.EmployeeName;
                Model.JoiningDate = data.JoiningDate.HasValue ? Convert.ToString(data.JoiningDate.Value.Date.ToShortDateString()) : null;
                Model.EmpNo = data.EmpNo;
                Model.Email = SessionManager.DecryptData(data.Email);
                Model.ID = data.Id;
                return View("_LeaveEmployee", Model);

            }

            return View("_LeaveEmployee", Model);


        }

        [HttpPost]
        [ValidateRole]
        public ActionResult ExitEmployee(AddEmployeeModel model)
        {
            AddEmployeeModel Model = new AddEmployeeModel();
            EmployeeMaster empmaster = new EmployeeMaster();

            empmaster.Id = model.ID;
            empmaster.ReasonForleaving = model.ReasonforLeaving;
            empmaster.LeavingDate = Convert.ToDateTime(model.LeavingDate);
            if (model.JoiningDate != null)
                empmaster.JoiningDate = Convert.ToDateTime(model.JoiningDate);
            if (_IUserService.UpdateEmployeeLeavingDetails(empmaster))
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);

            }
        }

        //index
        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult ManageEmployeeIndex()
        {

            var list = _IProfessionalDetailsService.GetProffesionalDetails().ToList();



            List<ManageEmployeesListModel> ModelList = new List<ManageEmployeesListModel>();


            ModelList = list
            .Select(x => new ManageEmployeesListModel()
            {
                Department = x.DepartmentName,
                Designation = x.Designation,
                ID = x.EmpProfID,
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                TotalExprInMonths = Convert.ToString(x.TotalExprInMonths),
                TotalExprInYears = Convert.ToString(x.TotalExprInYears),
                TotalExperience = x.TotalExprInMonths.HasValue && x.TotalExprInYears.HasValue ? (Convert.ToString(x.TotalExprInYears) + "." + Convert.ToString(x.TotalExprInMonths)) : string.Empty
            })
            .ToList();



            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;

            return View("ManageEmployees", ModelList.ToPagedList(1, pageSize));


        }
        public IPagedList<ManageEmployeesListModel> ManageEmployeePagedList(int? pageNo, int? pageSize)
        {

            List<ManageEmployeesListModel> ModelList = new List<ManageEmployeesListModel>();
            var list = _IProfessionalDetailsService.GetProffesionalDetails().ToList();

            ModelList = list
            .Select(x => new ManageEmployeesListModel()
            {
                Department = x.DepartmentName,
                Designation = x.Designation,
                ID = x.EmpProfID,
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                TotalExprInMonths = Convert.ToString(x.TotalExprInMonths),
                TotalExprInYears = Convert.ToString(x.TotalExprInYears),
                TotalExperience = x.TotalExprInMonths.HasValue && x.TotalExprInYears.HasValue ? (Convert.ToString(x.TotalExprInYears) + "." + Convert.ToString(x.TotalExprInMonths)) : string.Empty
            })
            .ToList();

            return ModelList.ToPagedList(pageNo.Value, pageSize.Value);
        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult ManageEmployeeList(int? pageNo, string searchString = "")
        {
            var list = _IProfessionalDetailsService.GetProffesionalDetails().ToList();

            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
                list = list.Where(m => m.EmployeeName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();



            var ModelList = list
             .Select(x => new ManageEmployeesListModel()
             {
                 Department = x.DepartmentName,
                 Designation = x.Designation,
                 ID = x.EmpProfID,
                 EmpName = x.EmployeeName,
                 EmpNo = x.EmpNo,
                 TotalExprInMonths = Convert.ToString(x.TotalExprInMonths),
                 TotalExprInYears = Convert.ToString(x.TotalExprInYears),
                 TotalExperience = x.TotalExprInMonths.HasValue && x.TotalExprInYears.HasValue ? (Convert.ToString(x.TotalExprInYears) + "." + Convert.ToString(x.TotalExprInMonths)) : string.Empty
             })
             .ToList();

            if (Request.HttpMethod != "GET")
            {
                pageNo = 1;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
            int pageNumber = (pageNo ?? 1);
            return PartialView("_ManageEmployeeList", ModelList.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult EditProffesionalDetails(string EmpNo, int ID = 0)
        {

            ManageEmployeeModel model = new ManageEmployeeModel();

            var list = _IProfessionalDetailsService.GetProffesionalDetails().ToList();

            if (ID != 0)
            {

                model = list.Where(x => x.EmpProfID == ID)
                .Select(x => new ManageEmployeeModel()
                {
                    Department = x.DepartmentName,
                    DepartmentId = x.DepartmentID.HasValue ? x.DepartmentID : 0,
                    DesignationID = x.DesignationID.HasValue ? x.DesignationID : 0,
                    Designation = x.Designation,
                    ID = x.EmpProfID,
                    EmpName = x.EmployeeName,
                    EmpNo = x.EmpNo,
                    TotalExprInmonths = x.TotalExprInMonths,
                    TotalExprInYears = x.TotalExprInYears,
                    UserId = Convert.ToInt32(x.UserID)
                })
                .FirstOrDefault();
            }
            else //try to get Employee data
            {
                var data = _IEmployeeService.GetAll(null, null, "").Where(m => m.EmpNo == EmpNo).FirstOrDefault();

                if (data != null)
                {
                    model.EmpName = data.EmployeeName;
                    model.EmpNo = data.EmpNo;

                }


            }

            ViewBag.Designation = GetDesignationList();
            ViewBag.Department = GetDepartments();


            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;

            return View("_EditProfessionalDeatils", model);


        }


        [HttpPost]
        [Authorize]
        [ValidateRole]
        public ActionResult EditProffesionalDetails(ManageEmployeeModel model)
        {
            EmployeeMaster empObj = new EmployeeMaster();

            empObj.EmpNo = model.EmpNo;
            empObj.EmployeeName = model.EmpName;

            EmployeeProfessionalDetail profObj = new EmployeeProfessionalDetail();

            if (model.DesignationID.HasValue)
                profObj.DesignationID = Convert.ToInt16(model.DesignationID);
            else
                profObj.DesignationID = 0;

            profObj.UserID = model.UserId;

            if (model.DepartmentId.HasValue)
                profObj.DepartmentID = Convert.ToInt16(model.DepartmentId);
            else
                profObj.DepartmentID = 0;
            profObj.TotalExprInMonths = Convert.ToInt32(model.TotalExprInmonths.Value);
            profObj.TotalExprInYears = Convert.ToInt32(model.TotalExprInYears.Value);
            profObj.EmpProfID = model.ID.HasValue ? model.ID.Value : 0;

            bool resut = _IProfessionalDetailsService.UpdateProfessionalDetails(empObj, profObj);
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        public SelectList GetDepartments()
        {
            List<Master_Department> List = new List<Master_Department>();

            List = _IProfessionalDetailsService.GetDepartMentList();
            List = List.OrderBy(x => x.DepartmentName).ToList();
            //Code change - EDMX Fix
            SelectList selList = new SelectList(List, "DepartmentID", "DepartmentName");
            return selList;
        }

        public SelectList GetDesignationList()
        {
            List<Master_Designation> List = new List<Master_Designation>();

            List = _IProfessionalDetailsService.GetDesignationList();
            List = List.OrderBy(x => x.Designation).ToList();
            //Code change - EDMX Fix
            SelectList selList = new SelectList(List, "DesignationID", "Designation");
            return selList;
        }

        /// <summary>
        /// Code change - adding new method for education subcategory
        /// </summary>
        /// <returns></returns>
        public List<EducationCategory> GetEducationCategories()
        {
            List<Master_EducationCategory> List = new List<Master_EducationCategory>();

            List = _IEducationCategoryService.GetAll(null, null, "").ToList();

            List<EducationCategory> result = new List<EducationCategory>();

            foreach (var item in List)
            {
                result.Add(new EducationCategory { Id = item.EducationCategoryID, Name = item.EducationCategory });
            }

            return result;
        }

        public List<EducationCategory> GetSelectedCategories(long Id)
        {
            List<Master_EducationCategory> eduCatList = new List<Master_EducationCategory>();

            eduCatList = _IEducationCategoryService.GetAll(null, null, "").ToList();

            //var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);

            List<AdminEducationCategoryForUser> List = new List<AdminEducationCategoryForUser>();

            List = _IUserService.GetSelectedCategories(Id);

            List<EducationCategory> result = new List<EducationCategory>();

            foreach (var item in List)
            {
                result.Add(new EducationCategory
                {
                    Id = item.EducationCategoryId.Value,
                    Name = eduCatList.SingleOrDefault(e => e.EducationCategoryID == item.EducationCategoryId).EducationCategory
                });
            }

            return result;
        }

        //[HttpGet]
        //public ActionResult ChangePassword()
        //{
        //    ChangePassword pwd = new ChangePassword();
        //    pwd.UserID = SessionManager.UserId;
        //    return View(pwd);
        //}

        [HttpGet]
        [Authorize]
        public JsonResult SaveNewPassword(int userid = 0, string pevpwd = "", string newpwd = "")
        {
            bool status = false;
            if (!string.IsNullOrWhiteSpace(pevpwd) && !string.IsNullOrWhiteSpace(newpwd) && userid != 0)
            {
                var usermodel = _IUserService.GetById(userid);
                if (SessionManager.DecryptData(usermodel.Password) == pevpwd)
                {
                    usermodel.Password = SessionManager.EncryptData(newpwd); //assign new pwd
                    status = _IUserService.Update(usermodel, null, "");
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetPreviousPwd(string prevpwd = "")
        {
            string pwd = null;
            string msg = null;

            long userid = Convert.ToInt64(SessionManager.UserId);
            if (userid != 0)
            {
                var model = _IUserService.GetById(userid);
                if (model != null)
                {
                    pwd = model.Password;
                }
            }
            if (!string.IsNullOrWhiteSpace(prevpwd))
            {
                if (prevpwd == SessionManager.DecryptData(pwd))
                {
                    msg = "same pwd";
                }
                else
                {
                    msg = "Please enter correct existing password";
                }
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public JsonResult CheckUniqueEmail(string email = "")
        {
            bool status = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var model = _IUserService.GetAll(null, null, "").Where(x => x.Email == email && x.IsDelete == false).FirstOrDefault();
                if (model != null)
                {
                    if (model.Email == email)
                        status = true;
                }
            }


            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult ExitedEmployees(int? pageIndex, string sortOrder, string searchString = "")
        {
            AddEmployeeModel model = new AddEmployeeModel();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var EmployeeList = new List<AddEmployeeModelList>();
                model.EmployeeDetailsList = ExitedEmployeePagedList(pageIndex, searchString, sortOrder);
                return View(model);
            }
            return View(model);
        }

        public IPagedList<AddEmployeeModelList> ExitedEmployeePagedList(int? pageIndex, string searchString, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeNoParm = sortOrder == "EmployeeNo_ASC" ? "EmployeeNo_DESC" : "EmployeeNo_ASC";
            ViewBag.EmployeeNameParm = sortOrder == "EmployeeName_ASC" ? "EmployeeName_DESC" : "EmployeeName_ASC";

            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();
            var LoginList = _IUserService.GetAll(null, null, "").Where(m => m.IsDelete == true).ToList();


            if (LoginList != null)
            {

                foreach (var item in LoginList)
                {
                    var designation = _IUserService.GetDesignationName(item.DesignationID);
                    var department = _IUserService.GetDepartmentName(item.DepartmentID);
                    string employeeNumber = "Not Exist";
                    var recordExist = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).ToList();
                    if (recordExist.Count != 0)
                        employeeNumber = recordExist.FirstOrDefault().EmpNo;

                    AddEmployeeModelList AddModel = new AddEmployeeModelList();
                    AddModel.EmpName = item.FirstName + " " + item.LastName;
                    AddModel.Email = item.Email;
                    AddModel.EmpNo = employeeNumber;
                    AddModel.JoiningDate = item.JoiningDate;
                    AddModel.ShortJoiningDate = convertDateToShort(item.JoiningDate);
                    AddModel.LeavingDate = null;
                    AddModel.ID = item.UserID;
                    AddModel.isActive = item.IsActive == 1 ? true : false;

                    //code change - edmx fix 
                    AddModel.ContactNumber = item.ContactNumber;
                    AddModel.DepartmentID = item.DepartmentID;
                    AddModel.DesignationID = item.DesignationID;
                    AddModel.Department = department;
                    AddModel.Designation = designation;
                    AddModel.ProjectName = item.ProjectName;
                    AddModel.RequisitionID = item.RequisitionID;
                    AddModel.JoiningLocation = item.JoiningLocation;

                    ModelList.Add(AddModel);
                }
            }

            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
            {
                ModelList = ModelList.ToList().Where(m => m.EmpName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.Contains("EmployeeNo_DESC"))
                    ModelList = ModelList.OrderByDescending(m => m.EmpNo).ToList();
                if (sortOrder.Contains("EmployeeNo_ASC"))
                    ModelList = ModelList.OrderBy(m => m.EmpNo).ToList();

                if (sortOrder.Contains("EmployeeName_DESC"))
                    ModelList = ModelList.OrderByDescending(m => m.EmpName).ToList();
                if (sortOrder.Contains("EmployeeName_ASC"))
                    ModelList = ModelList.OrderBy(m => m.EmpName).ToList();
            }
            else
            {
                ModelList = ModelList.OrderByDescending(m => m.EmpName).ToList();
            }
            if (Request.HttpMethod != "GET")
            {
                pageIndex = 1;
            }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);
            int? pageNumber = 1;
            pageNumber = pageIndex != null ? pageIndex : 1;
            ViewBag.PageIndex = pageNumber;
            ViewBag.SearchString = searchString;
            return ModelList.ToPagedList(pageNumber.Value, pageSize);
        }


        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult ExitedEmployeeList(int pageIndex, string sortOrder, string searchString = "")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeNoParm = sortOrder == "EmployeeNo_ASC" ? "EmployeeNo_DESC" : "EmployeeNo_ASC";
            ViewBag.EmployeeNameParm = sortOrder == "EmployeeName_ASC" ? "EmployeeName_DESC" : "EmployeeName_ASC";

            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();

            var LoginList = _IUserService.GetAll(null, null, "").Where(m => m.IsDelete == true).ToList();
            if (LoginList != null)
            {

                foreach (var item in LoginList)
                {
                    var designation = _IUserService.GetDesignationName(item.DesignationID);
                    var department = _IUserService.GetDepartmentName(item.DepartmentID);
                    string employeeNumber = "Not Exist";
                    var recordExist = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).ToList();
                    if (recordExist.Count != 0)
                        employeeNumber = recordExist.FirstOrDefault().EmpNo;

                    AddEmployeeModelList AddModel = new AddEmployeeModelList();
                    AddModel.EmpName = item.FirstName + " " + item.LastName;
                    AddModel.Email = item.Email;
                    AddModel.EmpNo = employeeNumber;
                    AddModel.JoiningDate = item.JoiningDate;
                    AddModel.LeavingDate = null;
                    AddModel.ID = item.UserID;
                    AddModel.isActive = item.IsActive == 1 ? true : false;

                    AddModel.ContactNumber = item.ContactNumber;
                    AddModel.DepartmentID = item.DepartmentID;
                    AddModel.DesignationID = item.DesignationID;
                    AddModel.Department = department;
                    AddModel.Designation = designation;
                    AddModel.ProjectName = item.ProjectName;
                    AddModel.RequisitionID = item.RequisitionID;
                    AddModel.JoiningLocation = item.JoiningLocation;
                    AddModel.ShortJoiningDate = convertDateToShort(item.JoiningDate);

                    ModelList.Add(AddModel);
                }
            }

            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
            {
                ModelList = ModelList.ToList().Where(m => m.EmpName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.Contains("EmployeeNo_DESC"))
                    ModelList = ModelList.OrderByDescending(m => m.EmpNo).ToList();
                if (sortOrder.Contains("EmployeeNo_ASC"))
                    ModelList = ModelList.OrderBy(m => m.EmpNo).ToList();

                if (sortOrder.Contains("EmployeeName_DESC"))
                    ModelList = ModelList.OrderByDescending(m => m.EmpName).ToList();
                if (sortOrder.Contains("EmployeeName_ASC"))
                    ModelList = ModelList.OrderBy(m => m.EmpName).ToList();
            }
            else
            {
                ModelList = ModelList.OrderByDescending(m => m.EmpName).ToList();
            }
            if (Request.HttpMethod != "GET")
            {
                pageIndex = 1;
            }
            int pageSize = 10;
            int pageNumber = 1;
            pageNumber = pageIndex != 1 ? pageIndex : 1;
            ViewBag.PageIndex = pageNumber;
            ViewBag.SearchString = searchString;
            return PartialView("_ExitedEmployeeList", ModelList.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        [CLSCompliant(false)]
        [ValidateRole]
        public ActionResult GetExitedEmployeeSorted(string sortOrder, int pageIndex, string searchString = "")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeNoParm = sortOrder == "EmployeeNo_ASC" ? "EmployeeNo_DESC" : "EmployeeNo_ASC";
            ViewBag.EmployeeNameParm = sortOrder == "EmployeeName_ASC" ? "EmployeeName_DESC" : "EmployeeName_ASC";

            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();

            var LoginList = _IUserService.GetAll(null, null, "").Where(m => m.IsDelete == true).ToList();
            if (LoginList != null)
            {
                foreach (var item in LoginList)
                {
                    string employeeNumber = "Not Exist";
                    var recordExist = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).ToList();
                    if (recordExist.Count != 0)
                        employeeNumber = recordExist.FirstOrDefault().EmpNo;
                    AddEmployeeModelList AddModel = new AddEmployeeModelList();
                    AddModel.EmpName = item.FirstName + " " + item.LastName;
                    AddModel.Email = item.Email;
                    AddModel.EmpNo = employeeNumber;
                    AddModel.JoiningDate = item.JoiningDate;
                    AddModel.LeavingDate = null;
                    AddModel.ID = item.UserID;
                    AddModel.isActive = item.IsActive == 1 ? true : false;

                    ModelList.Add(AddModel);
                }
            }
            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
            {
                ModelList = ModelList.ToList().Where(m => m.EmpName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.Contains("EmployeeNo_DESC"))
                    ModelList = ModelList.OrderByDescending(m => m.EmpNo).ToList();
                if (sortOrder.Contains("EmployeeNo_ASC"))
                    ModelList = ModelList.OrderBy(m => m.EmpNo).ToList();

                if (sortOrder.Contains("EmployeeName_DESC"))
                    ModelList = ModelList.OrderByDescending(m => m.EmpName).ToList();
                if (sortOrder.Contains("EmployeeName_ASC"))
                    ModelList = ModelList.OrderBy(m => m.EmpName).ToList();
            }
            else
            {
                ModelList = ModelList.OrderByDescending(m => m.EmpName).ToList();
            }

            if (Request.HttpMethod != "GET")
            {
                pageIndex = 1;
            }
            int pageSize = 10;
            int pageNumber = 1;
            pageNumber = pageIndex != 1 ? pageIndex : 1;
            ViewBag.PageIndex = pageNumber;
            ViewBag.SearchString = searchString;
            return PartialView("_ExitedEmployeeList", ModelList.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult GetEmployeeDetails(string EmpNo)
        {
            var EmployeeList = _IEmployeeService.GetAll(null, null, "").Where(T => T.EmpNo == EmpNo).FirstOrDefault();

            return Json(new
            {
                EmployeeName = EmployeeList.EmployeeName,
                EmpNo = EmployeeList.EmpNo,
                Email = SessionManager.DecryptData(EmployeeList.Email),
                DOB = (EmployeeList.DOB != null) ? SessionManager.DecryptData(EmployeeList.DOB) : string.Empty,//SessionManager.DecryptData(EmployeeList.DOB),
                JoiningDate = convertDateToShort(EmployeeList.JoiningDate)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditExitedEmployees(string EmpNo, int UserId)
        {
            AddEmployeeModel Model = new AddEmployeeModel();
            var employeeList = _IEmployeeService.GetAll(null, null, "").ToList();
            EmployeeMaster employeeData = employeeList.Where(x => x.UserId == UserId).FirstOrDefault();
            Model.DOB = (employeeData.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(employeeData.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;//Convert.ToDateTime(SessionManager.DecryptData(employeeData.DOB)).ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            Model.EmpName = employeeData.EmployeeName;
            Model.LeavingDate = convertDateToShort(employeeData.LeavingDate);
            if (employeeData.JoiningDate != new DateTime())
            {
                Model.JoiningDate = convertDateToShort(employeeData.JoiningDate);
            }

            Model.EmpNo = employeeData.EmpNo;
            Model.Email = SessionManager.DecryptData(employeeData.Email);
            Model.IsActive = employeeData.Active == 0 ? false : true;
            Model.UserID = Convert.ToInt32(employeeData.UserId);
            Model.EmployeeMasterId = Convert.ToInt32(employeeData.Id);
            return View("_EditExitedEmployee", Model);

        }

        public ActionResult AddToUserActivation(int UserId)
        {
            bool status = _IUserService.AddToUserActivation(UserId);
            return Json(new { result = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditExitedEmployees(AddEmployeeModel model)
        {
            if (ModelState.ContainsKey("ReasonforLeaving"))
                ModelState["ReasonforLeaving"].Errors.Clear();

            if (ModelState.ContainsKey("LeavingDate"))
                ModelState["LeavingDate"].Errors.Clear();

            if (ModelState.IsValid)
            {
                //Executed when employee is edited from employee details page
                EmployeeMaster obj = new EmployeeMaster();
                obj.EmployeeName = model.EmpName;
                obj.EmpNo = model.EmpNo;
                obj.Email = SessionManager.EncryptData(model.Email);
                obj.DOB = SessionManager.EncryptData(String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(model.DOB)));
                obj.Active = model.IsActive == true ? 1 : 0;
                if (model.JoiningDate != null)
                {
                    obj.JoiningDate = Convert.ToDateTime(model.JoiningDate);
                }
                if (model.LeavingDate != null)
                {
                    obj.LeavingDate = Convert.ToDateTime(model.LeavingDate);
                }
                obj.UserId = model.UserID;
                obj.Id = model.EmployeeMasterId;
                bool status = _IUserService.UpdateEmployeeDetails(obj);
                return Json(new { result = status }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsEmpNoExist(string EmpNo, int EmployeeMasterId)
        {
            bool ifEmailExist = false;
            try
            {
                var employeedata = _IEmployeeService.GetAll(null, null, "").Where(T => T.EmpNo.ToLower().Equals(EmpNo.ToLower()) && T.Id != EmployeeMasterId).FirstOrDefault();

                ifEmailExist = employeedata != null ? true : false;
                return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsEmailExist(string Email, int EmployeeMasterId, int UserID)
        {
            bool ifEmailExist = false;
            try
            {
                string EncEmail = SessionManager.EncryptData(Email.ToLower());
                var employeedata = _IEmployeeService.GetAll(null, null, "").Where(T => T.Email.Equals(EncEmail) && T.Id != EmployeeMasterId).FirstOrDefault();

                ifEmailExist = employeedata != null ? true : false;
                if (employeedata == null && UserID != null && UserID > 0)
                {
                    var Logindata = _IUserService.GetAll(null, null, "").Where(T => T.Email.Equals(Email.ToLower()) && T.UserID != UserID).FirstOrDefault();
                    ifEmailExist = Logindata != null ? true : false;
                }
                return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsUserEmailExist(string Email, int UserID)
        {
            bool ifEmailExist = false;
            try
            {
                var employeedata = _IUserService.GetAll(null, null, "").Where(T => T.Email.Equals(Email.ToLower()) && T.UserID != UserID).FirstOrDefault();

                ifEmailExist = employeedata != null ? true : false;

                if (!ifEmailExist)
                {
                    var roleData = _IRoleService.GetAll(null, null, "").Where(p => p.UserName.ToLower() == Email.ToLower()).FirstOrDefault();
                    ifEmailExist = roleData != null ? true : false;
                }

                return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsFirstNameLastNameDOBDuplicateCheck(string firstName, string lastName, string DOB, int userId, string userName)
        {
            bool ifRecordExist = false;
            try
            {
                if (userId != 0)
                {
                    var employeedata = _IUserService.GetAll(null, null, "").Where(T => T.UserID != userId && T.FirstName.ToLower().Equals(firstName.ToLower()) && T.LastName.ToLower().Equals(lastName.ToLower()) && T.DOB.DecryptData().Equals(DOB) && T.IsDelete == false).FirstOrDefault();
                    ifRecordExist = employeedata != null ? true : false;
                }
                else
                {
                    var employeedata = _IUserService.GetAll(null, null, "").Where(T => T.FirstName.ToLower().Equals(firstName.ToLower()) && T.LastName.ToLower().Equals(lastName.ToLower()) && T.DOB.DecryptData().Equals(DOB) && T.IsDelete == false).FirstOrDefault();
                    ifRecordExist = employeedata != null ? true : false;

                }

                //Added check for roles - It will check wheter email address is already existed in roles master or not?

                var roleData = _IRoleService.GetAll(null, null, "").Where(p => p.UserName.ToLower() == userName.ToLower());
                if (roleData != null && roleData.Count() > 0)
                {
                    ifRecordExist = true;
                }

                return Json(ifRecordExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateRole]
        public ActionResult DirectExitEmp(int EmployeeMasterId)
        {
            try
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
                var employeedata = _IEmployeeService.GetAll(null, null, "").Where(T => T.Id == EmployeeMasterId).FirstOrDefault();
                employeedata.LeavingDate = DateTime.UtcNow;
                employeedata.Active = 0;
                if (_IUserService.UpdateEmployeeLeavingDetails(employeedata))
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult DirectExitEmployee(int? page)
        {
            AddEmployeeModel model = new AddEmployeeModel();

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var user = _IUserService.GetAll(null, null, "").Where(m => m.UserID != SessionManager.UserId && m.IsDelete == false).ToList();

                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]); //5;
                int pageNumber = (page ?? 1);

                var OfferCandidatesList = new List<OfferCandidateModel>();

                var EmployeeList = new List<AddEmployeeModelList>();

                model.EmployeeDetailsList = EmployeePagedList(1, pageSize);
                return View(model);

            }


            return View(model);
        }


        [HttpGet]
        [Authorize]
        [ValidateRole]
        public ActionResult DirectExitEmployeeList(int? pageNo, string searchString = "")
        {

            var EmployeeList = _IEmployeeService.GetAll(null, null, "").ToList();

            if (!string.IsNullOrWhiteSpace(searchString) && searchString != "ALL")
                EmployeeList = EmployeeList.Where(m => m.Active == 1).ToList().Where(m => m.EmployeeName.ToLower().Contains(searchString.ToLower()) || m.EmpNo.ToLower().Contains(searchString.ToLower())).ToList();

            List<AddEmployeeModelList> ModelList = new List<AddEmployeeModelList>();
            ModelList = EmployeeList
            .Select(x => new AddEmployeeModelList()
            {
                DOB = (x.DOB != null) ? SessionManager.DecryptData(x.DOB) : string.Empty,//SessionManager.DecryptData(x.DOB),
                Email = SessionManager.DecryptData(x.Email),
                EmpName = x.EmployeeName,
                EmpNo = x.EmpNo,
                JoiningDate = x.JoiningDate,
                ReasonforLeaving = x.ReasonForleaving,
                ID = x.Id,
                isActive = x.Active == 1 ? true : false
            }).ToList();

            if (Request.HttpMethod != "GET")
            {
                pageNo = 1;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSizeEmp"]); //5;
            int pageNumber = (pageNo ?? 1);
            return PartialView("_DirectExitEmployeeList", ModelList.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult GetUserDetails(int userId)
        {
            ViewBag.UserId = userId;
            return View("~/Views/User/UserDetailsMain.cshtml");
        }

        /// <summary>
        /// Code change - Added new action method for dispalying user document details for individual
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult GetUserDocumentDetails(int userId, int? page = 1, string sortOrder = "", string searchString = "")
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CandidateNameParm = sortOrder == "CandidateName_ASC" ? "CandidateName_DESC" : "CandidateName_ASC";
            ViewBag.StatusParm = sortOrder == "Status_ASC" ? "Status_DESC" : "Status_ASC";

            List<DocumentStatus_Result> list;
            var user = _IUserService.GetAll(null, null, "").Where(m => (m.UserID != SessionManager.UserId) && (m.Email.ToLower().Contains(searchString) || m.FirstName.ToLower().Contains(searchString) || m.LastName.ToLower().Contains(searchString)));
            if (string.IsNullOrEmpty(searchString))
                list = _IUserService.DocumentStatusList("0"); // Passing 0 for all users
            else
            {
                searchString = searchString.ToLower();
                list = _IUserService.DocumentStatusList("0").Where(m => m.CandidateName.ToLower().Contains(searchString) || m.DocumentName.ToLower().Contains(searchString) || m.Category.ToLower().Contains(searchString) || m.Document.ToLower().Contains(searchString)).ToList(); // Passing 0 for all users
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.Contains("CandidateName_DESC"))
                    list = list.OrderByDescending(m => m.CandidateName).ToList();
                if (sortOrder.Contains("CandidateName_ASC"))
                    list = list.OrderBy(m => m.CandidateName).ToList();
                if (sortOrder.Contains("Status_DESC"))
                    list = list.OrderByDescending(m => m.IsVerify).ToList();
                if (sortOrder.Contains("Status_ASC"))
                    list = list.OrderBy(m => m.IsVerify).ToList();
            }
            else
            {
                list = list.OrderBy(m => m.CandidateName).ToList();
            }
            // var intMandatoryDocs = _ISubDocumentCategoryService.GetAll(null, null, "").Where(x => x.IsNeeded == true).Count();

            List<DocDetailsModel> model = new List<DocDetailsModel>();

            string strWebUrl = ConfigurationManager.AppSettings["DocumentPath"];

            model = list
           .Select(x => new DocDetailsModel()
           {
               Id = x.DocDetID,
               UserId = x.UserId,
               CandidateName = x.CandidateName,
               DocumentName = x.DocumentName,
               FilePath = strWebUrl + x.FilePath,
               Maincategory = x.Category,
               SubCategory = x.Document,
               IsVerify = x.IsVerify,
               Status = x.Status,
               // MandatoryDocs = intMandatoryDocs
           })
           .ToList();

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //create group by on candidate 
            var GroupCandidate = (from obj in model
                                  group obj by obj.CandidateName into empg
                                  select new
                                  {
                                      NAME = empg.Key,
                                      DocumentCount = empg.Count(x => x.Id > 0 ? true : false),
                                      ApprovedDocs = empg.Count(x => (x.IsVerify.HasValue ? x.IsVerify.Value == true : false))
                                      ,
                                      RejectedDocs = empg.Count(x => x.IsVerify.HasValue ? x.IsVerify.Value == false : false)
                                  }).ToList();

            List<DocDetailsModel> data = new List<DocDetailsModel>();

            foreach (var item in GroupCandidate)
            {
                if (item.DocumentCount == item.ApprovedDocs)
                {
                    data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        CandidateName = x.CandidateName,
                        DocumentName = x.DocumentName,
                        FilePath = strWebUrl + x.FilePath,
                        Maincategory = x.Maincategory,
                        SubCategory = x.SubCategory,
                        IsVerify = x.IsVerify,
                        Status = x.Status,
                        //MandatoryDocs = intMandatoryDocs,
                        isAllApproved = true
                    }).Where(d => d.UserId == userId).ToList());
                }
                else if (item.DocumentCount == item.RejectedDocs)
                {
                    data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        CandidateName = x.CandidateName,
                        DocumentName = x.DocumentName,
                        FilePath = strWebUrl + x.FilePath,
                        Maincategory = x.Maincategory,
                        SubCategory = x.SubCategory,
                        IsVerify = x.IsVerify,
                        Status = x.Status,
                        //MandatoryDocs = intMandatoryDocs,
                        isAllRejected = true
                    }).Where(d => d.UserId == userId).ToList());
                }
                else
                {
                    data.AddRange(model.Where(c => c.CandidateName == item.NAME).Select(x => new DocDetailsModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        CandidateName = x.CandidateName,
                        DocumentName = x.DocumentName,
                        FilePath = strWebUrl + x.FilePath,
                        Maincategory = x.Maincategory,
                        SubCategory = x.SubCategory,
                        IsVerify = x.IsVerify,
                        Status = x.Status,
                        //MandatoryDocs = intMandatoryDocs
                    }).Where(d => d.UserId == userId).ToList());
                }
            }


            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]); //5;
            int pageNumber = 1;
            pageNumber = page.Value != 1 ? page.Value : 1;
            ViewBag.PageIndex = pageNumber;
            ViewBag.SearchString = searchString;
            ViewBag.DocUserId = userId;
            //ViewBag.DocName=

            return PartialView("_UserDocList", data.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Action method to open change request form for candidate
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OpenChangeRequestForm()
        {
            ViewBag.CountryCodeList = GetCountryCode();

            //Get logged in user details 
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var LoginDetails = _IUserService.GetById(userId);

            var employeePersonalDetails = new ChangeRequestDetails();

            if (LoginDetails != null)
            {

                _Logindetails = LoginDetails;
                employeePersonalDetails.FirstName = _Logindetails.FirstName;
                employeePersonalDetails.LastName = _Logindetails.LastName;
                employeePersonalDetails.ContactNumber = _Logindetails.ContactNumber;
                employeePersonalDetails.EmpEmail = _Logindetails.Email;
                employeePersonalDetails.DateofBirth = (_Logindetails.DOB != null) ? SessionManager.DecryptData(_Logindetails.DOB) : string.Empty; //SessionManager.DecryptData(_Logindetails.DOB);
                employeePersonalDetails.CountryCode = _Logindetails.CountryCode;

            }

            Mapper.CreateMap<Models.ChangeRequestDetails, Models.ChangeRequestDetails>();
            var user = Mapper.Map<Models.ChangeRequestDetails, Models.ChangeRequestDetails>(employeePersonalDetails);
            user.DateofBirth = (user.DateofBirth != "") ? Convert.ToDateTime(user.DateofBirth).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            return PartialView("_ChangeRequestForm", user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveChangeRequestDetails(ChangeRequestDetails model)
        {
            //get user details from db from logindetails table
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var LoginDetails = _IUserService.GetById(userId);

            //Match first name 
            //if not same in logindetails create obj of CandidateChangeRequestsDetail 
            //Add to db - hardcode field name as firstname 

            if (!string.IsNullOrWhiteSpace(model.FirstName))
            {
                if (LoginDetails.FirstName.ToLower().Trim() != model.FirstName.ToLower().Trim())
                {
                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "FirstName";
                    obj.FieldValue = model.FirstName;
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = LoginDetails.FirstName;

                    var result = _IUserService.AddChangeRequestDetails(obj);

                }
            }
            if (!string.IsNullOrWhiteSpace(model.LastName))
            {
                if (LoginDetails.LastName.ToLower().Trim() != model.LastName.ToLower().Trim())
                {
                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "LastName";
                    obj.FieldValue = model.LastName;
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = LoginDetails.LastName;
                    var result = _IUserService.AddChangeRequestDetails(obj);

                }
            }
            if (!string.IsNullOrWhiteSpace(model.ContactNumber))
            {

                if (LoginDetails.ContactNumber.ToLower().Trim() != model.ContactNumber.ToLower().Trim())
                {
                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "ContactNumber";
                    obj.FieldValue = model.ContactNumber;
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = LoginDetails.ContactNumber;
                    var result = _IUserService.AddChangeRequestDetails(obj);

                }
            }
            if (!string.IsNullOrWhiteSpace(model.CountryCode))
            {
                if (LoginDetails.CountryCode.ToLower().Trim() != model.CountryCode.ToLower().Trim())
                {
                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "CountryCode";
                    obj.FieldValue = model.CountryCode;
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = LoginDetails.CountryCode;
                    var result = _IUserService.AddChangeRequestDetails(obj);

                }
            }
            if (!string.IsNullOrWhiteSpace(model.EmpEmail))
            {
                if (LoginDetails.Email.ToLower().Trim() != model.EmpEmail.ToLower().Trim())
                {
                    var isEmailPresent = _IUserService.GetAll(null, null, "").Where(T => T.Email.ToLower().Equals(model.EmpEmail.ToLower().Trim())).Any();
                    if (isEmailPresent)
                    {
                        return Json(new { result = false, Message = "Email Id already exists!" }, JsonRequestBehavior.AllowGet);
                    }

                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "Email";
                    obj.FieldValue = model.EmpEmail;
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = LoginDetails.Email;
                    var result = _IUserService.AddChangeRequestDetails(obj);

                }
            }
            DateTime oldDob, newDob;
            if (!string.IsNullOrWhiteSpace(model.DateofBirth))
            {
                if (DateTime.TryParse(SessionManager.DecryptData(LoginDetails.DOB).Trim(), out oldDob) && DateTime.TryParse(model.DateofBirth.Trim(), out newDob))
                {
                    CandidateChangeRequestsDetail obj = new CandidateChangeRequestsDetail();
                    obj.UserID = userId;
                    obj.FieldName = "DOB";
                    obj.FieldValue = model.DateofBirth;
                    //obj.IsApproved = false;
                    obj.IsApproved = null;
                    obj.CreatedBy = userName;
                    obj.UpdatedBy = userName;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.OldValue = SessionManager.DecryptData(LoginDetails.DOB).Trim();
                    if (oldDob != newDob)
                    {
                        _IUserService.AddChangeRequestDetails(obj);
                    }

                }
            }
            return Json(new { result = true, Message = "Change request has been sent successfully!" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult CheckPendingRequests()
        {

            //get user details from db from logindetails table
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            //get pending details those are neither reject nor approved
            List<string> pendingDetails = _IUserService.GetPendingRequests(userId);

            return Json(pendingDetails, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CheckIfThereIsAnyChange(ChangeRequestDetails model)
        {

            //get user details from db from logindetails table
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            //get login details
            var LoginDetails = _IUserService.GetById(userId);

            //All the fields on change request  fields should have values entered by Admin 
            if (LoginDetails.FirstName.ToLower().Trim() != model.FirstName.ToLower().Trim() ||
                LoginDetails.LastName.ToLower().Trim() != model.LastName.ToLower().Trim() ||
                LoginDetails.Email.ToLower().Trim() != model.EmpEmail.ToLower().Trim() ||
                SessionManager.DecryptData(LoginDetails.DOB).ToLower().Trim() != model.DateofBirth.ToLower().Trim() ||
              LoginDetails.ContactNumber.ToLower().Trim() != model.ContactNumber.ToLower().Trim() ||
                LoginDetails.CountryCode.ToLower().Trim() != model.CountryCode.ToLower().Trim()
                )
            {
                return Json(true, JsonRequestBehavior.AllowGet); //Change
            }
            else return Json(false, JsonRequestBehavior.AllowGet);


        }

        public ActionResult GetActivityDetails()
        {
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var activityDetails = new ActivityDetails();

            var details = _IUserService.GetActivityDetails(userId);

            if (details != null)
            {
                if (details.PersonalDetailsDate != null)
                {
                    activityDetails.PersonalDetailsDate = TimeZoneInfo.ConvertTimeFromUtc(details.PersonalDetailsDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
                if (details.ContactDetailsDate != null)
                {
                    activityDetails.ContactDetailsDate = TimeZoneInfo.ConvertTimeFromUtc(details.ContactDetailsDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
                if (details.EducationDetailsDate != null)
                {
                    activityDetails.EducationDetailsDate = TimeZoneInfo.ConvertTimeFromUtc(details.EducationDetailsDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
                if (details.EmploymentDetailsDate != null)
                {
                    activityDetails.EmploymentDetailsDate = TimeZoneInfo.ConvertTimeFromUtc(details.EmploymentDetailsDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
                if (details.FamilyDetailsDate != null)
                {
                    activityDetails.FamilyDetailsDate = TimeZoneInfo.ConvertTimeFromUtc(details.FamilyDetailsDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
                if (details.UploadDocumentDetailsDate != null)
                {
                    activityDetails.UploadDocumentDetailsDate = TimeZoneInfo.ConvertTimeFromUtc(details.UploadDocumentDetailsDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
            }

            List<ActivityViewModel> activityList = new List<ActivityViewModel>();

            activityList.Add(new ActivityViewModel { TabName = "Personal Details", LastModifiedDate = activityDetails.PersonalDetailsDate, TabURL = "/User/PersonalDetails" });
            activityList.Add(new ActivityViewModel { TabName = "Contact Details", LastModifiedDate = activityDetails.ContactDetailsDate, TabURL = "/User/ContactDetails" });
            activityList.Add(new ActivityViewModel { TabName = "Education Details", LastModifiedDate = activityDetails.EducationDetailsDate, TabURL = "/Education/GetEducationalDetails" });
            activityList.Add(new ActivityViewModel { TabName = "Employment Details", LastModifiedDate = activityDetails.EmploymentDetailsDate, TabURL = "/Employement/Index" });
            activityList.Add(new ActivityViewModel { TabName = "Family Details", LastModifiedDate = activityDetails.FamilyDetailsDate, TabURL = "/Family/FamilyDetails" });
            activityList.Add(new ActivityViewModel { TabName = "Document Details", LastModifiedDate = activityDetails.UploadDocumentDetailsDate, TabURL = "/document/index" });

            activityList = activityList.OrderByDescending(a => a.LastModifiedDate).ToList();

            return PartialView("_Activity", activityList);


        }

        /// <summary>
        /// Action method to open change password dialog box for candidate.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            var changePassword = new ChangePassword();
            changePassword.UserID = SessionManager.UserId;
            return PartialView("_ChangePasswordForm", changePassword);
        }

        /// <summary>
        /// Change passoword functionality.
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            bool status = false;
            changePassword.PrevPassword = changePassword.PrevPassword.Trim();
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(changePassword.PrevPassword.Trim()) && !string.IsNullOrWhiteSpace(changePassword.NewPassword.Trim()) && changePassword.UserID != 0)
                {
                    if (changePassword.NewPassword.Trim() == changePassword.ConfirmNewPassword.Trim())
                    {
                        var usermodel = _IUserService.GetById(changePassword.UserID);
                        if (SessionManager.DecryptData(usermodel.Password) == changePassword.PrevPassword)
                        {
                            usermodel.Password = SessionManager.EncryptData(changePassword.NewPassword);
                            status = _IUserService.Update(usermodel, null, "");

                            #region send email for changed password
                            try
                            {
                                LoginDetail loginDetail = _IUserService.GetById(changePassword.UserID);
                                string content = "<font face='Cambria' size= '3' color ='black'> "
                                + "Dear " + loginDetail.FirstName + ", <br><br> "
                                + "Your login credentials are : <ul>"
                                + "<li> User Name: <font color ='blue'>" + loginDetail.Email + " </font> </li>"
                                + "<li> New Password: <font>" + changePassword.NewPassword + " </font> </li></ul><br/>"
                                + "<font face='Cambria' size= '3'> From, <br/> Team Silicus!!!</body>";

                                if (loginDetail.Email.ToLower() == "admin")
                                {
                                    loginDetail.Email = ConfigurationManager.AppSettings["adminEmail"];
                                }

                                SendMailToUser(loginDetail, content, "Your password has been changed");
                            }
                            catch (Exception e)
                            {
                                ModelState.AddModelError("", "Issue sending email: " + e.Message);
                            }

                            #endregion

                            return Json(new ChangePasswordResponse { Status = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new ChangePasswordResponse { Message = "Please enter correct old password!", Status = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new ChangePasswordResponse { Message = "New password and confirm password did not match!", Status = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #region Add admin user

        [HttpGet]
        [Authorize]
        public ActionResult AdminUsers(int? page)
        {
            List<LoginDetails> userList = new List<LoginDetails>();

            var adminUsers = _IUserService.GetAll(null, null, "").Where(m => (m.RoleID == 1)).ToList();
            foreach (var item in adminUsers)
            {
                var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == item.UserID).FirstOrDefault();
                var designation = _IUserService.GetDesignationName(item.DesignationID);
                userList.Add(new LoginDetails()
                {
                    EmpNo = Employee == null ? string.Empty : Employee.EmpNo,
                    ActivatedDate = item.ActivatedDate,
                    Active = item.IsActive,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    DOB = (item.DOB != null) ? Convert.ToDateTime(SessionManager.DecryptData(item.DOB)) : (DateTime?)null, //Convert.ToDateTime(SessionManager.DecryptData(item.DOB)),
                    JoiningDate = item.JoiningDate,
                    ShortDOB = (item.DOB != null) ? convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))) : string.Empty, //convertDateToShort(Convert.ToDateTime(SessionManager.DecryptData(item.DOB))),
                    ShortJoiningDate = convertDateToShort(item.JoiningDate),
                    UserId = item.UserID,
                    IsOnboarded = Employee == null ? false : true,
                    ContactNumber = item.ContactNumber,
                    DesignationID = item.DesignationID,
                    JoiningLocation = item.JoiningLocation,
                    Designation = designation,
                });
            }

            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PagingSize"]);
            int pageNumber = (page ?? 1);

            ViewBag.PageIndex = pageNumber;

            return View("AdminUsers", userList.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddAdminUser()
        {
            var loginDetails = new AddEditUserModel();
            return PartialView("_AddAdminUser", loginDetails);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddAdminUser(AddEditUserModel model)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                    userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
                }

                LoginDetail adminUserDetail = new LoginDetail();
                adminUserDetail.CreatedDate = DateTime.UtcNow;
                adminUserDetail.FirstName = model.FirstName;
                adminUserDetail.LastName = model.LastName;
                adminUserDetail.Password = SessionManager.EncryptData(model.Password);
                adminUserDetail.Email = model.Email.Trim().ToLower();
                adminUserDetail.RoleID = 1;
                adminUserDetail.DepartmentID = 1;
                adminUserDetail.IsActive = 1;
                adminUserDetail.IsDelete = false;
                adminUserDetail.DesignationID = 4;
                status = _IUserService.AddUserDetails(adminUserDetail);
            }

            if (status)
            {
                var role = new Master_Role();
                role.IsActive = true;
                role.RoleName = "ADMIN";
                role.UserName = model.Email;
                role.CreatedDate = DateTime.UtcNow;
                var roleStatus = _IRoleService.Insert(role, null, "");
                if (roleStatus)
                {
                    return Json(new { result = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, Message = "Fail" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, Message = "Fail" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}