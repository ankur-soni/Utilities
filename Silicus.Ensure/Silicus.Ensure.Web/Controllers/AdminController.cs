using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Mappings;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Silicus.Ensure.Models.Constants;
using System.Data.Entity;
using System.Net.Mail;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;
using Silicus.FrameWorx.Logger;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Web.Models.Test;
using Silicus.Ensure.Web.Filters;
using System.Globalization;
using Microsoft.AspNet.Identity;
using Silicus.Ensure.Web.Models.Employee;
using System.Web.Configuration;

namespace Silicus.Ensure.Web.Controllers
{
    [CustomAuthorize("Admin","Panel", "Recruiter")]
    public class AdminController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IQuestionService _questionService;
        private readonly ILogger _logger;
        private ApplicationUserManager _userManager;
        private readonly ITagsService _tagsService;
        private readonly IMappingService _mappingService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly IUserService _userService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IRoleService _roleService;
        //private readonly IPositionService _positionService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;

        private readonly CommonController _commonController;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public AdminController(IEmailService emailService, ITagsService tagService, ITestSuiteService testSuiteService, MappingService mappingService, IQuestionService questionService, IUserService userService, ILogger logger, Silicus.UtilityContainer.Services.Interfaces.IUserService containerUserService, CommonController commonController, Silicus.UtilityContainer.Services.Interfaces.IRoleService roleService)
        {
            _emailService = emailService;
            _tagsService = tagService;
            _testSuiteService = testSuiteService;
            _mappingService = mappingService;
            _questionService = questionService;
            _userService = userService;
            //_positionService = positionService;
            _roleService = roleService;
            _logger = logger;
            _containerUserService = containerUserService;
            _commonController = commonController;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> SendEmail(FormCollection email)
        {
            string retVal = "failed";
            if (!string.IsNullOrEmpty(email[1]))
            {
                var userDetails = await UserManager.FindByEmailAsync(email[1]);
                var code = await UserManager.GeneratePasswordResetTokenAsync(userDetails.Id);
                if (SendWelcomeMail(userDetails.Email, email[1], code))
                {
                    retVal = "succeeded";
                }
            }

            return Json(retVal);
        }

        private bool SendWelcomeMail(string email, string userFirstName, object key)
        {
            bool retVal = false;
            try
            {
                string urlEncodedUserName = System.Web.HttpUtility.UrlEncode(email); // url encoded
                string subject = ConfigurationManager.AppSettings["ProductNameLong"] + ": " +
                                 ConfigurationManager.AppSettings["SmtpMailSubjectWelcome"];
                string baseUrl = ConfigurationManager.AppSettings["SmtpMailbaseUrl"];

                string link = baseUrl + "/Account/ResetPassword/?username=" + urlEncodedUserName +
                              "&reset=" + GenerateEncodedKey(email, key.ToString());

                string body =
                    "<html>" +
                    "<body>" +
                    "<table style='width: 630px; border: none;'><tr><td><table style='border: 1px solid #5C666F; align: left; width: 630px; font-family: arial; font-size: 14px; height: auto;border-spacing: 0;'>" +
                    "<tr style='width: 630px; height: 44px; border-bottom: 1px solid #5C666F;'>" +
                    "<td style=' background-color: #00263D; height: 44px; width: 195px; border-bottom: 5px solid #55A51C; margin: 0 auto;'><img src='" +
                    ConfigurationManager.AppSettings["WebsiteURL"] +
                    "/Images/rigdig_logo_email.png' style='padding: 3px; margin: 0 auto;' /></td>" +
                    "<td style='background-color: #5C666F; width: 435px; height: 44px; border-bottom: 5px solid #A3A9AC;vertical-align: middle;'>" +
                    "<p style='font-size: 19px; margin-left: 20px; color: #fff; font-weight: bold; padding: 0; width: 100%;'>" +
                    "Welcome to " + ConfigurationManager.AppSettings["ProductNameLong"] + "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td colspan='2' style='width: 630px;width: auto; border: 4px solid #D1D3D4; border-top: none; padding: 30px; margin-top: 4px;'>" +
                    "<p style='font-size: 14px; color: #000; margin-top: 20px!important;'>" +
                    "Dear " + userFirstName + "," +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000; margin-bottom: 30px;'>" +
                    "Welcome to " + ConfigurationManager.AppSettings["ProductNameLong"] +
                    "! Your account contains a wealth of information to help guide your strategic planning, target new prospects, retain customers and better understand your market." +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000; margin-bottom: 30px;'>" +
                    "<span style='color: #55A51C; font-weight: bold;'>" +
                    "Access " + ConfigurationManager.AppSettings["ProductNameLong"] + "</span>" +
                    "<br />" +
                    "You can begin accessing " + ConfigurationManager.AppSettings["ProductNameShort"] +
                    " with the following login credentials:" +
                    "</p>" +
                    "<ul>" +
                    "<li style='list-style-type:disc; width: 470px; text-decoration: none !important; font-family: arial; font-size: 14px; color: #000; '>" +
                    "Username: <span style='color: #00698E; text-decoration: none !important;'>" + email +
                    "</span></li>" +
                    "<li style='list-style-type:disc; width: 470px;'>" +
                    "Click to set your Password: " +
                    "<a href='" + link +
                    "' target='_blank' style='display: inline; width: 450px; -ms-word-wrap:break-word; word-wrap:break-word; color: #00698E; text-decoration: underline;'>" +
                    ConfigurationManager.AppSettings["ProductNameShort"] + "_PasswordSetup" +
                    "</a>" +
                    "</li>" +
                    "</ul>" +
                    "<p style='font-size: 14px; color: #000; margin-top: 30px;'>" +
                    "<span style='color: #55A51C; font-weight: bold;'>" +
                    ConfigurationManager.AppSettings["ProductNameShort"] + " Client Success Team" +
                    "</span>" +
                    "<br />" +
                    "We look forward to helping you put the power of " +
                    ConfigurationManager.AppSettings["ProductNameShort"] +
                    " Business Intelligence to work for your organization." +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000;'>" +
                    "Please don’t hesitate to contact us with any questions." +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000;'>" +
                    "Kind regards," +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000; margin-bottom: 30px; margin-bottom: 10px !important;'>" +
                    "<span style='font-weight: bold;'>" +
                    ConfigurationManager.AppSettings["ProductNameLong"] + " Client Success Team" +
                    "</span>" +
                    "<br /><span style='color: #000; text-decoration: none!important; font-size: 14px;'>" +
                    ConfigurationManager.AppSettings["SmtpMailSupportAddress"] + "" +
                    "</span><br />" +
                    ConfigurationManager.AppSettings["ContactPhone_DotFormat"] +
                    "</p>" +
                    "<p style='text-align:right;margin-bottom: -15px; margin-right: -10px'><img src='" +
                    ConfigurationManager.AppSettings["WebsiteURL"] +
                    "/Images/RandallReillyProduct.png' style='padding: 2px;' /></p></td>" +
                    "</tr></table>" +
                    "<table style='border: none; width: 630px; border-spacing: 0; align: left; margin: 0; padding: 0;'><tr><td colspan='2' style='border-top: 5px solid #00263D;'>" +
                    "<p style='color: #000; padding: 10px; margin-top: 10px; font-family: arial; font-size: 12px;'>" +
                    "NEED HELP?: Training and reference materials are available at " +
                    "<a href='http://www.silicus.com/' target='_blank' style='color: #00698E; text-decoration: underline;'>" +
                    "http://www.silicus.com/" +
                    "</a>" +
                    "<br />" +
                    "CONTACT US: We welcome your questions and comments. You can reach us at " +
                    "<a href='mailto:" + ConfigurationManager.AppSettings["SmtpMailSupportAddress"] +
                    "' target='_blank' style='color: #00698E; text-decoration: underline;'>" +
                    ConfigurationManager.AppSettings["SmtpMailSupportAddress"] + "" +
                    "</a>" +
                    "</p>" +
                    "</td></tr></table></td></tr></table>" +
                    "</body>" +
                    "</html>";

                _emailService.SendEmailAsync(email, subject, body);
                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
                System.Diagnostics.Trace.WriteLine(ex);
            }
            return retVal;
        }

        private static string GenerateEncodedKey(string username, string guid)
        {
            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + "new" + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string hashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
            return hashParams;
        }

        #region Candidate
        //[CustomAuthorize("Admin", "Panel", "Recruiter")]
        //public ActionResult Candidates()
        //{

        //    List<string> roles = MvcApplication.getCurrentUserRoles();
        //    // var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
        //    //ViewBag.PositionListItem = from item in positionDetails
        //    //                           select new SelectListItem()
        //    //                           {
        //    //                               Text = item.PositionName,
        //    //                               Value = item.PositionName

        //    //                           };
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult GetCandidateGrid(string firstName, String lastName, string dobString)
        //{
        //    DateTime dob;
        //    DateTime.TryParseExact(dobString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
        //    // var candidates = _userService.GetCandidates(firstName, lastName, dob).ToList();
        //    //var candidatebusinessModelList = _mappingService.Map<List<UserBusinessModel>, List<UserViewModel>>(candidates);
        //    return PartialView("_CadidateGrid", null);
        //}

        //public ActionResult GetCandidateProfile(int userId)
        //{
        //    var candidate = _userService.GetUserById(userId);
        //    var candidatebusinessModel = _mappingService.Map<UserBusinessModel, UserViewModel>(candidate);
        //    //var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
        //    //candidatebusinessModel.PositionList = positionDetails.ToList();
        //    return PartialView("_CandidateProfile", candidatebusinessModel);

        //}



        //public ActionResult CandidateHistory(int userId)
        //{
        //    List<CandidateHistoryViewModel> objUserApplicationDetails = new List<CandidateHistoryViewModel>();

        //    var candidateApplicationDetails = _userService.GetUserDetails(userId);
        //    foreach (var candidateApplication in candidateApplicationDetails)
        //    {
        //        TestSuiteViewModel testSuiteViewModel = null;
        //        var candidatebusinessModel = _mappingService.Map<UserBusinessModel, CandidateHistoryViewModel>(candidateApplication);
        //       // var positionDetails = _positionService.GetPositionDetails().OrderBy(m => m.PositionName);
        //       // candidatebusinessModel.PositionList = positionDetails.ToList();
        //        var userTestSuiteDetails = _userService.GetTestSuiteDetailsOfUser(candidateApplication.UserApplicationId);
        //        if (userTestSuiteDetails != null)
        //        {
        //            testSuiteViewModel = new TestSuiteViewModel();
        //            TestSuite testSuitDetails = _testSuiteService.GetTestSuitById(userTestSuiteDetails.GetType().GetProperty("TestSuiteId").GetValue(userTestSuiteDetails, null));
        //            testSuiteViewModel = _mappingService.Map<TestSuite, TestSuiteViewModel>(testSuitDetails);
        //            testSuiteViewModel.OverallProficiency = ((Proficiency)Convert.ToInt32(testSuiteViewModel.Competency)).ToString();
        //            //if (testSuiteViewModel.Position.HasValue)
        //            //{
        //            //   // var position = _positionService.GetPositionById((int)testSuiteViewModel.Position);
        //            //    //if (position != null)
        //            //       // testSuiteViewModel.PositionName = position.PositionName;
        //            //}

        //            List<TestSuiteTagViewModel> testSuiteTags;
        //            GetTestSuiteTags(testSuitDetails, out testSuiteTags);
        //            testSuiteViewModel.Tags = testSuiteTags;
        //            testSuiteViewModel.Userid = userId;
        //        }
        //        candidateApplication.Technology = GetSkills(candidateApplication.Technology);
        //        candidatebusinessModel.Technology = candidateApplication.Technology;
        //        candidatebusinessModel.TestSuiteViewModel = testSuiteViewModel;
        //        objUserApplicationDetails.Add(candidatebusinessModel);

        //    }



        //    return View(objUserApplicationDetails);

        //}

        //private string GetSkills(string technology)
        //{
        //    var skills = "";
        //    if (!string.IsNullOrWhiteSpace(technology))
        //    {
        //        var skillIdsAsStrings = technology.Split(',');
        //        var skillIds = new List<int>();
        //        foreach (var skill in skillIdsAsStrings)
        //        {
        //            int skillId;
        //            if (int.TryParse(skill, out skillId))
        //            {
        //                skillIds.Add(skillId);
        //            }
        //        }
        //        var skilllNames = _tagsService.GetTagNames(skillIds);
        //        skills = string.Join(", ", skilllNames);
        //    }
        //    return skills;
        //}

        //public ActionResult CandidatesSuit(int UserId, int IsReassign = 0)
        //{
        //    ViewBag.CurrentUser = UserId;
        //    ViewBag.IsReassign = IsReassign;
        //    return PartialView("SelectCandidatesSuit");
        //}

        //public ActionResult GetPanelDetails([DataSourceRequest] DataSourceRequest request, int UserId)
        //{
        //    //try
        //    //{
        //    //    bool isAssignedPanel = false;
        //    //    var user = _userService.GetUserById(UserId);
        //    //    var panelList = new List<PanelViewModel>();

        //    //    if (user != null)
        //    //    {
        //    //        //var panelUserlist = _positionService.GetAllPanelMemberDetails();

        //    //        foreach (var item in panelUserlist)
        //    //        {
        //    //            isAssignedPanel = false;
        //    //            if (item.UserId == Convert.ToInt32(user.PanelId))
        //    //            {
        //    //                isAssignedPanel = true;
        //    //            }
        //    //            panelList.Add(new PanelViewModel()
        //    //            {
        //    //                PanelId = item.UserId,
        //    //                PanelName = item.FirstName + " " + item.LastName,
        //    //                Designation = item.Designation,
        //    //                Department = item.Department,
        //    //                IsAssignedPanel = isAssignedPanel
        //    //            });
        //    //        }
        //    //        panelList.OrderBy(x => x.IsAssignedPanel == true);
        //    //    }

        //    //    DataSourceResult result = panelList.ToDataSourceResult(request);
        //    //    return Json(result);
        //    //}
        //    //catch
        //    //{
        //    //    return Json(-1);
        //    //}
        //}

        //public ActionResult AssignPanel(int UserId, int IsReassign = 0)
        //{
        //    ViewBag.CurrentUser = UserId;
        //    ViewBag.IsReassign = IsReassign;
        //    return PartialView("_partialSelctPanelList");
        //}

        //public ActionResult AssignPanelCandidate(string PUserId, int UserId, int IsReAssign = 0)
        //{
        //    try
        //    {
        //        var user = _positionService.GetAllPanelMemberDetails().FirstOrDefault(x => x.UserId == Convert.ToInt32(PUserId));
        //        if (user != null)
        //        {
        //            var updateUser = _userService.GetUserById(UserId);
        //            updateUser.PanelId = Convert.ToString(user.UserId);
        //            updateUser.PanelName = user.FirstName + " " + user.LastName;
        //            _userService.Update(updateUser);
        //            List<string> Receipient = new List<string>() { "Admin", "Panel" };
        //            _commonController.SendMailByRoleName("Panel Assigned For " + updateUser.FirstName + " " + updateUser.LastName + " Successfully", "CandidatePanelAssigned.cshtml", Receipient, updateUser.FirstName + " " + updateUser.LastName);

        //        }

        //        return Json(1);
        //    }
        //    catch
        //    {
        //        return Json(-1);
        //    }
        //}

        //public ActionResult AssignRecruiter(int UserId, int IsReassign = 0)
        //{
        //    ViewBag.CurrentUser = UserId;
        //    ViewBag.IsReassign = IsReassign;
        //    return PartialView("_partialSelectRecruiterList");
        //}

        //public ActionResult GetRecruiterDetails([DataSourceRequest] DataSourceRequest request, int UserId)
        //{
        //    try
        //    {
        //        bool isAssignedRecruiter = false;
        //        var user = _userService.GetUserById(UserId);
        //        var recruiterList = new List<RecruiterViewModel>();

        //        if (user != null)
        //        {
        //            var recruiterUserlist = _positionService.GetAllRecruiterMemberDetails();

        //            foreach (var item in recruiterUserlist)
        //            {
        //                isAssignedRecruiter = false;
        //                if (item.UserId == Convert.ToInt32(user.RecruiterId))
        //                {
        //                    isAssignedRecruiter = true;
        //                }
        //                recruiterList.Add(new RecruiterViewModel()
        //                {
        //                    RecruiterId = item.UserId,
        //                    RecruiterName = item.FirstName + " " + item.LastName,
        //                    IsAssignedRecruiter = isAssignedRecruiter
        //                });
        //            }

        //            recruiterList.OrderBy(x => x.IsAssignedRecruiter == true);
        //        }

        //        DataSourceResult result = recruiterList.ToDataSourceResult(request);
        //        return Json(result);
        //    }
        //    catch
        //    {
        //        return Json(-1);
        //    }
        //}


        //public ActionResult AssignRecruiterCandidate(string RecruiterUserId, int UserId, int IsReAssign = 0)
        //{
        //    try
        //    {
        //        var user = _positionService.GetAllRecruiterMemberDetails().FirstOrDefault(x => x.UserId == Convert.ToInt32(RecruiterUserId));
        //        if (user != null)
        //        {
        //            var updateUser = _userService.GetUserById(UserId);
        //            updateUser.RecruiterId = Convert.ToString(user.UserId);
        //            updateUser.RecruiterName = user.FirstName + " " + user.LastName;
        //            _userService.Update(updateUser);
        //            List<string> Receipient = new List<string>() { "Admin", "Recruiter" };
        //            _commonController.SendMailByRoleName("Recruiter Assigned For " + updateUser.FirstName + " " + updateUser.LastName + " Successfully", "CandidateRecruiterAssigned.cshtml", Receipient, updateUser.FirstName + " " + updateUser.LastName, null, user.FirstName + " " + user.LastName);

        //        }

        //        return Json(1);
        //    }
        //    catch
        //    {
        //        return Json(-1);
        //    }
        //}

        //public ActionResult PartialTestSuitView(int testSuiteId, int userId)
        //{
        //    try
        //    {
        //        TestSuite testSuitDetails = _testSuiteService.GetTestSuitById(testSuiteId);
        //        TestSuiteViewModel testSuiteViewModel = _mappingService.Map<TestSuite, TestSuiteViewModel>(testSuitDetails);
        //        testSuiteViewModel.OverallProficiency = ((Proficiency)Convert.ToInt32(testSuiteViewModel.Competency)).ToString();
        //        if(testSuiteViewModel.Position.HasValue)
        //        {
        //            var position = _positionService.GetPositionById((int)testSuiteViewModel.Position);
        //            if (position != null)
        //                testSuiteViewModel.PositionName = position.PositionName;
        //        }

        //        List<TestSuiteTagViewModel> testSuiteTags;
        //        GetTestSuiteTags(testSuitDetails, out testSuiteTags);
        //        testSuiteViewModel.Tags = testSuiteTags;
        //        testSuiteViewModel.Userid = userId;
        //        return PartialView(testSuiteViewModel);
        //    }
        //    catch
        //    {
        //        return PartialView();
        //    }
        //}

        //private void GetTestSuiteTags(TestSuite testSuite, out List<TestSuiteTagViewModel> testSuiteTags)
        //{
        //    TestSuiteTagViewModel testSuiteTagViewModel;
        //    testSuiteTags = new List<TestSuiteTagViewModel>();
        //    var tagList = _tagsService.GetTagsDetails();
        //    string[] tags = testSuite.PrimaryTags.Split(',');
        //    string[] weights = testSuite.Weights.Split(',');
        //    string[] proficiency = testSuite.Proficiency.Split(',');

        //    for (int i = 0; i < tags.Length; i++)
        //    {
        //        testSuiteTagViewModel = new TestSuiteTagViewModel();
        //        testSuiteTagViewModel.TagId = Convert.ToInt32(tags[i]);
        //        testSuiteTagViewModel.TagName = tagList.Where(x => x.TagId == testSuiteTagViewModel.TagId).Select(x => x.TagName).SingleOrDefault();
        //        testSuiteTagViewModel.Weightage = Convert.ToInt32(weights[i]);
        //        testSuiteTagViewModel.Proficiency = Convert.ToInt32(proficiency[i]);
        //        testSuiteTagViewModel.Minutes = testSuite.Duration * Convert.ToInt32(weights[i]) / 100;
        //        testSuiteTags.Add(testSuiteTagViewModel);
        //    }
        //}
        //public ActionResult AssignSuite(int SuiteId, int UserId, int IsReAssign = 0)
        //{
        //    string mailsubject = "";
        //   // var updateCurrentUsers = _userService.GetUserById(UserId);
        //    if (updateCurrentUsers != null)
        //    {
        //        if (SuiteId > 0 && UserId > 0)
        //        {
        //            if (IsReAssign == 1)
        //            {
        //                var userTest = _testSuiteService.GetUserTestSuite().Where(x => x.UserApplicationId == updateCurrentUsers.UserApplicationId && x.StatusId == Convert.ToInt32(CandidateStatus.TestAssigned)).ToList();
        //                if (userTest.Any())
        //                {
        //                    foreach (var utest in userTest)
        //                    {

        //                        _testSuiteService.DeleteUserTestSuite(utest);
        //                    }
        //                }
        //            }
        //            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == SuiteId && model.IsDeleted == false).SingleOrDefault();
        //            UserTestSuite userTestSuite = new UserTestSuite();
        //            userTestSuite.UserApplicationId = updateCurrentUsers.UserApplicationId;
        //            userTestSuite.TestSuiteId = SuiteId;
        //            _testSuiteService.AssignSuite(userTestSuite, testSuiteDetails);
        //            //var selectUser = _userService.GetUserDetails().Where(model => model.UserId == UserId).FirstOrDefault();
        //            //selectUser.TestStatus = Convert.ToString(CandidateStatus.TestAssigned);
        //            //selectUser.CandidateStatus = Convert.ToString(CandidateStatus.TestAssigned);

        //            //_userService.Update(selectUser);

        //            //List<string> Receipient = new List<string>() { "Admin", "Panel" };

        //            //if (IsReAssign == 0)
        //            //{
        //            //    mailsubject = "Test Assigned For " + selectUser.FirstName + " " + selectUser.LastName + " Successfully";
        //            //    _commonController.SendMailByRoleName(mailsubject, "CandidateTestAssigned.cshtml", Receipient, selectUser.FirstName + " " + selectUser.LastName);
        //            //}
        //            //else
        //            //{
        //            //    mailsubject = "Test Re-Assigned For " + selectUser.FirstName + " " + selectUser.LastName + " Successfully";
        //            //    _commonController.SendMailByRoleName(mailsubject, "TestReassign.cshtml", Receipient, selectUser.FirstName + " " + selectUser.LastName);
        //            //}

        //            return Json(1);
        //        }
        //        else
        //        {
        //            return Json(-1);
        //        }
        //    }
        //    return View();
        //}

        //public ActionResult GetTestSuiteDetails([DataSourceRequest] DataSourceRequest request, int UserId)
        //{
        //    _testSuiteService.TestSuiteActivation();
        //    var tags = _tagsService.GetTagsDetails();
        //    var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.IsDeleted == false).OrderByDescending(model => model.TestSuiteId).ToArray();
        //    var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist);
        //    bool userInRole = MvcApplication.getCurrentUserRoles().Contains((Silicus.Ensure.Models.Constants.RoleName.Admin.ToString()));
        //    //var userApplicationId = _userService.GetUserLastestApplicationId(UserId);
        //    var testSuitId = _testSuiteService.GetUserTestSuiteByUserApplicationId(userApplicationId);
        //    foreach (var item in viewModels)
        //    {
        //        if (testSuitId != null)
        //        {
        //            if (testSuitId.TestSuiteId != 0 && item.TestSuiteId == testSuitId.TestSuiteId)
        //            {
        //                item.IsAssigned = true;
        //            }
        //        }

        //        //if(item.Position.HasValue)
        //        //{
        //        //    item.PositionName = GetPosition((int)item.Position) == null ? "deleted from master" : GetPosition((int)item.Position).PositionName;

        //        //}
        //        //else
        //        //{
        //        //    item.PositionName = "Not assigned";
        //        //}
        //        List<Int32> TagId = item.PrimaryTags.Split(',').Select(int.Parse).ToList();
        //        item.PrimaryTagNames = string.Join(",", (from a in tags
        //                                                 where TagId.Contains(a.TagId)
        //                                                 select a.TagName));
        //        item.StatusName = ((TestSuiteStatus)item.Status).ToString();
        //        item.UserInRole = userInRole;
        //    }
        //    viewModels = viewModels.Where(x => x.StatusName == TestSuiteStatus.Ready.ToString()).ToArray();
        //    return Json(viewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        #region Remove Later

        //public ActionResult AssignEmployeeSuite(int SuiteId, int UserId, int IsReAssign = 0)
        //{
        //    string mailsubject = "";
        //    var updateCurrentUsers = _userService.GetUserById(UserId);
        //    if (updateCurrentUsers != null)
        //    {
        //        if (SuiteId > 0 && UserId > 0)
        //        {
        //            if (IsReAssign == 1)
        //            {
        //                var userTest = _testSuiteService.GetUserTestSuite().Where(x => x.UserApplicationId == updateCurrentUsers.UserApplicationId && x.StatusId == Convert.ToInt32(CandidateStatus.TestAssigned)).ToList();
        //                if (userTest.Any())
        //                {
        //                    foreach (var utest in userTest)
        //                    {

        //                        _testSuiteService.DeleteUserTestSuite(utest);
        //                    }
        //                }
        //            }
        //            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == SuiteId && model.IsDeleted == false).SingleOrDefault();
        //            EmployeeTestSuite userTestSuite = new EmployeeTestSuite();
        //            userTestSuite.EmployeeId = UserId;
        //            userTestSuite.TestSuiteId = SuiteId;
        //            _testSuiteService.AssignEmployeeSuite(userTestSuite, testSuiteDetails);
        //            var selectUser = _userService.GetUserDetails().Where(model => model.UserId == UserId).FirstOrDefault();
        //            selectUser.TestStatus = Convert.ToString(CandidateStatus.TestAssigned);
        //            selectUser.CandidateStatus = Convert.ToString(CandidateStatus.TestAssigned);

        //            _userService.Update(selectUser);

        //            List<string> Receipient = new List<string>() { "Admin", "Panel" };

        //            if (IsReAssign == 0)
        //            {
        //                mailsubject = "Test Assigned For " + selectUser.FirstName + " " + selectUser.LastName + " Successfully";
        //                _commonController.SendMailByRoleName(mailsubject, "CandidateTestAssigned.cshtml", Receipient, selectUser.FirstName + " " + selectUser.LastName);
        //            }
        //            else
        //            {
        //                mailsubject = "Test Re-Assigned For " + selectUser.FirstName + " " + selectUser.LastName + " Successfully";
        //                _commonController.SendMailByRoleName(mailsubject, "TestReassign.cshtml", Receipient, selectUser.FirstName + " " + selectUser.LastName);
        //            }

        //            return Json(1);
        //        }
        //        else
        //        {
        //            return Json(-1);
        //        }
        //    }
        //    return View();
        //}

        //public ActionResult GetEmployeeTestSuiteDetails([DataSourceRequest] DataSourceRequest request, int UserId)
        //{
        //    _testSuiteService.TestSuiteActivation();
        //    var tags = _tagsService.GetTagsDetails();
        //    var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.IsDeleted == false && model.IsExternal == false).OrderByDescending(model => model.TestSuiteId).ToArray();
        //    var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist);
        //    bool userInRole = MvcApplication.getCurrentUserRoles().Contains((Silicus.Ensure.Models.Constants.RoleName.Admin.ToString()));
        //    //var userApplicationId = _userService.GetUserLastestApplicationId(UserId);
        //    var testSuitId = _testSuiteService.GetEmployeeTestSuiteByEmployeeId(UserId);
        //    foreach (var item in viewModels)
        //    {
        //        if (testSuitId != null)
        //        {
        //            if (testSuitId.TestSuiteId != 0 && item.TestSuiteId == testSuitId.TestSuiteId)
        //            {
        //                item.IsAssigned = true;
        //            }
        //        }

        //        item.PositionName = GetPosition((int)item.Position) == null ? "deleted from master" : GetPosition((int)item.Position).PositionName;
        //        List<Int32> TagId = item.PrimaryTags.Split(',').Select(int.Parse).ToList();
        //        item.PrimaryTagNames = string.Join(",", (from a in tags
        //                                                 where TagId.Contains(a.TagId)
        //                                                 select a.TagName));
        //        item.StatusName = ((TestSuiteStatus)item.Status).ToString();
        //        item.UserInRole = userInRole;
        //    }
        //    //viewModels = viewModels.Where(x => x.StatusName == TestSuiteStatus.Ready.ToString()).ToArray();
        //    return Json(viewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        #endregion

        //private Position GetPosition(int positionId)
        //{
        //    return _positionService.GetPositionById(positionId);
        //}
        //public ActionResult CandidateAdd(int UserId, bool IsCandidateReappear = false)
        //{
        //    UserViewModel currUser = new UserViewModel();
        //    currUser.UserId = UserId;

        //    if (UserId != 0)
        //    {
        //        var user = _userService.GetUserById(UserId);
        //        currUser = _mappingService.Map<UserBusinessModel, UserViewModel>(user);
        //    }
        //    else if (TempData["UserViewModel"] != null)
        //    {
        //        currUser = TempData["UserViewModel"] as UserViewModel;
        //    }

        //    //var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
        //   // currUser.PositionList = positionDetails.ToList();
        //    currUser.IsCandidateReappear = IsCandidateReappear;
        //    if (!string.IsNullOrWhiteSpace(currUser.DOB))
        //    {
        //        DateTime dt = DateTime.Parse(currUser.DOB);
        //        currUser.DOB = dt.ToString("dd/MM/yyyy");
        //    }




        //    return View(currUser);
        //}

        #endregion

        //public ActionResult ViewQuestion(int testSuiteId, int userId)
        //{
        //    var viewerEmailId = User.Identity.Name;
        //    var viewer = _containerUserService.FindUserByEmail(viewerEmailId);
        //    var candidate = _userService.GetUserById(userId);
        //    int count = 0;
        //    var testSuiteViewQuesModel = new TestSuiteViewQuesModel();
        //    var testSuiteQuestionList = new List<TestSuiteQuestion>();
        //    try
        //    {
        //        TestSuite testSuitDetails = _testSuiteService.GetTestSuitById(testSuiteId);
        //        var previewTest = new PreviewTestBusinessModel { TestSuite = testSuitDetails, ViewerId = viewer.ID, CandidateId = candidate.UserApplicationId };
        //        if (testSuitDetails != null && testSuitDetails.Status == Convert.ToInt32(TestSuiteStatus.Ready))
        //        {
        //            var questionList = _testSuiteService.GetPreview(previewTest);
        //            foreach (var pQuestion in questionList)
        //            {
        //                count++;
        //                testSuiteQuestionList.Add(new TestSuiteQuestion()
        //                {
        //                    QuestionType = pQuestion.QuestionType,
        //                    QuestionNumber = count,
        //                    QuestionDescription = pQuestion.QuestionDescription,
        //                    OptionCount = pQuestion.OptionCount,
        //                    Answer = pQuestion.Answer,
        //                    CorrectAnswer = pQuestion.CorrectAnswer,
        //                    Id = pQuestion.Id,
        //                    Marks = pQuestion.Marks,
        //                    Option1 = pQuestion.Option1,
        //                    Option2 = pQuestion.Option2,
        //                    Option3 = pQuestion.Option3,
        //                    Option4 = pQuestion.Option4,
        //                    Option5 = pQuestion.Option5,
        //                    Option6 = pQuestion.Option6,
        //                    Option7 = pQuestion.Option7,
        //                    Option8 = pQuestion.Option8,
        //                });
        //            }

        //            testSuiteViewQuesModel.TestSuiteQuestion = testSuiteQuestionList;
        //            testSuiteViewQuesModel.TestSuiteName = testSuitDetails.TestSuiteName;
        //            testSuiteViewQuesModel.Duration = testSuitDetails.Duration;
        //            testSuiteViewQuesModel.ObjectiveCount = questionList.Count(x => x.QuestionType == 1);
        //            testSuiteViewQuesModel.PracticalCount = questionList.Count(x => x.QuestionType == 2);
        //            TestSuiteCandidateModel testSuiteCandidateModel = new TestSuiteCandidateModel
        //            {
        //                TestSuiteId = testSuiteId,
        //                UserId = userId,
        //                PracticalCount = testSuiteViewQuesModel.PracticalCount,
        //                ObjectiveCount = testSuiteViewQuesModel.ObjectiveCount,
        //                Duration = testSuiteViewQuesModel.Duration
        //            };
        //            var candidateInfoBusinessModel = _userService.GetCandidateInfo(candidate);
        //            testSuiteCandidateModel.CandidateInfo = _mappingService.Map<CandidateInfoBusinessModel, CandidateInfoViewModel>(candidateInfoBusinessModel);
        //            testSuiteCandidateModel.NavigationDetails = GetQuestionNavigationDetails(questionList);
        //            testSuiteCandidateModel.TotalQuestionCount = testSuiteCandidateModel.PracticalCount + testSuiteCandidateModel.ObjectiveCount;
        //            testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.Duration;
        //            testSuiteCandidateModel.ProfilePhotoFilePath = candidate.ProfilePhotoFilePath;
        //            return View("PreviewTest", testSuiteCandidateModel);
        //        }
        //        else
        //        {
        //            testSuiteViewQuesModel.ErrorMessage = "Test suite is not ready.";
        //        }

        //    }
        //    catch
        //    {
        //        testSuiteViewQuesModel.ErrorMessage = "Something went wrong! Please try later.";
        //    }

        //    return View(testSuiteViewQuesModel);
        //}

        private QuestionNavigationViewModel GetQuestionNavigationDetails(IEnumerable<Question> questions)
        {
            var navigation = new QuestionNavigationViewModel { Practical = new List<QuestionNavigationBasics>(), Objective = new List<QuestionNavigationBasics>() };

            if (questions != null && questions.Any())
            {
                questions = questions.OrderBy(ques => ques.Id).ToList();
                foreach (var question in questions)
                {
                    if (question.QuestionType == (int)QuestionType.Practical)
                    {
                        navigation.Practical.Add(new QuestionNavigationBasics { QuestionId = question.Id, QuestionDescription = question.QuestionDescription, IsViewedOnly = false });
                    }
                    else if (question.QuestionType == (int)QuestionType.Objective)
                    {
                        navigation.Objective.Add(new QuestionNavigationBasics { QuestionId = question.Id, QuestionDescription = question.QuestionDescription, IsViewedOnly = false });
                    }
                }
            }
            return navigation;
        }
        #region Create PDF

        public ActionResult CreatePDF(int id)
        {
            var filename = "";
            byte[] byteInfo = generatePDF(id, out filename);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
            Response.BinaryWrite(byteInfo);
            return new EmptyResult();
        }

        private Byte[] generatePDF(int id, out string filename)
        {
            int UserId = 0;
            int QuesId = 0;
            if (id != null)
            {
                UserId = Convert.ToInt32(id);
            }

            //var userDetails = _userService.GetUserDetails().Where(x => x.UserId == UserId).FirstOrDefault();
            var userTestSuit = _testSuiteService.GetEmployeeTestSuiteById(id);
            var userTestSuitDetails = userTestSuit.EmployeeTestDetails;

            //var userTestSuitDetails = _testSuiteService.GetUserTestSuite().Where(x => x.UserApplicationId == UserId).FirstOrDefault().UserTestDetails;

            string fname, lname;
            if (userTestSuit.EmployeeId > 0)
            {
                var user = _containerUserService.GetAllUsers().Where(u => u.ID == userTestSuit.EmployeeId).FirstOrDefault();
                fname = user != null ? user.FirstName : "";
                lname = user != null ? user.LastName : "";
            }
            else
            {
                var user = UserManager.FindById(userTestSuit.CandidateID);
                fname = user != null ? user.FirstName : "";
                lname = user != null ? user.LastName : "";
            }
            ViewBag.FNameLName = fname + lname;

            List<Question> Que = (from question in _questionService.GetQuestion().ToList()
                                  join userTest in userTestSuitDetails.ToList()
                                      on question.Id equals userTest.QuestionId
                                  select question).ToList();

            Que = Que.OrderBy(x => x.Id).ToList();

            StringBuilder sb = new StringBuilder();

            sb.Append("<h2>Question Set for " + "<strong>" + fname + " " + lname + "</strong></h2>");
            sb.Append("<br />");
            sb.Append("<div></div>");
            sb.Append("<strong>Date: </strong>" + DateTime.Now.ToString("dd-MM-yyyy"));
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<table border='5' cellpadding='0' cellspacing='10' width='100%'><tr><td>&nbsp;</td></tr></table>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div>");
            sb.Append("</div>");
            sb.Append("<br />");
            sb.Append("<table border='1' cellpadding='0' cellspacing='0' width='100%'><tr><td>&nbsp;</td></tr></table>");
            sb.Append("<h3><strong>Objective Question Set</strong></h3>");
            sb.Append("<br />");
            sb.Append("<table border='1' cellpadding='0' cellspacing='0' width='100%'><tr><td>&nbsp;</td></tr></table>");

            foreach (var q in Que)
            {
                if (q.QuestionType == 1)
                {
                    QuesId += 1;
                    sb.Append("  <div class='col-md-8' style='margin-top:30px'>");
                    sb.Append("  <strong>Question " + QuesId + ":</strong> &nbsp;&nbsp;");
                    sb.Append("" + q.QuestionDescription + "");
                    sb.Append(" </div>");
                    sb.Append(" </div>");
                    sb.Append("  <div class='row'>");
                    sb.Append("   <div class='col-md-8' style='margin-top:20px'>");
                    sb.Append("  <strong>Option 1:</strong> &nbsp;&nbsp;");
                    sb.Append("" + q.Option1 + "");
                    sb.Append(" </div>");
                    sb.Append(" </div>");
                    sb.Append(" <div class='row'>");
                    sb.Append("<div class='col-md-8' style='margin-top:20px'>");
                    sb.Append("  <strong>Option 2:</strong> &nbsp;&nbsp;");
                    sb.Append("" + q.Option2 + "");
                    sb.Append("  </div>");
                    sb.Append(" </div>");
                    sb.Append(" <div class='row'>");
                    sb.Append(" <div class='col-md-8' style='margin-top:20px'>");
                    sb.Append("  <strong>Option 3:</strong> &nbsp;&nbsp;");
                    sb.Append("" + q.Option3 + "");
                    sb.Append("  </div>");
                    sb.Append("  </div>");
                    sb.Append("   <div class='row'>");
                    sb.Append(" <div class='col-md-8' style='margin-top:20px'>");
                    sb.Append("  <strong>Option 4:</strong> &nbsp; &nbsp;");
                    sb.Append("" + q.Option4 + "");
                    sb.Append("  </div>");
                    sb.Append("  </div>");
                    sb.Append("  <div class='row'>");
                    sb.Append("  <div class='col-md-8' style='margin-top:20px'>");
                    sb.Append("   <strong>Correct Answer:</strong> &nbsp;&nbsp;");
                    sb.Append("" + q.CorrectAnswer + "");
                    sb.Append("<br />");
                    sb.Append("   </div>");
                    sb.Append(" </div>");

                }
            }

            sb.Append("<div></div><div></div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<table border='1' cellpadding='0' cellspacing='0' width='100%'><tr><td>&nbsp;</td></tr></table>");
            sb.Append("<h3><strong>Practical Question Set</strong></h3>");
            sb.Append("<br />");
            sb.Append("<table border='1' cellpadding='0' cellspacing='0' width='100%'><tr><td>&nbsp;</td></tr></table>");
            sb.Append("<br />");

            QuesId = 0;
            foreach (var q in Que)
            {
                if (q.QuestionType == 2)
                {
                    QuesId += 1;
                    sb.Append("<div class='row' style='page-break-before: always'>");
                    sb.Append("<div class='col-md-9' style='margin-top:30px'>");
                    sb.Append("<strong>Question " + QuesId + ":</strong>&nbsp;");
                    sb.Append("" + q.QuestionDescription + "");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class='row'>");
                    sb.Append("<div class='col-md-9' style='margin-top:30px'>");
                    sb.Append("<strong>Answer:</strong> &nbsp;&nbsp;&nbsp;&nbsp;");
                    sb.Append("" + HttpUtility.HtmlDecode(q.Answer) + "");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("</div>");
                    sb.Append("</div>");
                }
            }
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style='text-align:center'>***********************************************End***********************************************</div>");

            StringReader sr = new StringReader(sb.ToString());
            filename = fname + "_" + lname + "_" + userTestSuit.EmployeeTestSuiteId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            return memoryStream.ToArray();
        }

        #endregion

        [CustomAuthorize("Admin", "Panel", "Recruiter")]
        public ActionResult SubmittedTest(int EmployeeTestSuitId)
        {
            try
            {
                List<ObjectiveQuestionList> objectiveQuestionList = new List<ObjectiveQuestionList>();
                List<PracticalQuestionList> practicalQuestionList = new List<PracticalQuestionList>();

                //var userDetails = _userService.GetUserDetails().Where(x => x.UserId == UserId).FirstOrDefault();

                var userTestSuitDetails = _testSuiteService.GetEmployeeTestSuiteById(EmployeeTestSuitId);

                SubmittedTestViewModel submittedTestViewModel = new Models.SubmittedTestViewModel();
                var userList = _containerUserService.GetAllUsers();
                if (userTestSuitDetails.EmployeeId > 0)
                {
                    var user = userList.Where(u => u.ID == userTestSuitDetails.EmployeeId).FirstOrDefault();
                    submittedTestViewModel.FirstName = user != null ? user.FirstName : "";
                    submittedTestViewModel.LastName = user != null ? user.LastName : "";
                }
                else
                {
                    var user = UserManager.FindById(userTestSuitDetails.CandidateID); 
                    submittedTestViewModel.FirstName = user != null ? user.FirstName : "";
                    submittedTestViewModel.LastName = user != null ? user.LastName : "";
                }

                if (userTestSuitDetails == null)
                {
                    //TempData["ErrorMsg"] = userDetails == null ? "User id can not be null." : "Test suite is not assigned to user.";
                    return RedirectToAction("ReviewAssignedTest");
                }

                var testSuitDetails = _testSuiteService.GetTestSuitById(userTestSuitDetails.TestSuiteId);

                if (testSuitDetails == null)
                {
                    TempData["ErrorMsg"] = "Test suite is not assigned to user.";
                    return RedirectToAction("Candidates");
                }


                submittedTestViewModel.Status = userTestSuitDetails.StatusId.ToString();
                submittedTestViewModel.Duration = userTestSuitDetails.Duration;
                submittedTestViewModel.TotalMakrs = userTestSuitDetails.MaxScore;
                submittedTestViewModel.TestSuitName = testSuitDetails.TestSuiteName;
                submittedTestViewModel.UserId = userTestSuitDetails.CandidateID;
                submittedTestViewModel.TestSuiteId = EmployeeTestSuitId;
                submittedTestViewModel.ReviewDate = userTestSuitDetails.ReviewDate;
                if(userTestSuitDetails.ReviewerId != null && userTestSuitDetails.ReviewerId > 0)
                {
                   var reviwer = userList.Where(u => u.ID == userTestSuitDetails.ReviewerId).FirstOrDefault();
                    submittedTestViewModel.ReviewerName = reviwer != null ? reviwer.FirstName  + " " + reviwer.LastName : "";
                }
               
                //if(testSuitDetails.Position.HasValue)
                //{
                //    submittedTestViewModel.Postion = _positionService.GetPositionById((int)testSuitDetails.Position) != null ? _positionService.GetPositionById((int)testSuitDetails.Position).PositionName : "";
                //}

                foreach (var questionId in userTestSuitDetails.EmployeeTestDetails)
                {

                    var question = _questionService.GetSingleQuestion(questionId.QuestionId);
                    if (question.QuestionType == 1)
                    {
                        if (questionId.Mark != null && questionId.Mark > 0)
                        {
                            submittedTestViewModel.ObjectiveQuestionResult += question.Marks;
                        }
                        submittedTestViewModel.ObjectiveQuestionMarks += question.Marks;

                        objectiveQuestionList.Insert(0, new ObjectiveQuestionList()
                        {
                            QuestionDescription = question.QuestionDescription,
                            CorrectAnswer = GetOption(question.CorrectAnswer),
                            SubmittedAnswer = GetOption(questionId.Answer),
                            Result = questionId.Mark != null && questionId.Mark > 0 ? "Correct" : "Incorrect",
                        });
                    }
                    else
                    {
                        practicalQuestionList.Insert(0, new PracticalQuestionList()
                        {
                            QuestionId = questionId.QuestionId,
                            QuestionDescription = question.QuestionDescription,
                            SubmittedAnswer = questionId.Answer,
                            CorrectAnwer = question.Answer,
                            Weightage = question.Marks,
                            EvaluatedMark = questionId.Mark,
                        });

                    }

                }
                submittedTestViewModel.EvaluatedFeedBack = userTestSuitDetails.FeedBack;
                submittedTestViewModel.TotalMarksObtained = submittedTestViewModel.ObjectiveQuestionResult;
                submittedTestViewModel.objectiveQuestionList = objectiveQuestionList;
                submittedTestViewModel.practicalQuestionList = practicalQuestionList;
                //submittedTestViewModel.Status = userTestSuitDetails.StatusId;
                //if (userDetails.CandidateStatus == CandidateStatus.TestSubmitted.ToString())
                //{
                //    userDetails.CandidateStatus = CandidateStatus.UnderEvaluation.ToString();
                //    _userService.Update(userDetails);
                //}

                List<string> Receipient = new List<string>() { "Admin", "Panel" };
                //  _commonController.SendMailByRoleName("Online Test Submitted For " + userDetails.FirstName + " " + userDetails.LastName + "", "CandidateTestSubmitted.cshtml", Receipient, userDetails.FirstName + " " + userDetails.LastName);

                return View(submittedTestViewModel);

            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("ReviewAssignedTest");
            }
        }

        [CustomAuthorize("Admin", "Panel", "Recruiter")]
        public ActionResult ReviewAssignedTest(int ReviewType)
        {
            if (ReviewType > 0 && ReviewType < 3)
                Session["ReviewType"] = ReviewType;

            return View("ReviewAssigedTest", ReviewType);
        }

        public ActionResult GetTestSuitsforEvaluation([DataSourceRequest] DataSourceRequest request)
        {
            var userlist = _containerUserService.GetAllUsers();
            var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.IsDeleted == false && model.IsExternal == false);
            var TestSuits = _testSuiteService.GetEmployeeTestSuite().Where(ts => ts.StatusId >= (int)CandidateStatus.TestSubmitted);

            var ReviewType = (int)Session["ReviewType"];
            if (ReviewType ==1)
            {
                TestSuits = TestSuits.Where(t => t.EmployeeId > 0);
            }
            if (ReviewType == 2)
            {
                TestSuits = TestSuits.Where(t => t.EmployeeId == 0);
            }
            var userEmailId = User.Identity.Name;
            var Currentuser = _containerUserService.FindUserByEmail(userEmailId);

            //var utilityId = GetUtilityId();
            //var roleDetails = _roleService.GetRoleByRoleName("Admin");
            //var AdminUserList = _containerUserService.GetAllUsersByRoleInUtility(utilityId, roleDetails.ID).OrderByDescending(model => model.DisplayName).ToList();

            List<string> roles = MvcApplication.getCurrentUserRoles();
            if (! roles.Any(r=>r == "Admin"))
                TestSuits = TestSuits.Where(t => t.ReviewerId.Value.ToString() == User.Identity.GetUserId()).ToList();

            var TestResults = new List<EmployeeTestResultViewModel>();

            foreach (var tests in TestSuits)
            {
                var currentTs = new EmployeeTestResultViewModel();

                currentTs.MaxScore = tests.MaxScore;
                currentTs.MaxScore = tests.MaxScore;
                currentTs.MarksObtained = tests.EmployeeTestDetails.Select(x => x.Mark).Sum();
                currentTs.TestSuitId = tests.TestSuiteId;
                currentTs.EmployeeTestSuiteId = tests.EmployeeTestSuiteId;

                var testDetail = testSuitelist.Where(ts => ts.TestSuiteId == tests.TestSuiteId).FirstOrDefault();
                if (testDetail != null)
                    currentTs.TestSuitName = testDetail.TestSuiteName;

                currentTs.AttemptDate = tests.AttemptDate;
                currentTs.StatusId = tests.StatusId;
                currentTs.ReviewDate = tests.ReviewDate;
                var Reviweruser = userlist.Where(u => u.ID == tests.ReviewerId).FirstOrDefault();
                if (Reviweruser != null)
                    currentTs.ReviewerName = Reviweruser.FirstName + " " + Reviweruser.LastName;
                 

                if (tests.EmployeeId > 0)
                {
                    var user = userlist.Where(u => u.ID == tests.EmployeeId).FirstOrDefault();
                    if (user != null)
                        currentTs.EmpName = user.FirstName + " " + user.LastName;
                }
                else
                {
                    var user = UserManager.FindById(tests.CandidateID);
                    if (user != null)
                        currentTs.EmpName = user.FirstName + " " + user.LastName;
                }
                TestResults.Add(currentTs);

            }

            DataSourceResult result = TestResults.ToDataSourceResult(request);
            return Json(result);

        }

        //private int GetUtilityId()
        //{
        //    var utilityProductId = WebConfigurationManager.AppSettings["ProductId"];
        //    if (string.IsNullOrWhiteSpace(utilityProductId))
        //    {
        //        throw new ArgumentNullException();
        //    }

        //    return Convert.ToInt32(utilityProductId);
        //}

        private string GetOption(string p)
        {
            string optionSelect = "Option:";
            switch (p)
            {
                default:
                    optionSelect += p;
                    break;
            }
            return p == null ? "" : optionSelect;
        }

        [HttpPost]
        [CustomAuthorize("Admin", "Panel", "Recruiter")]
        public ActionResult SubmittedTest(FormCollection fm)
        {
            try
            {
                int count = 1;
                var userTestSuitDetails = _testSuiteService.GetEmployeeTestSuiteById(Convert.ToInt32(Convert.ToString(Request.Form["TestSuiteId"])));
                //var userTestSuitDetails = _testSuiteService.GetUserTestSuiteByUdi_TestSuitId(Convert.ToInt32(Convert.ToString(Request.Form["UserId"])), Convert.ToInt32(Convert.ToString(Request.Form["TestSuiteId"])));

                userTestSuitDetails.EvaluatedMark = Convert.ToInt32(Request.Form["TotalMarksObtained"].ToString());
                userTestSuitDetails.FeedBack = Convert.ToString(Request.Form["EvaluatedFeedBack"]);
                userTestSuitDetails.StatusId = Convert.ToInt32(CandidateStatus.TestSubmitted);
                var userTestDetails = (from uDetails in userTestSuitDetails.EmployeeTestDetails
                                       join question in _questionService.GetQuestion().Where(X => X.QuestionType == 2)
                                       on uDetails.QuestionId equals question.Id
                                       select uDetails).ToList();

                foreach (var userTestDetail in userTestDetails)
                {
                    //var currentUser =  (User.Identity.Name);
                    userTestDetail.MarkGivenByName = User.Identity.Name;
                    //userTestDetail.MarkGivenBy = User.Identity.GetUserId();
                    userTestDetail.Mark = Convert.ToInt32(Request.Form["Emarks" + count]);
                    userTestDetail.MarkGivenDate = DateTime.Now;

                    _testSuiteService.UpdateEmployeeTestDetails(userTestDetail);
                    count++;
                }

               // _testSuiteService.UpdateEmployeeTestSuite(userTestSuitDetails);
                //var user = _userService.GetUserById(Convert.ToInt32(Convert.ToString(Request.Form["UserId"])));
                //user.TestStatus = CandidateStatus.TestSubmitted.ToString();
                //user.CandidateStatus = Convert.ToString(Request.Form["Status"]);
                userTestSuitDetails.StatusId = Convert.ToInt32(Request.Form["Status"]);
                var userEmailId = User.Identity.Name;
                var user = _containerUserService.FindUserByEmail(userEmailId);
                userTestSuitDetails.ReviewerId = user.ID;
                userTestSuitDetails.ReviewDate = DateTime.Now;
                _testSuiteService.UpdateEmployeeTestSuite(userTestSuitDetails);
                //_userService.Update(user);
            }
            catch
            {
                throw;
            }
            var ReviewType = (int)Session["ReviewType"];
            return RedirectToAction("ReviewAssignedTest", new { ReviewType = ReviewType });
        }

        # region Mail Send
        //public ActionResult SendMail(int? id)
        //{
        //    int UserId = 0;
        //    if (id != null)
        //    {
        //        UserId = Convert.ToInt32(id);
        //    }
        //   // var userDetails = _userService.GetUserDetails().Where(x => x.UserId == UserId).FirstOrDefault();
        //    var userTestSuitDetails = _testSuiteService.GetUserTestSuite().Where(x => x.UserApplicationId == UserId).FirstOrDefault().UserTestDetails;

        //   // ViewBag.FNameLName = userDetails.FirstName + userDetails.LastName;

        //    List<Question> Que = (from question in _questionService.GetQuestion().ToList()
        //                          join userTest in userTestSuitDetails.ToList()
        //                              on question.Id equals userTest.QuestionId
        //                          select question).ToList();

        //    Que = Que.OrderBy(x => x.Id).ToList();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var body = "<p>Dear Admin,</p><p>The Online Test has been submitted for <strong>{0} {1}</strong> on " + DateTime.Now + ".</p> Please review, evatuate and add your valuable feedback of the Test in order to conduct first round of interview.<br /><p>Regards,</p><p>Ensure, IT Support</p><p>This is an auto-generated mail sent by Ensure. Please do not reply to this email.</p>";
        //            var message = new MailMessage();
        //            message.To.Add(new MailAddress("Nishant.Lohakare@silicus.com"));
        //            message.From = new MailAddress("nish89.cse@gmail.com", "Ensure Team");
        //            message.Subject = "Test Submitted for " + userDetails.FirstName + " " + userDetails.LastName;
        //            message.Body = string.Format(body, userDetails.FirstName, userDetails.LastName);

        //            string filename = "";
        //            byte[] byteInfo = generatePDF(id, out filename);
        //            MemoryStream ms = new MemoryStream(byteInfo);
        //            FileStream file = new FileStream(Server.MapPath("~\\Attachment") + "\\" + filename, FileMode.Create, FileAccess.Write);
        //            ms.WriteTo(file);
        //            file.Close();
        //            ms.Close();

        //            Attachment attachment = new Attachment(Server.MapPath("~\\Attachment") + "\\" + filename);
        //            message.Attachments.Add(attachment);
        //            message.IsBodyHtml = true;

        //            using (var smtp = new SmtpClient())
        //            {
        //                smtp.Send(message);
        //                TempData["Success"] = "Mail Sent Successfully";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
        //    }
        //    return RedirectToAction("ViewQuestion", "Admin", new { id = UserId });
        //}

        #endregion
            
        [HttpPost]
        public JsonResult SessionTimeout()
        {
            return Json("success");
        }

    }
}
