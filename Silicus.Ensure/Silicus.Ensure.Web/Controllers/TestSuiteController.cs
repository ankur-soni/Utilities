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

namespace Silicus.Ensure.Web.Controllers
{
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

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add(Int32 testSuiteId = 0)
        {
            TestSuiteViewModel testSuite = new TestSuiteViewModel();
            TestSuiteTagViewModel tagView = new TestSuiteTagViewModel();
            List<TestSuiteTagViewModel> tags = new List<TestSuiteTagViewModel>();            
            tagView.TagId = 0;
            tagView.TagName = "";            
            tags.Add(tagView);
            testSuite.Tags = tags;
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

        public ActionResult Save(TestSuiteViewModel testSuiteView)
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
                    return RedirectToAction("List");
                }
                else
                {
                    _testSuiteService.Update(testSuiteDomainModel);
                    return RedirectToAction("List");
                }
            }
            //ViewBag.ModelError = 1;
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            testSuiteView.TagList = tagDetails.ToList();
            testSuiteView.PositionList = positionDetails.ToList();
            return View("Add", testSuiteView);
        }

        public ActionResult Delete(int testSuiteId)
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

        public ActionResult Copy(int testSuiteId = 0)
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
                return View("Add", viewModels);
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
            _questionService.GetQuestion().Select(x => x.Tags.Split(',').ToArray());
            //ListA.Where(a => ListX.Any(x => x.b == a.b));
            //Question question = from a in _questionService.GetQuestion()
            //                    where a.Tags.Contains()//.Split(',').Select(Int32.Parse).ToArray().Contains(tags)
            //                    select a;
        }

        [HttpPost]
        public ActionResult Read()
        {
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            return Json(tagDetails);
        }

        public ActionResult GetTags(string term)
        {
            var tagDetails = _tagsService.GetTagsDetails().OrderBy(model => model.TagName);
            return Json(tagDetails);
        }
    }
}