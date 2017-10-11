using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Web.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Silicus.Ensure.Web.Models.Employee;
using Silicus.Ensure.Web.Filters;

namespace Silicus.Ensure.Web.Controllers
{
    [CustomAuthorize("Admin", "Panel", "Employee", "Recruiter")]
    public class EmployeeController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        private readonly IMappingService _mappingService;
        private Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService _utilityUserRoleService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly IUserService _userService;
        private readonly CommonController _commonController;
        //private readonly IPositionService _positionService;
        private readonly ITagsService _tagsService;

        public EmployeeController(IQuestionService questionService, MappingService mappingService, UtilityContainer.Services.Interfaces.IUserService containerUserService, Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService utilityUserRoleService, ITestSuiteService testSuiteService, IUserService userService, CommonController commonController,  ITagsService tagsService)
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
           // _positionService = positionService;
            _tagsService = tagsService;
        }
        // GET: Employee
        [CustomAuthorize("Admin", "Recruiter")]
        public ActionResult Index()
        {
            var model = new EmployeeTestSuitAssignementViewmodel();
            model.EmployeeList = GetUserDetails();

            model.TestSuitList = _testSuiteService.GetTestSuiteDetails()
                              .Where(m => m.IsDeleted == false && m.IsExternal == false).OrderByDescending(m => m.TestSuiteId);

            return View("EmployeeList", model);
            //return View("AssigedTest");
        }

        [CustomAuthorize("Admin", "Panel", "Employee", "Recruiter")]
        public ActionResult AssignedTest()
        {
            //return View("EmployeeList");
            return View("AssigedTest");
        }

        [CustomAuthorize("Admin", "Recruiter")]
        public ActionResult MarkDashboard()
        {
            //return View("EmployeeList");
            return View("EmployeeMarks");
        }

        [CustomAuthorize("Admin", "Recruiter")]
        public ActionResult GetEmployeeassigedforTestSuits(int suitId)
        {
            var TestSuits = _testSuiteService.GetEmployeeTestSuite().Where(ts => ts.TestSuiteId == suitId && ts.StatusId == 2).ToList();
            var empList = TestSuits.Select(ts => ts.EmployeeId).Distinct().ToList<int>();
            var reviewers = TestSuits.Count > 0 ? TestSuits.FirstOrDefault().AssignedReviewers : "";


            return Json(new { employeeList= empList, reviewer= reviewers }, JsonRequestBehavior.AllowGet);
        }

        private int GetUtilityId()
        {
            var utilityProductId = WebConfigurationManager.AppSettings["ProductId"];
            if (string.IsNullOrWhiteSpace(utilityProductId))
            {
                throw new ArgumentNullException();
            }

            return Convert.ToInt32(utilityProductId);
        }

        [CustomAuthorize("Admin", "Recruiter")]
        public ActionResult GetTestSuitResult([DataSourceRequest] DataSourceRequest request)
        {
            var userlist = _containerUserService.GetAllUsers();
            var testSuitelist = _testSuiteService.GetTestSuiteDetails().Where(model => model.IsDeleted == false && model.IsExternal == false);
            var TestSuits = _testSuiteService.GetEmployeeTestSuite();

            var TestResults = (from tests in TestSuits
                               join users in userlist
                               on tests.EmployeeId equals users.ID
                               join testDetail in testSuitelist
                                on tests.TestSuiteId equals testDetail.TestSuiteId
                               select new EmployeeTestResultViewModel
                               {
                                   UserId = users.ID,
                                   EmployeeId = users.EmployeeID,
                                   EmpName = users.DisplayName,
                                   MaxScore = tests.MaxScore,
                                   MarksObtained = tests.EmployeeTestDetails.Select(x => x.Mark).Sum(),
                                   TestSuitId = tests.TestSuiteId,
                                   TestSuitName = testDetail.TestSuiteName,
                                   AttemptDate = tests.AttemptDate,
                                   StatusId = tests.StatusId
                               }).ToList();

            DataSourceResult result = TestResults.ToDataSourceResult(request);
            return Json(result);

        }

        [CustomAuthorize("Admin", "Recruiter")]
        private List<SelectListItem> GetUserDetails()
        {
            var userlist = _containerUserService.GetAllUsers();
            var userlistViewModel = _mappingService.Map<List<Silicus.UtilityContainer.Models.DataObjects.User>, List<UserDetailViewModel>>(userlist).Distinct();
            var UtilityId = GetUtilityId();
            var userRoles = _utilityUserRoleService.GetAllUserRolesForUtility(UtilityId);

            //var userWithRoles = (from userinRoles in userRoles
            //                     join allUsers in userlistViewModel
            //                     on userinRoles.UserId equals allUsers.UserId
            //                     where userinRoles.IsActive && userinRoles.UtilityId == UtilityId
            //                     select new SelectListItem
            //                     {
            //                         Text = allUsers.FullName,
            //                         Value = allUsers.UserId.ToString()
            //                     }).OrderBy(t => t.Text).Distinct<SelectListItem>().ToList();

            var userWithRoles = (from userinRoles in userRoles
                                 join allUsers in userlistViewModel
                                 on userinRoles.UserId equals allUsers.UserId
                                 where userinRoles.IsActive && userinRoles.UtilityId == UtilityId
                                 select new
                                 {
                                     Name = allUsers.FullName,
                                     Id = allUsers.UserId.ToString()
                                 }).ToList();

            var userList = userWithRoles.Distinct().Select(x=> new SelectListItem
                                 {
                                     Text = x.Name,
                                     Value = x.Id
                                 }).OrderBy(t => t.Text).Distinct<SelectListItem>().ToList();

            return userList;
        }

        public ActionResult GetEmployeeTestSuits([DataSourceRequest] DataSourceRequest request)
        {
            var userEmailId = User.Identity.Name;
            var user = _containerUserService.FindUserByEmail(userEmailId);

            var empTestSuits = _testSuiteService.GetEmployeeTestSuite(user.ID);
            var testSuits = _testSuiteService.GetTestSuiteDetails();
            
            var TestResults = (from tests in empTestSuits
                               join masterTest in testSuits
                                on tests.TestSuiteId equals masterTest.TestSuiteId
                               select new Models.Employee.EmployeeTestSuitViewModel
                               {
                                   EmployeeTestSuiteId = tests.EmployeeTestSuiteId,
                                   TestSuiteId = tests.TestSuiteId,
                                   TestSuitName = masterTest.TestSuiteName,
                                   EmployeeId = tests.EmployeeId,
                                   MarksObtained = tests.EmployeeTestDetails.Select(x => x.Mark).Sum(),
                                   ObjectiveCount = tests.ObjectiveCount,
                                   PracticalCount = tests.PracticalCount,
                                   Duration = tests.Duration,
                                   TotalQuestionCount = tests.ObjectiveCount + tests.PracticalCount,
                                   AttemptDate = tests.AttemptDate,
                                   StatusId = (CandidateStatus)tests.StatusId
                               }).ToList();



            DataSourceResult result = TestResults.ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult EmployeeSuit(int UserId, int IsReassign = 0)
        {
            ViewBag.CurrentUser = UserId;
            ViewBag.IsReassign = IsReassign;
            return PartialView("SelectEmployeeSuit");
        }

        //public ActionResult OnlineTest(int EmployeeTestSuitId)
        //{
        //    var userEmailId = User.Identity.Name;
        //    var user = _containerUserService.FindUserByEmail(userEmailId);
        //    EmployeeTestSuite employeeTestSuite = _testSuiteService.GetEmployeeTestSuiteById(EmployeeTestSuitId);

        //    if (employeeTestSuite == null)
        //    {
        //        ViewBag.Status = 1;
        //        ViewBag.Msg = "No test is assigned for you, kindly contact admin.";
        //        return View("Welcome", new TestSuiteEmployeeModel());
        //    }
        //    else if (employeeTestSuite.EmployeeId != user.ID)
        //    {
        //        return RedirectToAction("AssignedTest", "Employee");
        //    }
        //    else if (employeeTestSuite.StatusId != (int)CandidateStatus.TestAssigned)
        //    {
        //        ViewBag.Status = 1;
        //        ViewBag.Msg = "You have already submitted your test.";
        //        return View("Welcome", new TestSuiteEmployeeModel());
        //    }
        //    TestSuiteEmployeeModel testSuiteEmployeeModel = _mappingService.Map<EmployeeTestSuite, TestSuiteEmployeeModel>(employeeTestSuite);
        //    testSuiteEmployeeModel.NavigationDetails = GetNavigationDetails(testSuiteEmployeeModel.EmployeeTestSuiteId);
        //    testSuiteEmployeeModel.TotalQuestionCount = testSuiteEmployeeModel.PracticalCount + testSuiteEmployeeModel.ObjectiveCount;
        //    testSuiteEmployeeModel.DurationInMin = testSuiteEmployeeModel.RemainingTime > 0 ? testSuiteEmployeeModel.RemainingTime : testSuiteEmployeeModel.Duration;
        //    testSuiteEmployeeModel.EmployeeId = user.ID;

        //    return View(testSuiteEmployeeModel);
        //}

        //[HttpPost]
        //public JsonResult UpdateTimeCounter(int time, int employeeTestSuiteId)
        //{
        //    EmployeeTestSuite employeeTestSuite = _testSuiteService.GetEmployeeTestSuiteById(employeeTestSuiteId);
        //    if (employeeTestSuite != null)
        //    {
        //        int remainingTime = time / 60;
        //        employeeTestSuite.RemainingTime = remainingTime;
        //        _testSuiteService.UpdateEmployeeTestSuite(employeeTestSuite);
        //    }
        //    return Json(1);
        //}

        //public ActionResult LoadQuestion(int employeeTestSuiteId)
        //{
        //    TestDetailsViewModel testSuiteQuestionModel = TestSuiteQuestion(null, employeeTestSuiteId, (int)QuestionType.Objective);
        //    return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        //}

        //public ActionResult GetQuestionDetails(QuestionDetailsViewModel questionDetails)
        //{
        //    questionDetails.QuestionType = _testSuiteService.GetQuestionType(questionDetails.QuestionId);
        //    questionDetails.Answer = HttpUtility.UrlDecode(questionDetails.Answer);
        //    UpdateAnswer(questionDetails.Answer, questionDetails.UserTestDetailId);
        //    var testSuiteQuestionModel = TestSuiteQuestion(questionDetails.QuestionId, questionDetails.UserTestSuiteId, questionDetails.QuestionType);
        //    ModelState.Clear();
        //    return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        //}


        //public ActionResult OnSubmitTest(int testSuiteId, int EmployeeTestSuiteId, int? employeeTestDetailId, int EmployeeId, string answer)
        //{
        //    // Update last question answer of test.
        //    answer = HttpUtility.HtmlDecode(answer);
        //    //_userService.UpdateUserApplicationTestDetails(userId);

        //    // Update total time utilization for test back to UserTestSuite.
        //    TestSuite suite = _testSuiteService.GetTestSuitById(testSuiteId);
        //    EmployeeTestSuite testSuit = _testSuiteService.GetEmployeeTestSuiteById(EmployeeTestSuiteId);
        //    testSuit.Duration = suite.Duration + (testSuit.ExtraCount * 10);
        //    testSuit.StatusId = Convert.ToInt32(CandidateStatus.TestSubmitted);
        //    testSuit.AttemptDate = DateTime.Now;
        //    _testSuiteService.UpdateEmployeeTestSuite(testSuit);

        //    // Calculate marks on test submit.
        //    CalculateMarks(EmployeeTestSuiteId, employeeTestDetailId, answer);
        //    //List<string> Receipient = new List<string>() { "Admin", "Panel" };
        //    //var users = _userService.GetUserApplicationDetailsById(userId);
        //    //if (users != null)
        //    //{
        //    //    var userDetails = _userService.GetUserById(users.UserId);
        //    //    //   _commonController.SendMailByRoleName("Online Test Submitted For " + userDetails.FirstName + " " + userDetails.LastName + "", "CandidateTestSubmitted.cshtml", Receipient, userDetails.FirstName + " " + userDetails.LastName);
        //    //}
        //    return RedirectToAction("AssignedTest", "Employee");
        //}

        //private void UpdateAnswer(string answer, int? employeeTestDetailId)
        //{
        //    EmployeeTestDetails employeeTestDetails = _testSuiteService.GetEmployeeTestDetailsId(employeeTestDetailId.Value);
        //    employeeTestDetails.Answer = answer;
        //    _testSuiteService.UpdateEmployeeTestDetails(employeeTestDetails);
        //}

        //private TestDetailsViewModel TestSuiteQuestion(int? questionId, int? employeeTestSuiteId, int questionType)
        //{
        //    TestDetailsBusinessModel userTestDetails = _testSuiteService.GeEmployeeTestDetailsByEmployeeTestSuitId(employeeTestSuiteId, questionId, questionType);
        //    var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
        //    testDetails = testDetails ?? new TestDetailsViewModel();
        //    return testDetails;
        //}

        //private void CalculateMarks(int employeeTestSuiteId, int? userLastQuestionDetailId, string answer)
        //{
        //    List<EmployeeTestDetails> employeeTestDetails = _testSuiteService.GetEmployeeTestDetailsListByEmployeeTestSuitId(employeeTestSuiteId).ToList();
        //    foreach (EmployeeTestDetails testDetail in employeeTestDetails)
        //    {
        //        if (testDetail.TestDetailId == userLastQuestionDetailId)
        //            testDetail.Answer = answer;
        //        Question question = _questionService.GetSingleQuestion(testDetail.QuestionId);
        //        if (question.QuestionType == 1)
        //        {
        //            if (!string.IsNullOrWhiteSpace(testDetail.Answer) && question.CorrectAnswer.Trim().Contains(testDetail.Answer.Trim()))
        //                testDetail.Mark = question.Marks;
        //            else
        //                testDetail.Mark = 0;
        //        }

        //        _testSuiteService.UpdateEmployeeTestDetails(testDetail);
        //    }
        //}

        //private QuestionNavigationViewModel GetNavigationDetails(int EmployeeTestSuiteId)
        //{
        //    var navigationDetailsBusinessModel = _testSuiteService.GetEmployeeNavigationDetails(EmployeeTestSuiteId);
        //    var navigationDetails = _mappingService.Map<QuestionNavigationBusinessModel, QuestionNavigationViewModel>(navigationDetailsBusinessModel);
        //    return navigationDetails;
        //}

        [HttpPost]
        [CustomAuthorize("Admin", "Recruiter")]
        public ActionResult AssignEmployeeSuite(int SuiteId, List<int> UserList,string reviewerId, int IsReAssign = 0)
        {
            // string mailsubject = "";
            //var updateCurrentUsers = _userService.GetUserById(UserId);
            //if (updateCurrentUsers != null)
            //{
            //if (SuiteId > 0 && UserId > 0)
            //{
            //    //Need confirmation for TestStatus - TBD
            //    //if (IsReAssign == 1)
            //    //{
            var employeeTestSuits = _testSuiteService.GetEmployeeTestSuite().Where(x => x.TestSuiteId == SuiteId && x.StatusId == Convert.ToInt32(CandidateStatus.TestAssigned)).ToList();

            if (employeeTestSuits.Any())
            {
                foreach (var empTestSuite in employeeTestSuits)
                {
                    _testSuiteService.DeleteEmployeeTestSuite(empTestSuite);
                }
            }
            //}

            foreach (var UserId in UserList.Distinct())
            {
                var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == SuiteId && model.IsDeleted == false).SingleOrDefault();
                EmployeeTestSuite userTestSuite = new EmployeeTestSuite();
                userTestSuite.EmployeeId = UserId;
                userTestSuite.TestSuiteId = SuiteId;
                userTestSuite.CandidateID = "0";
                userTestSuite.AssignedReviewers = reviewerId;
                userTestSuite.StatusId = (int)CandidateStatus.TestAssigned;
                _testSuiteService.AssignEmployeeSuite(userTestSuite, testSuiteDetails);
            }
            //List<string> Receipient = new List<string>() { "Admin", "Panel" };
            //Need to work on following to configure email
            //if (IsReAssign == 0)
            //{
            //    mailsubject = "Test Assigned For " + selectUser.FirstName + " " + selectUser.LastName + " Successfully";
            //    _commonController.SendMailByRoleName(mailsubject, "CandidateTestAssigned.cshtml", Receipient, selectUser.FirstName + " " + selectUser.LastName);
            //}
            //else
            //{
            //    mailsubject = "Test Re-Assigned For " + selectUser.FirstName + " " + selectUser.LastName + " Successfully";
            //    _commonController.SendMailByRoleName(mailsubject, "TestReassign.cshtml", Receipient, selectUser.FirstName + " " + selectUser.LastName);
            //}

            return Json(1);
            //}
            //else
            //{
            //    return Json(-1);
            //}
            //}
            //return View();
        }

        //public ActionResult GetEmployeeTestSuiteDetails([DataSourceRequest] DataSourceRequest request)
        //{
        //    //_testSuiteService.TestSuiteActivation();
        //    //var tags = _tagsService.GetTagsDetails();
        //    var testSuitelist = _testSuiteService.GetTestSuiteDetails()
        //        .Where(model => model.IsDeleted == false && model.IsExternal == false).OrderByDescending(model => model.TestSuiteId).ToArray();
        //    var viewModels = _mappingService.Map<TestSuite[], TestSuiteViewModel[]>(testSuitelist);
        //    //bool userInRole = MvcApplication.getCurrentUserRoles().Contains((Silicus.Ensure.Models.Constants.RoleName.Admin.ToString()));
        //    //var testSuitId = _testSuiteService.GetEmployeeTestSuiteByEmployeeId(UserId);
        //    //foreach (var item in viewModels)
        //    //{
        //    //    if (testSuitId != null)
        //    //    {
        //    //        if (testSuitId.TestSuiteId != 0 && item.TestSuiteId == testSuitId.TestSuiteId)
        //    //        {
        //    //            item.IsAssigned = true;
        //    //        }
        //    //    }
        //    //    if (item.Position.HasValue)
        //    //    {
        //    //        item.PositionName = GetPosition((int)item.Position) == null ? "deleted from master" : GetPosition((int)item.Position).PositionName;
        //    //    }
        //    //    else
        //    //    {
        //    //        item.PositionName = "Not assigned";
        //    //    }
        //    //    List<Int32> TagId = item.PrimaryTags.Split(',').Select(int.Parse).ToList();
        //    //    item.PrimaryTagNames = string.Join(",", (from a in tags
        //    //                                             where TagId.Contains(a.TagId)
        //    //                                             select a.TagName));
        //    //    item.StatusName = ((TestSuiteStatus)item.Status).ToString();
        //    //    item.UserInRole = userInRole;
        //    //}
        //    return Json(viewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        //private Position GetPosition(int positionId)
        //{
        //    return _positionService.GetPositionById(positionId);
        //}
    }
}