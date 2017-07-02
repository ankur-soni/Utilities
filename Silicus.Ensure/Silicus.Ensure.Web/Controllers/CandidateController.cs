using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.JobVite;
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
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Silicus.Ensure.Web.Models.JobVite;
using Microsoft.AspNet.Identity;

namespace Silicus.Ensure.Web.Controllers
{
    [CustomAuthorize("Candidate")]
    [CandidateAttribute]
    public class CandidateController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IMappingService _mappingService;
        private readonly IUserService _userService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly CommonController _commonController;
        public CandidateController(IQuestionService questionService, MappingService mappingService, IUserService userService, ITestSuiteService testSuiteService, CommonController commonController)
        {
            _questionService = questionService;
            _mappingService = mappingService;
            _userService = userService;
            _testSuiteService = testSuiteService;
            _commonController = commonController;
        }

        [CustomAuthorize("Candidate")]
        public ActionResult Welcome()
        {
            if (!ModelState.IsValid)
                return RedirectToAction("LogOff", "CandidateAccount");
            var userEmail = User.Identity.Name.Trim();
            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                //var user = _userService.GetUserByEmail(userEmail);
                //if (user == null)
                //    return RedirectToAction("LogOff", "CandidateAccount");
                //else if (user.CandidateStatus == CandidateStatus.TestSubmitted.ToString())
                //{
                //    ViewBag.Status = 1;
                //    ViewBag.Msg = "You have already submitted your test.";
                //    return View("Welcome", new TestSuiteCandidateModel());
                //}
                // ViewBag.CandidateName = user.FirstName;
                var userTestSuite = _testSuiteService.GetUserTestSuite(User.Identity.GetUserId());
                if (userTestSuite == null)
                {
                    ViewBag.Status = 1;
                    ViewBag.Msg = "No test is assigned for you, kindly contact admin.";
                    return View("Welcome", new TestSuiteEmployeeModel());
                }

                return View("Welcome", new TestSuiteEmployeeModel() { EmployeeTestSuiteId = userTestSuite.EmployeeTestSuiteId, CandidateId = User.Identity.GetUserId().ToString(), EmployeeId =0 });

                //// ViewBag.ProfilePhotoPath = user.ProfilePhotoFilePath;
                // ViewBag.Status = 0;

                //return RedirectToAction("OnlineTest","Test", new { EmployeeTestSuitId = userTestSuite.EmployeeTestSuiteId, employeeId = 0, CandidateId = User.Identity.GetUserId().ToString() });


            }
            return View();
        }

        //[CustomAuthorize("Candidate")]
        //public ActionResult TestSuiteAndCandidateDetails()
        //{
        //    var userEmail = User.Identity.Name.Trim();
        //    //var user = null;//_userService.GetUserByEmail(userEmail);
        //    //if (user == null)
        //    //    return RedirectToAction("LogOff", "CandidateAccount");

        //    UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteByUserApplicationId(0);
        //    if (userTestSuite != null)
        //    {
        //        if (userTestSuite.RemainingTime > 0)
        //        {
        //            userTestSuite.Duration = userTestSuite.RemainingTime;
        //        }
        //        else
        //        {
        //            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().SingleOrDefault(model => model.TestSuiteId == userTestSuite.TestSuiteId && !model.IsDeleted);
        //            _testSuiteService.AssignSuite(userTestSuite, testSuiteDetails);
        //        }
        //    }

        //    TestSuiteCandidateModel testSuiteCandidateModel = _mappingService.Map<UserTestSuite, TestSuiteCandidateModel>(userTestSuite);
        //    testSuiteCandidateModel = testSuiteCandidateModel ?? new TestSuiteCandidateModel();
        //  //  var candidateInfoBusinessModel = _userService.GetCandidateInfo(user);
        //    //testSuiteCandidateModel.CandidateInfo = _mappingService.Map<CandidateInfoBusinessModel, CandidateInfoViewModel>(candidateInfoBusinessModel);

        //    //testSuiteCandidateModel.NavigationDetails = GetNavigationDetails(testSuiteCandidateModel.UserTestSuiteId);
        //    //testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.RemainingTime > 0 ? testSuiteCandidateModel.RemainingTime : testSuiteCandidateModel.Duration;

        //    return PartialView("_testSuiteAndCandidateDetails", null);
        //}

        [CustomAuthorize("Candidate")]
        public ActionResult ReadOnlyInstructions()
        {
            //if (!ModelState.IsValid)
            //    return RedirectToAction("LogOff", "CandidateAccount");
            //var userEmail = User.Identity.Name.Trim();
            //var user = _userService.GetUserByEmail(userEmail);
            //if (user == null)
            //    return RedirectToAction("LogOff", "CandidateAccount");
            return PartialView("ReadOnlyInstructions");
        }

        //[CustomAuthorize("Admin", "Panel", "Recruiter", "Candidate")]
        //public ActionResult OnlineTest()
        //{
        //    if (!ModelState.IsValid)
        //        return RedirectToAction("LogOff", "CandidateAccount");

        //    var userEmail = User.Identity.Name.Trim();
        //    //var user = _userService.GetUserByEmail(userEmail);
        //    //if (user == null)
        //    //{
        //    //    ViewBag.Status = 1;
        //    //    ViewBag.Msg = "User not found for online test, kindly contact admin.";
        //    //    return View("Welcome", new TestSuiteCandidateModel());
        //    //}
        //    //UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteByUserApplicationId(user.UserApplicationId);
        //    //if (userTestSuite == null)
        //    //{
        //    //    ViewBag.Status = 1;
        //    //    ViewBag.Msg = "No test is assigned for you, kindly contact admin.";
        //    //    return View("Welcome", new TestSuiteCandidateModel());
        //    //}
        //    //else if (user.CandidateStatus != CandidateStatus.TestAssigned.ToString())
        //    //{
        //    //    ViewBag.Status = 1;
        //    //    ViewBag.Msg = "You have already submitted your test.";
        //    //    return View("Welcome", new TestSuiteCandidateModel());
        //    //}
        //    //TestSuiteCandidateModel testSuiteCandidateModel = _mappingService.Map<UserTestSuite, TestSuiteCandidateModel>(userTestSuite);
        //    //var candidateInfoBusinessModel = _userService.GetCandidateInfo(user);
        //    //testSuiteCandidateModel.CandidateInfo = _mappingService.Map<CandidateInfoBusinessModel, CandidateInfoViewModel>(candidateInfoBusinessModel);
        //    //testSuiteCandidateModel.ProfilePhotoFilePath = user.ProfilePhotoFilePath;
        //    //testSuiteCandidateModel.NavigationDetails = GetNavigationDetails(testSuiteCandidateModel.UserTestSuiteId);
        //    //testSuiteCandidateModel.TotalQuestionCount = testSuiteCandidateModel.PracticalCount + testSuiteCandidateModel.ObjectiveCount;
        //    //testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.RemainingTime > 0 ? testSuiteCandidateModel.RemainingTime : testSuiteCandidateModel.Duration;
        //    //testSuiteCandidateModel.UserId = user.UserApplicationId;
        //    return View();
        //}

        //private QuestionNavigationViewModel GetNavigationDetails(int userTestSuiteId)
        //{
        //    var navigationDetailsBusinessModel = _testSuiteService.GetNavigationDetails(userTestSuiteId);
        //    var navigationDetails = _mappingService.Map<QuestionNavigationBusinessModel, QuestionNavigationViewModel>(navigationDetailsBusinessModel);
        //    return navigationDetails;
        //}

        //public ActionResult LoadQuestion(int userTestSuiteId)
        //{
        //    TestDetailsViewModel testSuiteQuestionModel = TestSuiteQuestion(null, userTestSuiteId, (int)QuestionType.Objective);
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



        //public ActionResult OnSubmitTest(int testSuiteId, int userTestSuiteId, int? userTestDetailId, int userId, string answer)
        //{
        //    // Update last question answer of test.
        //    answer = HttpUtility.HtmlDecode(answer);
        //    //_userService.UpdateUserApplicationTestDetails(userId);

        //    // Update total time utilization for test back to UserTestSuite.
        //    TestSuite suite = _testSuiteService.GetTestSuitById(testSuiteId);
        //    UserTestSuite testSuit = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
        //    testSuit.Duration = suite.Duration + (testSuit.ExtraCount * 10);
        //    testSuit.StatusId = Convert.ToInt32(CandidateStatus.TestSubmitted);
        //    _testSuiteService.UpdateUserTestSuite(testSuit);

        //    // Calculate marks on test submit.
        //    CalculateMarks(userTestSuiteId, userTestDetailId, answer);
        //    List<string> Receipient = new List<string>() { "Admin", "Panel" };
        //    //var users = _userService.GetUserApplicationDetailsById(userId);
        //    //if (users != null)
        //    //{
        //    //    var userDetails = _userService.GetUserById(users.UserId);
        //    //    //   _commonController.SendMailByRoleName("Online Test Submitted For " + userDetails.FirstName + " " + userDetails.LastName + "", "CandidateTestSubmitted.cshtml", Receipient, userDetails.FirstName + " " + userDetails.LastName);
        //    //}
        //    return RedirectToAction("LogOff", "CandidateAccount");
        //}

        //[HttpPost]
        //public JsonResult AddMoreTime(int count, int userTestSuiteId)
        //{
        //    count = count + 1;
        //    UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
        //    userTestSuite.ExtraCount = count;
        //    _testSuiteService.UpdateUserTestSuite(userTestSuite);
        //    return Json(count);
        //}


        //[HttpPost]
        //public JsonResult UpdateTimeCounter(int time, int userTestSuiteId)
        //{
        //    UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
        //    if (userTestSuite != null)
        //    {
        //        int remainingTime = time / 60;
        //        userTestSuite.RemainingTime = remainingTime;
        //        _testSuiteService.UpdateUserTestSuite(userTestSuite);
        //    }
        //    return Json(1);
        //}

        //private void UpdateAnswer(string answer, int? userTestDetailId)
        //{
        //    UserTestDetails userTestDetails = _testSuiteService.GetUserTestDetailsId(userTestDetailId);
        //    userTestDetails.Answer = answer;
        //    _testSuiteService.UpdateUserTestDetails(userTestDetails);
        //}

        //private TestDetailsViewModel TestSuiteQuestion(int? questionId, int? userTestSuiteId, int questionType)
        //{
        //    TestDetailsBusinessModel userTestDetails = _testSuiteService.GetUserTestDetailsByUserTestSuitId(userTestSuiteId, questionId, questionType);
        //    var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
        //    testDetails = testDetails ?? new TestDetailsViewModel();
        //    return testDetails;
        //}

        //private void CalculateMarks(int userTestSuiteId, int? userLastQuestionDetailId, string answer)
        //{
        //    List<UserTestDetails> userTestDetails = _testSuiteService.GetUserTestDetailsListByUserTestSuitId(userTestSuiteId).ToList();
        //    foreach (UserTestDetails testDetail in userTestDetails)
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

        //        _testSuiteService.UpdateUserTestDetails(testDetail);
        //    }
        //}

        #region Assign Test
        //public ActionResult AssignTest()
        //{
        //    return PartialView("_AssignTest");
        //}

        //public JsonResult GetCandidatesByRequisition([DataSourceRequest] DataSourceRequest request, int requisitionId)
        //{
        //    var candidatesBusinessModel = _userService.GetCandidatesFromJobVite();
        //    var candidatesViewModel = _mappingService.Map<List<JobViteCandidateBusinessModel>, List<JobViteCandidateViewModel>>(candidatesBusinessModel);
        //    foreach (var candidate in candidatesViewModel)
        //    {
        //        candidate.CandidateJson = "";
        //        candidate.CandidateJson = Newtonsoft.Json.JsonConvert.SerializeObject(candidate);
        //    }
        //    return Json(candidatesViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetAllRequisitions([DataSourceRequest] DataSourceRequest request)
        //{
        //    var requistionsBusinessModel = _userService.GetAllRequistions();
        //    var requistionsViewModel = _mappingService.Map<List<RequisitionBusinessModel>, List<RequisitionViewModel>>(requistionsBusinessModel);
        //    return Json(requistionsViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult SaveCandidateAndAssignTest(AssignTestViewModel assignTestViewModel)
        //{
        //    var assignTestBusinessModel = _mappingService.Map<AssignTestViewModel, AssignTestBusinessModel>(assignTestViewModel);
        //    //var candidateBusinessModel=new US
        //    return PartialView("_AssignTestFormElements", new AssignTestBusinessModel());
        //}
        #endregion
    }
}