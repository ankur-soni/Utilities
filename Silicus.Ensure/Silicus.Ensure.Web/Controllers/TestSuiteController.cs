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
using System.Configuration;

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
                    List<TestSuiteTagViewModel> testSuiteTags;
                    GetTestSuiteTags(testSuitelist.SingleOrDefault(), out testSuiteTags);
                    viewModels.Tags = testSuiteTags;
                }
                return View(viewModels);
            }
        }

        public ActionResult Save(TestSuiteViewModel testSuiteView)
        {          
            List<TestSuiteTag> tagModel = new List<TestSuiteTag>();
            string errorMessage = string.Empty;
            var tags = _tagsService.GetTagsDetails();
            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteName == testSuiteView.TestSuiteName && model.TestSuiteId != testSuiteView.TestSuiteId);
            if (testSuiteDetails.Count() > 0)
            { 
                errorMessage = "The Test Suite already exists, please create with other name.\n"; 
            }
            string[] tagArry = testSuiteView.PrimaryTagNames.Split(',');
            string tagId;
            for (int i = 0; i < tagArry.Length; i = i + 2)
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
                if (string.IsNullOrWhiteSpace(testSuiteView.Weights))
                {
                    testSuiteView.Weights = tagArry[i + 1];
                }
                else
                {
                    testSuiteView.Weights += "," + tagArry[i + 1];
                }
            }
            var testSuiteDomainModel = _mappingService.Map<TestSuiteViewModel, TestSuite>(testSuiteView);
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
                return Json(new { status = "success", message = "" }, JsonRequestBehavior.AllowGet);
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
                    List<TestSuiteTagViewModel> testSuiteTags;
                    GetTestSuiteTags(testSuitelist.SingleOrDefault(), out testSuiteTags);
                    viewModels.Tags = testSuiteTags;            
                    viewModels.PositionList = positionDetails.ToList();                
                }
                return View("Add", viewModels);
            }
        }

        public ActionResult TestSuitUsers([DataSourceRequest] DataSourceRequest request)
        {
            var userlist = _userService.GetUserDetails().Where(p => p.Role.ToLower() == "candidate").ToArray();
            var viewModels = _mappingService.Map<User[], UserViewModel[]>(userlist);

            int testSuiteId = Convert.ToInt32(TempData["TesSuiteId"]);           
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
                    var selectUser = _userService.GetUserDetails().Where(model => model.UserId == Convert.ToInt32(item)).FirstOrDefault();
                    selectUser.TestStatus = "Assigned";
                    _userService.Update(selectUser);
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
            int optionalQuestions = Convert.ToInt32(ConfigurationManager.AppSettings["OptionalQuestion"]);
            int practicalQuestions = Convert.ToInt32(ConfigurationManager.AppSettings["PracticalQuestion"]);
            int index=0,requiredMinutes=0,minutes=0,tryCount=0;
            UserTestDetails testSuiteDetail;            
            List<TestSuiteTagViewModel> testSuiteTags;
            List<UserTestDetails> testSuiteDetails = new List<UserTestDetails>();
            List<Question> questions = new List<Question>();
            var questionBank = _questionService.GetQuestion();
            GetTestSuiteTags(testSuite, out testSuiteTags);
            foreach(var tag in testSuiteTags)
            {                
                var questionList = questionBank.Where(p => p.Tags.Split(',').Contains(Convert.ToString(tag.TagId))).ToList();
                if(questionList.Sum(x=>x.Duration) > tag.Minutes)
                {
                    //Optional Questions
                    var optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency == testSuite.Competency);
                    requiredMinutes = tag.Minutes * Convert.ToInt32(optionalQuestions) / 100;
                    if (optionalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                    {
                        Random random = new Random();
                        do
                        {
                            optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency == testSuite.Competency);                         
                            index = random.Next(optionalQuestion.Count());
                            if (index != 0)
                            {
                                Question question = optionalQuestion.ElementAt(index);
                                if (!questions.Exists(x => x.Id == question.Id))
                                {
                                    questions.Add(question);
                                    minutes += question.Duration;
                                }
                            }
                            tryCount += 1;
                            if (tryCount > 3)
                            {
                                break;
                            }                           
                                
                        } while (requiredMinutes >= minutes);
                    }
                    else
                    {
                        foreach (var question in questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency == testSuite.Competency))
                        {
                            questions.Add(question);
                            minutes += question.Duration;
                        }
                    }
                    
                    //Practical Questions
                    minutes = 0;
                    var practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency == testSuite.Competency);
                    if (practicalQuestion.Sum(x=>x.Duration) >= requiredMinutes)
                    {
                        Random random = new Random();
                        do
                        {
                            practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency == testSuite.Competency);
                            index = random.Next(practicalQuestion.Count());
                            if (index != 0)
                            {
                                Question question = practicalQuestion.ElementAt(index);
                                if (!questions.Exists(x => x.Id == question.Id))
                                {
                                    questions.Add(question);
                                    minutes += question.Duration;
                                }
                            }
                            tryCount += 1;
                            if (tryCount > 3)
                            {
                                break;
                            }  
                        } while (requiredMinutes >= minutes);
                    }
                    else
                    {
                        foreach (var question in practicalQuestion.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency == testSuite.Competency))
                        {
                            questions.Add(question);
                            minutes += question.Duration;
                        }
                    }
                    var allQuestions = questionList.Where(x => !questions.Any(y => y.Id == x.Id));
                    if(minutes < tag.Minutes)
                    {
                        foreach(var question in allQuestions)
                        {
                            questions.Add(question);
                            minutes += question.Duration;
                            if (minutes >= tag.Minutes)
                            {
                                break;
                            }
                        }
                    }
                }                               
            }
            //Attach Questions
            foreach (var question in questions)
            {
                testSuiteDetail = new UserTestDetails();
                testSuiteDetail.QuestionId = question.Id;
                testSuiteDetails.Add(testSuiteDetail);
            }
            userTestSuite.UserTestDetails = testSuiteDetails;  
            _testSuiteService.AddUserTestSuite(userTestSuite);
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

            for (int i = 0; i < tags.Length; i++)
            {
                testSuiteTagViewModel = new TestSuiteTagViewModel();
                testSuiteTagViewModel.TagId = Convert.ToInt32(tags[i]);
                testSuiteTagViewModel.TagName = tagList.Where(x => x.TagId == testSuiteTagViewModel.TagId).Select(x => x.TagName).SingleOrDefault();
                testSuiteTagViewModel.Weightage = weights[i];
                testSuiteTagViewModel.Minutes = testSuite.Duration * Convert.ToInt32(weights[i]) / 100;
                testSuiteTags.Add(testSuiteTagViewModel);
            }
        }
    }
}