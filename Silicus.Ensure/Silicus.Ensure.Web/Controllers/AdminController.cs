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
using System.Collections.Generic;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IQuestionService _questionService;

        private ApplicationUserManager _userManager;
        private readonly ITagsService _tagsService;
        private readonly IMappingService _mappingService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly IUserService _userService;
        private readonly IPositionService _positionService;

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

        public AdminController(IEmailService emailService, ITagsService tagService, ITestSuiteService testSuiteService, MappingService mappingService, IQuestionService questionService, IUserService userService, IPositionService positionService)
        {
            _emailService = emailService;
            _tagsService = tagService;
            _testSuiteService = testSuiteService;
            _mappingService = mappingService;
            _questionService = questionService;
            _userService = userService;
            _positionService = positionService;
        }

        public ActionResult Dashboard()
        {
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return View();
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

        #region Tag
        public ActionResult GetTagsDetails([DataSourceRequest] DataSourceRequest request)
        {
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            return Json(tagDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagList()
        {
            return View();
        }

        public ActionResult TagAdd(Int32 tagId = 0)
        {
            return View("TagAdd");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTag(Tags tag)
        {
            var tagDetails = _tagsService.GetTagsDetails().Where(model => model.TagName == tag.TagName && model.TagId != tag.TagId);
            if (tagDetails.Count() > 0)
                ModelState.AddModelError(string.Empty, "The Tag already exists, please create with other name.");
            if (tag != null && ModelState.IsValid)
            {
                tag.IsActive = true;
                tag.Description = HttpUtility.HtmlDecode(tag.Description);
                _tagsService.Add(tag);
                TempData.Add("IsNewTag", 1);
                return RedirectToAction("TagList");
            }
            tag.Description = HttpUtility.HtmlDecode(tag.Description);
            return View("TagAdd", tag);
        }

        #endregion

        #region Candidate

        public ActionResult Candidates()
        {
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return View();
        }

        public ActionResult CandidatesSuit(int UserId)
        {
            ViewBag.CurrentUser = UserId;
            return PartialView("SelectCandidatesSuit");
        }

        public ActionResult AssignSuite(int SuiteId, int Userid)
        {

            DataSourceRequest DataSourceRequest = new Kendo.Mvc.UI.DataSourceRequest();
            DataSourceRequest.Page = 1;
            DataSourceRequest.PageSize = 10;

            var objectiveCount = new object();
            var updateCurrentUsers = _userService.GetUserDetails().Where(model => model.UserId == Userid).FirstOrDefault();
            if (updateCurrentUsers != null)
            {

                // return Json(1);

                if (SuiteId > 0 && Userid > 0)
                {
                    var ViewPrimaryTagList = _testSuiteService.GetTestSuiteDetails().Where(q => q.TestSuiteId == SuiteId).Select(p => p.PrimaryTags).ToList();

                    foreach (var tagid in ViewPrimaryTagList)
                    {
                        string[] values = tagid.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();
                            objectiveCount = _questionService.GetQuestion().Where(p => p.Tags.Contains(values[i]) && p.QuestionType == 1).ToList().Count();
                        }
                    }

                    UserTestSuite newusertestsuit = new UserTestSuite
                    {
                        UserId = Userid,
                        TestSuiteId = SuiteId,
                        ObjectiveCount = 5,
                        EvaluatedMark = 50,
                        MaxScore = 70,
                        CreatedDate = DateTime.Now,
                    };
                    foreach(var tagid in ViewPrimaryTagList)
                    {
                        string[] values = tagid.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();
                           var questionList = _questionService.GetQuestion().Where(p => p.Tags.Contains(values[i])).Select(q=>q.Id).ToList();
                           foreach (var questionId in questionList)
                           {
                               UserTestDetails userTestDetails = new UserTestDetails
                               {
                                   UserTestSuiteId = SuiteId,
                                   QuestionId = Convert.ToInt32(questionId),
                                   Mark = 30
                               };
                           }
                        }
                    }
                    

                    _testSuiteService.AddUserTestSuite(newusertestsuit);
                    updateCurrentUsers.TestStatus = "Assigned";
                    _userService.Update(updateCurrentUsers);
                    return Json(1);
                    // TempData.Add("IsNewTestSuite", 1);
                    // return RedirectToAction("GetCandidateDetails", "User", DataSourceRequest);
                }
                else
                {
                    //return RedirectToAction("GetCandidateDetails", "User", DataSourceRequest);
                    return Json(-1);
                }
            }
            return View();
        }

        public ActionResult CandidateAdd()
        {
            UserViewModel currUser = new UserViewModel();
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            currUser.PositionList = positionDetails.ToList();
            return View(currUser);
        }

        #endregion

        #region Test Suite
        public ActionResult GetTestSuiteDetails([DataSourceRequest] DataSourceRequest request)
        {
            var tags = _tagsService.GetTagsDetails();
            var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.IsDeleted == false).OrderByDescending(model => model.TestSuiteId).ToArray();
            var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist);
            foreach (var item in viewModels)
            {
                item.PositionName = GetPosition(item.Position);
                List<Int32> TagId = item.PrimaryTags.Split(',').Select(int.Parse).ToList();
                item.PrimaryTagNames = string.Join(",", (from a in tags
                                                         where TagId.Contains(a.TagId)
                                                         select a.TagName));
                if (!string.IsNullOrWhiteSpace(item.SecondaryTags))
                {
                    TagId = item.SecondaryTags.Split(',').Select(int.Parse).ToList();
                    item.PrimaryTagNames += "," + string.Join(",", (from a in tags
                                                                    where TagId.Contains(a.TagId)
                                                                    select a.TagName));
                }
            }
            return Json(viewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private string GetPosition(int positionId)
        {
            return _positionService.GetPositionById(positionId).PositionName;
        }

        public ActionResult TestSuiteList()
        {
            return View();
        }

        public ActionResult TestSuiteAdd(Int32 testSuiteId = 0)
        {
            TestSuiteViewModel testSuite = new TestSuiteViewModel();
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            if (testSuiteId == 0)
            {
                ViewBag.Type = "New";
                testSuite.TagList = tagDetails.ToList();
                testSuite.PositionList = positionDetails.ToList();
                testSuite.ObjectiveQuestionsCount = "20";
                testSuite.PracticalQuestionsCount = "1";

                return View(testSuite);
            }
            else
            {
                var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).ToArray();
                var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist).SingleOrDefault();
                if (viewModels != null)
                {
                    ViewBag.Type = "Edit";
                    viewModels.TagList = tagDetails.ToList();
                    viewModels.PositionList = positionDetails.ToList();
                    viewModels.PrimaryTagIds = viewModels.PrimaryTags.Split(',').ToList();
                    if (!string.IsNullOrWhiteSpace(viewModels.SecondaryTags))
                    {
                        viewModels.SecondaryTagIds = viewModels.SecondaryTags.Split(',').ToList();
                    }
                }
                return View(viewModels);
            }
        }

        public ActionResult TestSuiteSave(TestSuiteViewModel testSuiteView)
        {
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteName == testSuiteView.TestSuiteName && model.TestSuiteId != testSuiteView.TestSuiteId);
            if (testSuiteDetails.Count() > 0)
                ModelState.AddModelError(string.Empty, "The Test Suite already exists, please create with other name.");
            if (testSuiteView.SecondaryTagIds != null)
            {
                if (testSuiteView.PrimaryTagIds.All(testSuiteView.SecondaryTagIds.Contains) || testSuiteView.SecondaryTagIds.All(testSuiteView.PrimaryTagIds.Contains))
                    ModelState.AddModelError(string.Empty, "Tags should unique in primary and secondary field.");
            }

            if (ModelState.IsValid)
            {
                var testSuiteDomainModel = _mappingService.Map<TestSuiteViewModel, TestSuite>(testSuiteView);
                testSuiteDomainModel.PrimaryTags = string.Join(",", testSuiteView.PrimaryTagIds);
                if (testSuiteView.SecondaryTagIds != null)
                {
                    testSuiteDomainModel.SecondaryTags = string.Join(",", testSuiteView.SecondaryTagIds);
                }

                TempData.Add("IsNewTestSuite", 1);
                if (testSuiteView.TestSuiteId == 0 || testSuiteView.IsCopy == true)
                {
                    _testSuiteService.Add(testSuiteDomainModel);                    
                    return RedirectToAction("TestSuiteList");
                }
                else
                {
                    _testSuiteService.Update(testSuiteDomainModel);
                    return RedirectToAction("TestSuiteList");
                }
            }
            //ViewBag.ModelError = 1;
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            testSuiteView.TagList = tagDetails.ToList();
            testSuiteView.PositionList = positionDetails.ToList();
            return View("TestSuiteAdd", testSuiteView);
        }

        public ActionResult TestSuiteDelete(int testSuiteId)
        {
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).SingleOrDefault();
            if (testSuiteDetails != null)
            {               
                _testSuiteService.Delete(testSuiteDetails);
                return Json(1);
            }
            else
            {
                return Json(-1);
            }
        }

        public ActionResult TestSuiteCopy(int testSuiteId = 0)
        {
            TestSuiteViewModel testSuite = new TestSuiteViewModel();
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            testSuite.TagList = tagDetails.ToList();

            if (testSuiteId == 0)
            {
                return View(testSuite);
            }
            else
            {
                var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).ToArray();
                var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist).SingleOrDefault();
                if (viewModels != null)
                {
                    ViewBag.Type = "Copy";
                    viewModels.IsCopy = true;
                    viewModels.TestSuiteName = "Copy " + viewModels.TestSuiteName;
                    viewModels.TagList = tagDetails.ToList();
                    viewModels.PositionList = positionDetails.ToList();
                    viewModels.PrimaryTagIds = viewModels.PrimaryTags.Split(',').ToList();
                    if (!string.IsNullOrWhiteSpace(viewModels.SecondaryTags))
                    {
                        viewModels.SecondaryTagIds = viewModels.SecondaryTags.Split(',').ToList();
                    }
                }
                return View("TestSuiteAdd", viewModels);
            }
        }

        public ActionResult TestSuitUsers([DataSourceRequest] DataSourceRequest request)
        {           
            int testSuiteId = Convert.ToInt32(TempData["TesSuiteId"]);
            var userlist = _userService.GetUserDetails().Where(model => model.Role == "USER").OrderByDescending(model => model.UserId).ToArray();           
            var viewModels = _mappingService.Map<User[], UserViewModel[]>(userlist);
            DataSourceResult result = viewModels.ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult TestSuiteActivate(string users, int testSuiteId)
        {
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).SingleOrDefault();
            UserTestSuite userTestSuite;
            if (!string.IsNullOrWhiteSpace(users))
            {
                foreach (var item in users.Split(','))
                {
                    userTestSuite = new UserTestSuite();
                    userTestSuite.UserId = Convert.ToInt32(item);
                    userTestSuite.TestSuiteId = testSuiteId;
                    ActiveteSuite(userTestSuite, testSuiteDetails);
                }
                return Json(1);
            }            
            else
            {
                return Json(-1);
            }
        }

        public ActionResult TestSuiteUserView(int testSuiteId = 0)
        {
            TempData["TesSuiteId"] = testSuiteId;
            return PartialView("_TestSuiteAssign");
        }

        public void ActiveteSuite(UserTestSuite userTestSuite, TestSuite testSuite)
        {
            Int32 userTestSuiteId = _testSuiteService.AddUserTestSuite(userTestSuite);
            if (!string.IsNullOrWhiteSpace(testSuite.SecondaryTags))
                testSuite.PrimaryTags += "," + testSuite.SecondaryTags;
            Int32[] tags = testSuite.PrimaryTags.Split(',').Select(Int32.Parse).ToArray();
            //Question question = from a in _questionService.GetQuestion()
            //                    where a.tag
        }
        #endregion
                
        #region Position

        public ActionResult GetPositionDetails([DataSourceRequest] DataSourceRequest request)
        {
            var positionlist = _positionService.GetPositionDetails().OrderByDescending(model => model.PositionId);
            return Json(positionlist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Positions()
        {
            return PartialView();
        }

        public ActionResult PositionSave(Position position)
        {
            if (ModelState.IsValid)
            {
                if (position.PositionId == 0)
                    return Json(_positionService.Add(position));
                else
        {
                    _positionService.Update(position);
                    return Json(1);
                }
            }
            return View("TestSuiteAdd", position);
        }
        #endregion Position

        public ActionResult ViewQuestion()
        {
            List<Question> QList = new List<Question>()
            {
                new Question { Id=1, QuestionDescription="First Question", Option1="Option1", Option2="Option2", Option3="Option3", Option4="Option4" },
                new Question { Id=1, QuestionDescription="First Question", Option1="Option1", Option2="Option2", Option3="Option3", Option4="Option4" },
                new Question { Id=1, QuestionDescription="First Question", Option1="Option1", Option2="Option2", Option3="Option3", Option4="Option4" }
            };

            return View(QList);
        }

        public ActionResult CreatePDF()
        {
            List<Question> QList = new List<Question>()
            {
                new Question { Id=1, QuestionDescription="First Question", Option1="Option1", Option2="Option2", Option3="Option3", Option4="Option4" },
                new Question { Id=1, QuestionDescription="First Question", Option1="Option1", Option2="Option2", Option3="Option3", Option4="Option4" },
                new Question { Id=1, QuestionDescription="First Question", Option1="Option1", Option2="Option2", Option3="Option3", Option4="Option4" }
            };

            var pdf = new RazorPDF.PdfResult(QList, "CreatePDF");

            // Add to the view bag
           // pdf.ViewBag.Title = "Title from ViewBag";

            return pdf;

          //  return View(QList);
        }

        public ActionResult SubmittedTest(int canditateId)
        {
            var userDetails = _userService.GetUserDetails().Where(x => x.UserId == canditateId).FirstOrDefault();
            //var testSuitDetails=_testSuiteService.GetTestSuiteDetails().Where(x=>x.)
            SubmittedTestViewModel submittedTestViewModel = new Models.SubmittedTestViewModel();
            submittedTestViewModel.FirstName = userDetails.FirstName;
            submittedTestViewModel.LastName = userDetails.LastName;
            return View(submittedTestViewModel);
        }
    }

}
