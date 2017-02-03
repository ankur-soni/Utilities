using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class ReviewerController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IQuestionService _questionService;
        private readonly IMappingService _mappingService;
        private readonly IUserService _userService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;

        public ReviewerController(IEmailService emailService, IQuestionService questionService, MappingService mappingService, IUserService userService, ITestSuiteService testSuiteService,Silicus.UtilityContainer.Services.Interfaces.IUserService containerUserService)
        {
            _emailService = emailService;
            _questionService = questionService;
            _mappingService = mappingService;
            _userService = userService;
            _testSuiteService = testSuiteService;
            _containerUserService = containerUserService;
        }

        // GET: Reviewer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadQuestion(int userTestSuiteId)
        {
            ReviewerQuestionViewModel testSuiteQuestionModel = ReviewTestSuiteQuestion(null, userTestSuiteId, (int)QuestionType.Practical);
            
            return PartialView("_ReviewerViewQuestion", testSuiteQuestionModel);
        }


        public ActionResult ReviewTest(int UserId, int TestSuiteId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("LogOff", "CandidateAccount");

            var user = _userService.GetUserById(UserId);
            if (user == null)
            {
                ViewBag.Status = 1;
                ViewBag.Msg = "User not found for online test, Kindly contact admin.";
                return View("Welcome");
            }
            UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteByUserId(user.UserId);
            if (userTestSuite == null)
            {
                ViewBag.Status = 1;
                ViewBag.Msg = "Test suite is not assigned for you, Kindly contact admin.";
                return View("Welcome");
            }
            TestSuiteCandidateModel testSuiteCandidateModel = _mappingService.Map<UserTestSuite, TestSuiteCandidateModel>(userTestSuite);
            testSuiteCandidateModel.CandidateInfo = GetCandidateInfo(user);
            testSuiteCandidateModel.NavigationDetails = GetNavigationDetails(testSuiteCandidateModel.UserTestSuiteId);
            testSuiteCandidateModel.TotalQuestionCount = testSuiteCandidateModel.PracticalCount + testSuiteCandidateModel.ObjectiveCount;
            testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.Duration;
            testSuiteCandidateModel.TestSummary = GetTestSummary(testSuiteCandidateModel.UserTestSuiteId);
            return View(testSuiteCandidateModel);
        }

        private TestSummaryViewModel GetTestSummary(int userTestSuiteId)
        {
            var testSummaryBusinessModel = _testSuiteService.GetTestSummary(userTestSuiteId);
            var testSummary = _mappingService.Map<TestSummaryBusinessModel, TestSummaryViewModel>(testSummaryBusinessModel);
            testSummary = testSummary ?? new TestSummaryViewModel();
            return testSummary;
        }

        public ActionResult UpdateReviewAndGetQuestionDetails(QuestionDetailsViewModel questionDetails)
        {
            questionDetails.QuestionType = _testSuiteService.GetQuestionType(questionDetails.QuestionId);
            questionDetails.Answer = HttpUtility.HtmlDecode(questionDetails.Answer);
            UpdateReview(questionDetails.Marks, questionDetails.Comment, questionDetails.UserTestDetailId);
            var reviewerQuestionViewModel = ReviewTestSuiteQuestion(questionDetails.QuestionId, questionDetails.UserTestSuiteId, questionDetails.QuestionType);

            return PartialView("_ReviewerViewQuestion", reviewerQuestionViewModel);
        }

        [HttpPost]
        public JsonResult SumbmitTestReview(QuestionDetailsViewModel questionDetails)
        {

            questionDetails.QuestionType = _testSuiteService.GetQuestionType(questionDetails.QuestionId);
            questionDetails.Answer = HttpUtility.HtmlDecode(questionDetails.Answer);
            UpdateReview(questionDetails.Marks, questionDetails.Comment, questionDetails.UserTestDetailId);
            var IsAllQuestionEvaluated =  _testSuiteService.IsAllQuestionEvaluated(questionDetails.UserTestSuiteId);

           return Json(IsAllQuestionEvaluated);
        }

        [HttpPost]
        public JsonResult SumbmitCandidateResult(CandidateResultViewmodel candidateResultViewmodel)
        {
            var user = _userService.GetUserById(candidateResultViewmodel.CandidateUserId);
            user.CandidateStatus = candidateResultViewmodel.Status.ToString();
            _userService.Update(user);
            var userTestSuitedetails = _testSuiteService.GetUserTestSuiteByUserId(candidateResultViewmodel.CandidateUserId);
            userTestSuitedetails.StatusId = (int)candidateResultViewmodel.Status;
            userTestSuitedetails.FeedBack = candidateResultViewmodel.ReviewerComment;
            _testSuiteService.UpdateUserTestSuite(userTestSuitedetails);
            return Json(true);
        }



        #region private

        private ReviewerQuestionViewModel ReviewTestSuiteQuestion(int? questionId, int? userTestSuiteId, int questionType)
        {
            TestDetailsBusinessModel userTestDetails = _testSuiteService.GetUserTestDetailsByUserTestSuitId(userTestSuiteId, questionId, questionType);
            var testDetails = _mappingService.Map<TestDetailsBusinessModel, ReviewerQuestionViewModel>(userTestDetails);
            testDetails = testDetails ?? new ReviewerQuestionViewModel();
            return testDetails;
        }
        private QuestionNavigationViewModel GetNavigationDetails(int userTestSuiteId)
        {
            var navigationDetailsBusinessModel = _testSuiteService.GetNavigationDetails(userTestSuiteId);
            var navigationDetails = _mappingService.Map<QuestionNavigationBusinessModel, QuestionNavigationViewModel>(navigationDetailsBusinessModel);
            return navigationDetails;
        }

        private CandidateInfoViewModel GetCandidateInfo(UserBusinessModel user)
        {
            return new CandidateInfoViewModel
            {
                Name = user.FirstName + " " + user.LastName,
                DOB = user.DOB,
                RequisitionId = user.RequisitionId,
                Position = user.Position,
                TotalExperience = ConvertExperienceIntoDecimal(user.TotalExperienceInYear, user.TotalExperienceInMonth)
            };
        }
        private decimal ConvertExperienceIntoDecimal(int totalExperienceInYear, int totalExperienceInMonth)
        {
            if (totalExperienceInMonth > 0)
            {
                return totalExperienceInYear + (decimal)(totalExperienceInMonth / 12.0);
            }
            else
                return totalExperienceInYear;
        }

        private void UpdateReview(int mark, string comment, int? userTestDetailId)
        {
            UserTestDetails userTestDetails = _testSuiteService.GetUserTestDetailsId(userTestDetailId);
            userTestDetails.Mark = mark;
            
            Silicus.UtilityContainer.Models.DataObjects.User user= _containerUserService.FindUserByEmail(HttpContext.User.Identity.Name);
            if(user!=null)
            {
                userTestDetails.MarkGivenBy = user.ID;
                userTestDetails.MarkGivenByName = user.DisplayName;
            }
            userTestDetails.MarkGivenDate = DateTime.Now;
            userTestDetails.ReviwerComment = comment;
            _testSuiteService.UpdateUserTestDetails(userTestDetails);
        }



        #endregion
    }
}