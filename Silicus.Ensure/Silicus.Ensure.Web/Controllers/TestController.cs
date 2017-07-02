using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Filters;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Web.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    [CandidateAttribute]
    [CustomAuthorize("Admin", "Panel","Employee","Candidate")]
    public class TestController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        private readonly IMappingService _mappingService;
        private Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService _utilityUserRoleService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly IUserService _userService;
        private readonly CommonController _commonController;
        //  private readonly IPositionService _positionService;
        private readonly ITagsService _tagsService;

        public TestController(IQuestionService questionService, MappingService mappingService, UtilityContainer.Services.Interfaces.IUserService containerUserService, Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService utilityUserRoleService, ITestSuiteService testSuiteService, IUserService userService, CommonController commonController, ITagsService tagsService)
        {
            //_positionService = positionService;
            //_userService = userService;
            _questionService = questionService;
            _mappingService = mappingService;
            _testSuiteService = testSuiteService;
            _containerUserService = containerUserService;
            //_utilityService = utilityService;
            _utilityUserRoleService = utilityUserRoleService;
            //_panelMemberService = panelMemberService;
            //_emailService = emailService;
            //_commonController = commonController;
            //_tagsService = tagService;
            _testSuiteService = testSuiteService;
            _userService = userService;
            _commonController = commonController;
            //_positionService = positionService;
            _tagsService = tagsService;
        }
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
              
        public ActionResult OnlineTest(int EmployeeTestSuitId,int employeeId, string CandidateId)
        {
            var userEmailId = User.Identity.Name;
            //var user = _containerUserService.FindUserByEmail(userEmailId);
            EmployeeTestSuite employeeTestSuite = _testSuiteService.GetEmployeeTestSuiteById(EmployeeTestSuitId);

            if (employeeTestSuite == null)
            {
                ViewBag.Status = 1;
                ViewBag.Msg = "No test is assigned for you, kindly contact admin.";
                return View("Welcome", new TestSuiteEmployeeModel());
            }
            else if (employeeTestSuite.EmployeeId != employeeId && employeeTestSuite.CandidateID != CandidateId)
            {
                return RedirectToAction("AssignedTest", "Employee");
            }
            else if (employeeTestSuite.StatusId != (int)CandidateStatus.TestAssigned)
            {
                ViewBag.Status = 1;
                ViewBag.Msg = "You have already submitted your test.";
                return View("Welcome", new TestSuiteEmployeeModel());
            }
            TestSuiteEmployeeModel testSuiteEmployeeModel = _mappingService.Map<EmployeeTestSuite, TestSuiteEmployeeModel>(employeeTestSuite);
            testSuiteEmployeeModel.NavigationDetails = GetNavigationDetails(testSuiteEmployeeModel.EmployeeTestSuiteId);
            testSuiteEmployeeModel.TotalQuestionCount = testSuiteEmployeeModel.PracticalCount + testSuiteEmployeeModel.ObjectiveCount;
            testSuiteEmployeeModel.DurationInMin = testSuiteEmployeeModel.RemainingTime > 0 ? testSuiteEmployeeModel.RemainingTime : testSuiteEmployeeModel.Duration;
            testSuiteEmployeeModel.EmployeeId = employeeId;
            testSuiteEmployeeModel.CandidateId = CandidateId;

            return View(testSuiteEmployeeModel);
        }

        [HttpPost]
        public JsonResult UpdateTimeCounter(int time, int employeeTestSuiteId)
        {
            EmployeeTestSuite employeeTestSuite = _testSuiteService.GetEmployeeTestSuiteById(employeeTestSuiteId);
            if (employeeTestSuite != null)
            {
                int remainingTime = time / 60;
                employeeTestSuite.RemainingTime = remainingTime;
                _testSuiteService.UpdateEmployeeTestSuite(employeeTestSuite);
            }
            return Json(1);
        }

        public ActionResult LoadQuestion(int employeeTestSuiteId)
        {
            TestDetailsViewModel testSuiteQuestionModel = TestSuiteQuestion(null, employeeTestSuiteId, (int)QuestionType.Objective);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult GetQuestionDetails(QuestionDetailsViewModel questionDetails)
        {
            questionDetails.QuestionType = _testSuiteService.GetQuestionType(questionDetails.QuestionId);
            questionDetails.Answer = HttpUtility.UrlDecode(questionDetails.Answer);
            UpdateAnswer(questionDetails.Answer, questionDetails.UserTestDetailId);
            var testSuiteQuestionModel = TestSuiteQuestion(questionDetails.QuestionId, questionDetails.UserTestSuiteId, questionDetails.QuestionType);
            ModelState.Clear();
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }


        public ActionResult OnSubmitTest(int testSuiteId, int EmployeeTestSuiteId, int? employeeTestDetailId, int EmployeeId, string answer)
        {
            // Update last question answer of test.
            answer = HttpUtility.HtmlDecode(answer);
            //_userService.UpdateUserApplicationTestDetails(userId);

            // Update total time utilization for test back to UserTestSuite.
            TestSuite suite = _testSuiteService.GetTestSuitById(testSuiteId);
            EmployeeTestSuite testSuit = _testSuiteService.GetEmployeeTestSuiteById(EmployeeTestSuiteId);
            testSuit.Duration = suite.Duration + (testSuit.ExtraCount * 10);
            testSuit.StatusId = Convert.ToInt32(CandidateStatus.TestSubmitted);
            testSuit.AttemptDate = DateTime.Now;
            _testSuiteService.UpdateEmployeeTestSuite(testSuit);

            // Calculate marks on test submit.
            CalculateMarks(EmployeeTestSuiteId, employeeTestDetailId, answer);
            //List<string> Receipient = new List<string>() { "Admin", "Panel" };
            //var users = _userService.GetUserApplicationDetailsById(userId);
            //if (users != null)
            //{
            //    var userDetails = _userService.GetUserById(users.UserId);
            //    //   _commonController.SendMailByRoleName("Online Test Submitted For " + userDetails.FirstName + " " + userDetails.LastName + "", "CandidateTestSubmitted.cshtml", Receipient, userDetails.FirstName + " " + userDetails.LastName);
            //}
            return View("CompleteTest");
        }

        private void UpdateAnswer(string answer, int? employeeTestDetailId)
        {
            EmployeeTestDetails employeeTestDetails = _testSuiteService.GetEmployeeTestDetailsId(employeeTestDetailId.Value);
            employeeTestDetails.Answer = answer;
            _testSuiteService.UpdateEmployeeTestDetails(employeeTestDetails);
        }

        private TestDetailsViewModel TestSuiteQuestion(int? questionId, int? employeeTestSuiteId, int questionType)
        {
            TestDetailsBusinessModel userTestDetails = _testSuiteService.GeEmployeeTestDetailsByEmployeeTestSuitId(employeeTestSuiteId, questionId, questionType);
            var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
            testDetails = testDetails ?? new TestDetailsViewModel();
            return testDetails;
        }

        private void CalculateMarks(int employeeTestSuiteId, int? userLastQuestionDetailId, string answer)
        {
            List<EmployeeTestDetails> employeeTestDetails = _testSuiteService.GetEmployeeTestDetailsListByEmployeeTestSuitId(employeeTestSuiteId).ToList();
            foreach (EmployeeTestDetails testDetail in employeeTestDetails)
            {
                if (testDetail.TestDetailId == userLastQuestionDetailId)
                    testDetail.Answer = answer;
                Question question = _questionService.GetSingleQuestion(testDetail.QuestionId);
                if (question.QuestionType == 1)
                {
                    if (!string.IsNullOrWhiteSpace(testDetail.Answer) && question.CorrectAnswer.Trim().Contains(testDetail.Answer.Trim()))
                        testDetail.Mark = question.Marks;
                    else
                        testDetail.Mark = 0;
                }

                _testSuiteService.UpdateEmployeeTestDetails(testDetail);
            }
        }

        private QuestionNavigationViewModel GetNavigationDetails(int EmployeeTestSuiteId)
        {
            var navigationDetailsBusinessModel = _testSuiteService.GetEmployeeNavigationDetails(EmployeeTestSuiteId);
            var navigationDetails = _mappingService.Map<QuestionNavigationBusinessModel, QuestionNavigationViewModel>(navigationDetailsBusinessModel);
            return navigationDetails;
        }
    }
}