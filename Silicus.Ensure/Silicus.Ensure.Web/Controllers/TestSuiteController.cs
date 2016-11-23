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
            TestSuiteTagViewModel tagView;
            List<TestSuiteTagViewModel> tags = new List<TestSuiteTagViewModel>();     
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);            

            if (testSuiteId == 0)
            {
                ViewBag.Type = "New";             
                testSuite.PositionList = positionDetails.ToList();                    
                return View(testSuite);
            }
            else
            {
                var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == testSuiteId && model.IsDeleted == false).ToArray();
                var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist).SingleOrDefault();
                if (viewModels != null)
                {
                    ViewBag.Type = "Edit";                 
                    viewModels.PositionList = positionDetails.ToList();                  
                     foreach(var tag in testSuitelist.SingleOrDefault().TestSuiteTags)
                     {
                         tagView = new TestSuiteTagViewModel();
                         tagView.TagId = tag.TagId;
                         tagView.TagName = tagDetails.Where(x => x.TagId == tag.TagId).Select(y => y.TagName).SingleOrDefault();
                         tagView.Weightage = Convert.ToString(tag.Weightage);
                         tags.Add(tagView);
                     }
                     viewModels.Tags = tags;                 
                }
                return View(viewModels);
            }
        }

        public ActionResult Save(TestSuiteViewModel testSuiteView)
        {
            var tagId=0;
            TestSuiteTag tagView;
            List<TestSuiteTag> tagModel = new List<TestSuiteTag>();
            string errorMessage = string.Empty;
            var tags = _tagsService.GetTagsDetails();
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteName == testSuiteView.TestSuiteName && model.TestSuiteId != testSuiteView.TestSuiteId);
            if (testSuiteDetails.Count() > 0)
            { 
                errorMessage = "The Test Suite already exists, please create with other name.\n"; 
            }
                string[] arr = testSuiteView.PrimaryTagNames.Split(',');
                testSuiteView.PrimaryTagNames = string.Empty;
                for (int i = 1; i < arr.Length;i=i+2 )
                {
                    tagView = new TestSuiteTag();
                    tagId = tags.Where(x => x.TagName == arr[i - 1]).Select(x => x.TagId).SingleOrDefault();
                    if (testSuiteView.PrimaryTagNames==string.Empty)
                    {
                        tagView.TagId = tagId;
                        tagView.Weightage = Convert.ToInt32(arr[i]);
                        testSuiteView.PrimaryTagNames += tagId;                                             
                    }
                    else
                    {
                        tagView.TagId = tagId;
                        tagView.Weightage = Convert.ToInt32(arr[i]);
                        testSuiteView.PrimaryTagNames += ","+ tagId;
                    }
                    tagModel.Add(tagView);
                }

                var testSuiteDomainModel = _mappingService.Map<TestSuiteViewModel, TestSuite>(testSuiteView);
                testSuiteDomainModel.TestSuiteTags = tagModel;
                testSuiteDomainModel.PrimaryTags = testSuiteView.PrimaryTagNames;               
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    if (testSuiteView.TestSuiteId == 0 || testSuiteView.IsCopy == true)
                    {
                        _testSuiteService.Add(testSuiteDomainModel);                      
                    }
                    else
                    {
                        _testSuiteService.Update(testSuiteDomainModel);                    
                    }
                }          
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    return Json(new { status = "success", message =""}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "error", message = errorMessage }, JsonRequestBehavior.AllowGet);
                }          
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
            TestSuiteTagViewModel tagView;
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
                    ViewBag.Type = "Copy";
                    viewModels.IsCopy = true;
                    viewModels.TestSuiteName = "Copy " + viewModels.TestSuiteName;
                    foreach (var tag in testSuitelist.SingleOrDefault().TestSuiteTags)
                    {
                        tagView = new TestSuiteTagViewModel();
                        tagView.TagId = tag.TagId;
                        tagView.TagName = tagDetails.Where(x => x.TagId == tag.TagId).Select(y => y.TagName).SingleOrDefault();
                        tagView.Weightage = Convert.ToString(tag.Weightage);
                        tags.Add(tagView);
                    }
                    viewModels.Tags = tags;            
                    viewModels.PositionList = positionDetails.ToList();                
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
        }

        public ActionResult GetTags(string term)
        {
            var tagDetails = _tagsService.GetTagsDetails();
            return Json(tagDetails);
        }
    }
}