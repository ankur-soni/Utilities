using System;
using System.Linq;
using System.Web.Mvc;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Mappings;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class TestSuiteController : Controller
    {
        private readonly ITestSuiteService _testSuiteService;
        private readonly ITagsService _tagsService;
        private readonly IMappingService _mappingService;
        private readonly IPositionService _positionService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public TestSuiteController(ITestSuiteService testSuiteService, ITagsService tagsService, IMappingService mappingService, IPositionService positionService, IQuestionService questionService, IUserService userService)
        {
            _testSuiteService = testSuiteService;
            _tagsService = tagsService;
            _mappingService = mappingService;
            _positionService = positionService;
            _questionService = questionService;
            _userService = userService;
        }

        public ActionResult GetTestSuiteDetails([DataSourceRequest] DataSourceRequest request)
        {
            _testSuiteService.TestSuiteActivation();
            var tags = _tagsService.GetTagsDetails();
            var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.IsDeleted == false).OrderByDescending(model => model.TestSuiteId).ToArray();
            var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist);
            var userTestSuites = _userService.GetAllTestSuiteDetails();
            bool userInRole = MvcApplication.getCurrentUserRoles().Contains((Silicus.Ensure.Models.Constants.RoleName.Admin.ToString()));
            foreach (var item in viewModels)
            {
                item.PositionName = GetPosition(item.Position) == null ? "deleted from master" : GetPosition(item.Position).PositionName;
                List<Int32> TagId = item.PrimaryTags.Split(',').Select(int.Parse).ToList();
                item.PrimaryTagNames = string.Join(",", (from a in tags
                                                         where TagId.Contains(a.TagId)
                                                         select a.TagName));
                item.StatusName = ((TestSuiteStatus)item.Status).ToString();
                item.UserInRole = userInRole;
                item.IsAssigned = userTestSuites.Any(y => y.TestSuiteId == item.TestSuiteId && y.StatusId == (int)TestStatus.Assigned);
            }
            return Json(viewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private Position GetPosition(int positionId)
        {
            return _positionService.GetPositionById(positionId);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add(Int32 testSuiteId = 0)
        {
            ViewData["categories"] = _tagsService.GetTagsDetails();
            TestSuiteViewModel testSuite = new TestSuiteViewModel();
            //return View("AddTestSuite", testSuite);
            List<TestSuiteTagViewModel> tags = new List<TestSuiteTagViewModel>();
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);

            if (testSuiteId == 0)
            {
                ViewBag.Type = "New";
                testSuite.PositionList = positionDetails.ToList();
                return View("AddTestSuite", testSuite);
            }
            else
            {
                var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).ToArray();
                var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist).SingleOrDefault();
                if (viewModels != null)
                {
                    ViewBag.Type = "Edit";
                    //if (!string.IsNullOrWhiteSpace(viewModels.ExperienceRange))
                    //{
                    //    viewModels.ExperienceRangeId = viewModels.ExperienceRange.Split(',').ToList();
                    //}
                    viewModels.PositionList = positionDetails.ToList();
                    List<TestSuiteTagViewModel> testSuiteTags;
                    GetTestSuiteTags(testSuitelist.SingleOrDefault(), out testSuiteTags);
                    viewModels.Tags = testSuiteTags;
                }
                return View("AddTestSuite", viewModels);
            }
        }

        public ActionResult Save(TestSuiteViewModel testSuiteView)
        {
            List<TestSuiteTag> tagModel = new List<TestSuiteTag>();
            string errorMessage = string.Empty;
            string tagId;
            var tags = _tagsService.GetTagsDetails();
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteName == testSuiteView.TestSuiteName && model.TestSuiteId != testSuiteView.TestSuiteId);
            if (testSuiteDetails.Count() > 0)
            {
                errorMessage = "The Test Suite already exists, please create with other name.\n";
            }
            string[] tagArry = testSuiteView.PrimaryTagNames.Split(',');
            foreach (var tag in tagArry)
            {
                if (!tags.Any(x => x.TagName == tag))
                {
                    errorMessage += "Please enter correct tag name.\n";
                    break;
                }
            }
            for (int i = 0; i < tagArry.Length; i++)
            {
                tagId = tags.Where(x => x.TagName == tagArry[i]).Select(x => x.TagId).SingleOrDefault().ToString();
                if (string.IsNullOrWhiteSpace(testSuiteView.PrimaryTags))
                {
                    testSuiteView.PrimaryTags = tagId;
                }
                else
                {
                    testSuiteView.PrimaryTags += "," + tagId;
                }
            }
            if (testSuiteView.FromExperience == null)
            {
                testSuiteView.FromExperience = 0;
            }
            if (testSuiteView.ToExperience == null)
            {
                testSuiteView.ToExperience = 0;
            }
            var testSuiteDomainModel = _mappingService.Map<TestSuiteViewModel, TestSuite>(testSuiteView);
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                if (testSuiteView.IsCopy == true || testSuiteView.TestSuiteId == 0)
                {
                    testSuiteDomainModel.Status = Convert.ToInt32(TestSuiteStatus.Pending);
                    _testSuiteService.Add(testSuiteDomainModel);
                }
                else
                {
                    _testSuiteService.Update(testSuiteDomainModel);
                }
            }
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                return Json(new { status = "success", message = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "error", message = errorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(TestSuiteViewModel testSuite)
        {
            if (testSuite != null)
            {
                var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuite.TestSuiteId && model.IsDeleted == false).SingleOrDefault();
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
            else
            {
                return Json(-1);
            }
        }

        public ActionResult Copy(int testSuiteId = 0)
        {
            TestSuiteViewModel testSuite = new TestSuiteViewModel();
            List<TestSuiteTagViewModel> tags = new List<TestSuiteTagViewModel>();
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
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
                    //if (!string.IsNullOrWhiteSpace(viewModels.ExperienceRange))
                    //{
                    //    viewModels.ExperienceRangeId = viewModels.ExperienceRange.Split(',').ToList();
                    //}
                    ViewBag.Type = "Copy";
                    viewModels.IsCopy = true;
                    viewModels.TestSuiteName = "Copy " + viewModels.TestSuiteName;
                    List<TestSuiteTagViewModel> testSuiteTags;
                    GetTestSuiteTags(testSuitelist.SingleOrDefault(), out testSuiteTags);
                    viewModels.Tags = testSuiteTags;
                    viewModels.PositionList = positionDetails.ToList();
                }
                return View("AddTestSuite", viewModels);
            }
        }

        public ActionResult TestSuitUsers([DataSourceRequest] DataSourceRequest request)
        {
            var userlist = _userService.GetUserDetails().Where(x => x.Role.ToLower() == RoleName.Candidate.ToString().ToLower()
                                                        && (x.TestStatus == Convert.ToString(TestStatus.NotAssigned) || x.TestStatus == Convert.ToString(TestStatus.Assigned))).ToArray();
            var viewModels = _mappingService.Map<User[], UserViewModel[]>(userlist);

            int testSuiteId = Convert.ToInt32(TempData["TesSuiteId"]);
            DataSourceResult result = viewModels.ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult AssignTest(string users, int testSuiteId)
        {
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).SingleOrDefault();
            var alreadyAssignedTestSuites = _testSuiteService.GetAllUserIdsForTestSuite(testSuiteId);
            UserTestSuite userTestSuite;
            if (!string.IsNullOrWhiteSpace(users))
            {
                foreach (var item in users.Split(','))
                {
                    userTestSuite = new UserTestSuite();
                    userTestSuite.UserId = Convert.ToInt32(item);
                    if (!alreadyAssignedTestSuites.Contains(userTestSuite.UserId))
                    {
                        userTestSuite.TestSuiteId = testSuiteId;
                        _testSuiteService.AssignSuite(userTestSuite, testSuiteDetails);
                        var selectUser = _userService.GetUserDetails().Where(model => model.UserId == Convert.ToInt32(item)).FirstOrDefault();
                        selectUser.TestStatus = Convert.ToString(TestStatus.Assigned);
                        _userService.Update(selectUser);
                    }
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

        public ActionResult GetTags(string term)
        {
            var tagDetails = _tagsService.GetTagsDetails();
            return Json(tagDetails);
        }

        private void GetTestSuiteTags(TestSuite testSuite, out List<TestSuiteTagViewModel> testSuiteTags)
        {
            TestSuiteTagViewModel testSuiteTagViewModel;
            testSuiteTags = new List<TestSuiteTagViewModel>();
            var tagList = _tagsService.GetTagsDetails();
            string[] tags = testSuite.PrimaryTags.Split(',');
            string[] weights = testSuite.Weights.Split(',');
            string[] proficiency = testSuite.Proficiency.Split(',');

            for (int i = 0; i < tags.Length; i++)
            {
                testSuiteTagViewModel = new TestSuiteTagViewModel();
                testSuiteTagViewModel.TagId = Convert.ToInt32(tags[i]);
                testSuiteTagViewModel.TagName = tagList.Where(x => x.TagId == testSuiteTagViewModel.TagId).Select(x => x.TagName).SingleOrDefault();
                testSuiteTagViewModel.Weightage = Convert.ToInt32(weights[i]);
                testSuiteTagViewModel.Proficiency = Convert.ToInt32(proficiency[i]);
                testSuiteTagViewModel.Minutes = testSuite.Duration * Convert.ToInt32(weights[i]) / 100;
                testSuiteTags.Add(testSuiteTagViewModel);
            }
        }

        public ActionResult ViewQuestion(int TestSuitId)
        {
            int count = 0;
            TestSuiteViewQuesModel testSuiteViewQuesModel = new Models.TestSuiteViewQuesModel();
            List<TestSuiteQuestion> testSuiteQuestionList = new List<Models.TestSuiteQuestion>();
            try
            {
                TestSuite testSuitDetails = _testSuiteService.GetTestSuitById(TestSuitId);
                if (testSuitDetails != null && testSuitDetails.Status == Convert.ToInt32(TestSuiteStatus.Ready))
                {
                    var questionList = _testSuiteService.GetPriview(testSuitDetails);
                    foreach (var pQuestion in questionList)
                    {
                        count++;
                        testSuiteQuestionList.Add(new TestSuiteQuestion()
                        {
                            QuestionType = pQuestion.QuestionType,
                            QuestionNumber = count,
                            QuestionDescription = pQuestion.QuestionDescription,
                            OptionCount = pQuestion.OptionCount,
                            Answer = pQuestion.Answer,
                            CorrectAnswer = pQuestion.CorrectAnswer,
                            Id = pQuestion.Id,
                            Marks = pQuestion.Marks,
                            Option1 = pQuestion.Option1,
                            Option2 = pQuestion.Option2,
                            Option3 = pQuestion.Option3,
                            Option4 = pQuestion.Option4,
                            Option5 = pQuestion.Option5,
                            Option6 = pQuestion.Option6,
                            Option7 = pQuestion.Option7,
                            Option8 = pQuestion.Option8,
                        });
                    }

                    testSuiteViewQuesModel.TestSuiteQuestion = testSuiteQuestionList;
                    testSuiteViewQuesModel.TestSuiteName = testSuitDetails.TestSuiteName;
                    testSuiteViewQuesModel.Duration = testSuitDetails.Duration;
                    testSuiteViewQuesModel.ObjectiveCount = questionList.Where(x => x.QuestionType == 1).ToList().Count;
                    testSuiteViewQuesModel.PracticalCount = questionList.Where(x => x.QuestionType == 2).ToList().Count;
                }
                else
                {
                    testSuiteViewQuesModel.ErrorMessage = "Test suite is not ready.";

                }

            }
            catch
            {
                testSuiteViewQuesModel.ErrorMessage = "Something went wrong! Please try later.";
            }

            return View(testSuiteViewQuesModel);
        }

        public JsonResult GetUserIdsForTestSuite(int testSuiteId)
        {
            var users = _testSuiteService.GetAllUserIdsForTestSuite(testSuiteId);
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTestSuiteNameAvailable(string testSuiteName)
        {
            var testSuite = _testSuiteService.GetTestSuiteByName(testSuiteName);
            return Json(testSuite == null, JsonRequestBehavior.AllowGet);
        }
    }
}