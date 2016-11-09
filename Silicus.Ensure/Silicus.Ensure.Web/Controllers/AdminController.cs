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

        public AdminController(IEmailService emailService, ITagsService tagService, ITestSuiteService testSuiteService, MappingService mappingService, IQuestionService questionService, IUserService userService)
        {
            _emailService = emailService;
            _tagsService = tagService;
            _testSuiteService = testSuiteService;
            _mappingService = mappingService;
            _questionService = questionService;
            _userService = userService;
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
            return View("TagAdd", tag);
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
            if (positionId == 1)
                return "Jr. Developer";
            else if (positionId == 2)
                return "Sr. Developer";
            else
                return "QA";
        }

        public ActionResult TestSuiteList()
        {
            return View();
        }

        public ActionResult TestSuiteAdd(Int32 testSuiteId = 0)
        {
            TestSuiteViewModel testSuite = new TestSuiteViewModel();
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            if (testSuiteId == 0)
            {
                ViewBag.Type = "New";
                testSuite.TagList = tagDetails.ToList();
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
            if (testSuiteView.PrimaryTagIds.All(testSuiteView.SecondaryTagIds.Contains) || testSuiteView.SecondaryTagIds.All(testSuiteView.PrimaryTagIds.Contains))
                ModelState.AddModelError(string.Empty, "Tags should unique in primary and secondary field.");

            if (ModelState.IsValid)
            {
                var testSuiteDomainModel = _mappingService.Map<TestSuiteViewModel, TestSuite>(testSuiteView);
                testSuiteDomainModel.PrimaryTags = string.Join(",", testSuiteView.PrimaryTagIds);
                if (testSuiteView.SecondaryTagIds != null)
                {
                    testSuiteDomainModel.SecondaryTags = string.Join(",", testSuiteView.SecondaryTagIds);
                }

                if (testSuiteView.TestSuiteId == 0 || testSuiteView.IsCopy == true)
                {
                    _testSuiteService.Add(testSuiteDomainModel);
                    TempData.Add("IsNewTestSuite", 1);
                    return RedirectToAction("TestSuiteList");
                }
                else
                {
                    _testSuiteService.Update(testSuiteDomainModel);
                    return RedirectToAction("TestSuiteList");
                }
            }
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            testSuiteView.TagList = tagDetails.ToList();
            return View("TestSuiteAdd", testSuiteView);
        }

        public ActionResult TestSuiteDelete(int testSuiteId)
        {
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).SingleOrDefault();
            if (testSuiteDetails != null)
            {
                testSuiteDetails.IsDeleted = true;
                testSuiteDetails.DeletedDate = DateTime.UtcNow;
                _testSuiteService.Update(testSuiteDetails);
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
                    viewModels.PrimaryTagIds = viewModels.PrimaryTags.Split(',').ToList();
                    if (!string.IsNullOrWhiteSpace(viewModels.SecondaryTags))
                    {
                        viewModels.SecondaryTagIds = viewModels.SecondaryTags.Split(',').ToList();
                    }
                }
                return View("TestSuiteAdd", viewModels);
            }
        }

        public ActionResult AssignSuite(int SuiteId, int Userid)
        {
            return View();
        } 
        #endregion

        #region Question Bank
        //---------------------------------- Question Bank Section -----------------------------------

        public ActionResult AddQuestions(string QuestionId)
        {
            QuestionModel Que = new QuestionModel();
            Que.QuestionType = "0";
            Que.Success = 0;
            Que.Edit = false;
            Que.Tags = Tags();
            return View(Que);
        }

        [HttpPost]
        public ActionResult AddQuestions(QuestionModel question)
        {
            Question Que = new Question
            {
                QuestionType = Convert.ToInt32(question.QuestionType),
                QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription),
                AnswerType = Convert.ToInt32(question.AnswerType),
                Option1 = question.Option1,
                Option2 = question.Option2,
                Option3 = question.Option3,
                Option4 = question.Option4,
                CorrectAnswer = InlineList(question.CorrectAnswer),
                Answer = HttpUtility.HtmlDecode(question.Answer),
                Tags = InlineList(question.SkillTag),
                Competency = Convert.ToInt32(question.Competency),
                Duration = question.Duration,
                IsPublishd = true,
                IsDeleted = false
            };


            if (question.Edit)
            {
                Que.Id = question.QuestionId;
                Que.CreatedOn = question.CreatedOn;
                Que.CreatedBy = question.CreatedBy;
                Que.ModifiedOn = DateTime.Now;
                Que.ModifiedBy = 0;

                question = new QuestionModel();
                _questionService.Update(Que);
                question.Success = 1;
                question.Edit = true;
            }
            else
            {
                Que.CreatedOn = DateTime.Now;
                Que.CreatedBy = 0;
                Que.ModifiedOn = DateTime.Now;
                Que.ModifiedBy = 0;

                question = new QuestionModel();
                int id = _questionService.Add(Que);
                if (id > 0)
                {
                    question.Success = 1;
                    question.Edit = false;
                }
            }

            question.Tags = Tags();
            question.QuestionType = "0";
            return View(question);
        }

        public ActionResult QuestionBank()
        {
            List<QuestionModel> Qmodel = new List<QuestionModel>();
            QuestionModel model;
            IEnumerable<Question> Que = _questionService.GetQuestion();
            foreach (var q in Que)
            {
                model = new QuestionModel();
                model.QuestionId = q.Id;
                model.QuestionDescription = TruncateLongString(q.QuestionDescription, 100);
                model.QuestionType = GetQuestionType(q.QuestionType);
                model.Tag = GetTags(q.Tags);
                model.Competency = GetCompetency(q.Competency);
                Qmodel.Add(model);
            }
            return View(Qmodel);
        }

        public ActionResult EditQuestion(string QuestionId)
        {
            if (!string.IsNullOrEmpty(QuestionId))
            {
                Question question = _questionService.GetSingleQuestion(Convert.ToInt32(QuestionId));

                QuestionModel Que = new QuestionModel
                {
                    QuestionId = question.Id,
                    QuestionType = question.QuestionType.ToString(),
                    QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription),
                    AnswerType = question.AnswerType.ToString(),
                    Option1 = question.Option1,
                    Option2 = question.Option2,
                    Option3 = question.Option3,
                    Option4 = question.Option4,
                    CorrectAnswer = CorrectAnswer(question.CorrectAnswer),
                    Answer = HttpUtility.HtmlDecode(question.Answer),
                    SkillTag = TagList(question.Tags),
                    Competency = question.Competency.ToString(),
                    Duration = question.Duration,
                    CreatedOn = question.CreatedOn,
                    CreatedBy = question.CreatedBy,
                    Success = 0,
                    Edit = true,
                    Tags = Tags()
                };
                return View("AddQuestions", Que);
            }
            return View("AddQuestions", null);
        }

        [HttpDelete]
        public JsonResult DeleteQuestion(int QuestionId)
        {
            _questionService.Delete(QuestionId);
            return Json(1);
        }

        #region Question Bank Private Methods
        private string InlineList(List<string> list)
        {
            string lst = "";
            if (list != null)
            {
                int cnt = list.Count();
                int commacnt = 0;
                foreach (string str in list)
                {
                    commacnt++;
                    if (commacnt == cnt)
                        lst += str;
                    else
                        lst += str + ",";

                }
            }

            return lst;
        }

        private List<Tags> Tags()
        {
            List<Tags> tags = _tagsService.GetTagsDetails().ToList();
            return tags;
        }

        private string GetQuestionType(int type)
        {
            if (type == 1)
                return "Objective";
            else
                return "Practical";
        }

        private string GetCompetency(int type)
        {
            if (type == 1)
                return "Beginner";
            else if (type == 2)
                return "Intermediate";
            else
                return "Expert";
        }

        private string GetTags(string tags)
        {
            string ret = null;
            if (!string.IsNullOrEmpty(tags))
            {
                string[] str = tags.Split(',');
                int cnt = str.Count();
                List<Tags> tagsList = Tags();
                int count = 0;
                foreach (string s in str)
                {
                    count++;
                    if (count == cnt)
                        ret += tagsList.Find(x => x.TagId == Convert.ToInt32(s)).TagName;
                    else
                        ret += tagsList.Find(x => x.TagId == Convert.ToInt32(s)).TagName + " | ";
                }
            }
            return ret;
        }

        private List<string> TagList(string tag)
        {
            List<string> tags = new List<string>();
            List<Tags> TagSkill = Tags();
            if (!string.IsNullOrEmpty(tag))
            {
                string[] str = tag.Split(',');
                if (str.Count() > 0)
                {
                    foreach (string s in str)
                    {
                        tags.Add(TagSkill.Find(x => x.TagId == Convert.ToInt32(s)).TagId.ToString());
                    }
                }
            }

            return tags;
        }

        private List<string> CorrectAnswer(string Ans)
        {
            List<string> answer = new List<string>();
            if (!string.IsNullOrEmpty(Ans))
            {
                string[] str = Ans.Split(',');
                if (str.Count() > 0)
                {
                    foreach (string s in str)
                    {
                        answer.Add(s);
                    }
                }
            }
            return answer;
        }

        private string TruncateLongString(string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }
        #endregion

        #endregion
    }
}
