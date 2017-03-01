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
        private readonly CommonController _commonController;

        public ReviewerController(IEmailService emailService, IQuestionService questionService, MappingService mappingService, IUserService userService, ITestSuiteService testSuiteService, Silicus.UtilityContainer.Services.Interfaces.IUserService containerUserService, CommonController commonController)
        {
            _emailService = emailService;
            _questionService = questionService;
            _mappingService = mappingService;
            _userService = userService;
            _testSuiteService = testSuiteService;
            _containerUserService = containerUserService;
            _commonController = commonController;
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


        public ActionResult ReviewTest(int UserApplicationId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("LogOff", "CandidateAccount");

            var user = _userService.GetUserByUserApplicationId(UserApplicationId);
            if (user == null)
            {
                ViewBag.Status = 1;
                ViewBag.Msg = "User not found for online test, Kindly contact admin.";
                return View("Welcome");
            }
            UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteByUserApplicationId(UserApplicationId);
            if (userTestSuite == null)
            {
                ViewBag.Status = 1;
                ViewBag.Msg = "Test suite is not assigned for you, Kindly contact admin.";
                return View("Welcome");
            }
            TestSuiteCandidateModel testSuiteCandidateModel = _mappingService.Map<UserTestSuite, TestSuiteCandidateModel>(userTestSuite);
            var candidateInfoBusinessModel = _userService.GetCandidateInfo(user);
            testSuiteCandidateModel.CandidateInfo = _mappingService.Map<CandidateInfoBusinessModel, CandidateInfoViewModel>(candidateInfoBusinessModel);
            testSuiteCandidateModel.NavigationDetails = GetNavigationDetails(testSuiteCandidateModel.UserTestSuiteId);
            testSuiteCandidateModel.TotalQuestionCount = testSuiteCandidateModel.PracticalCount + testSuiteCandidateModel.ObjectiveCount;
            testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.Duration;
            testSuiteCandidateModel.TestSummary = GetTestSummary(testSuiteCandidateModel.UserTestSuiteId);
            testSuiteCandidateModel.UserId = user.UserId;
            CandidateStatus status;
            if (Enum.TryParse(user.CandidateStatus, out status))
            {
                testSuiteCandidateModel.CandidateStatus = status;
            }
            return View(testSuiteCandidateModel);
        }

        public ActionResult LoadTestSummaryView(int UserTestSuiteId)
        {
            var testSummary = GetTestSummary(UserTestSuiteId);
            return PartialView("_TestSummary", testSummary);
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
            var reviewerQuestionViewModel = ReviewTestSuiteQuestion(questionDetails.QuestionId, questionDetails.UserTestSuiteId, questionDetails.QuestionType);
            if (questionDetails.QuestionType == (int)QuestionType.Practical)
            {
                UpdateReview(questionDetails.Marks, questionDetails.Comment, questionDetails.UserTestDetailId);
            }
            if (reviewerQuestionViewModel.QuestionType == 1 && reviewerQuestionViewModel.AnswerType == 2)
            {
                reviewerQuestionViewModel = GetAnswerDetails(reviewerQuestionViewModel);
            }
            return PartialView("_ReviewerViewQuestion", reviewerQuestionViewModel);
        }


        [HttpPost]
        public JsonResult SumbmitTestReview(QuestionDetailsViewModel questionDetails)
        {

            questionDetails.QuestionType = _testSuiteService.GetQuestionType(questionDetails.QuestionId);
            questionDetails.Answer = HttpUtility.HtmlDecode(questionDetails.Answer);
            UpdateReview(questionDetails.Marks, questionDetails.Comment, questionDetails.UserTestDetailId);
            var IsAllQuestionEvaluated = _testSuiteService.IsAllQuestionEvaluated(questionDetails.UserTestSuiteId);

            return Json(IsAllQuestionEvaluated);
        }

        [HttpPost]
        public JsonResult SumbmitCandidateResult(CandidateResultViewmodel candidateResultViewmodel)
        {
            var user = _userService.GetUserById(candidateResultViewmodel.CandidateUserId);
            user.CandidateStatus = candidateResultViewmodel.Status.ToString();
            _userService.Update(user);
            var userTestSuitedetails = _testSuiteService.GetUserTestSuiteId(candidateResultViewmodel.UserTestSuiteId);
            userTestSuitedetails.StatusId = (int)candidateResultViewmodel.Status;
            userTestSuitedetails.FeedBack = candidateResultViewmodel.ReviewerComment;
            _testSuiteService.UpdateUserTestSuite(userTestSuitedetails);

            List<string> Receipient = new List<string>() { "Admin", "Panel" };
            _commonController.SendMailByRoleName("Evaluation is submitted for " + user.FirstName + " " + user.LastName + " Successfully", "EvaluationSubmittedMail.cshtml", Receipient, user.FirstName + " " + user.LastName, candidateResultViewmodel.Status.ToString());

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

        private void UpdateReview(int mark, string comment, int? userTestDetailId)
        {
            UserTestDetails userTestDetails = _testSuiteService.GetUserTestDetailsId(userTestDetailId);
            userTestDetails.Mark = mark;

            Silicus.UtilityContainer.Models.DataObjects.User user = _containerUserService.FindUserByEmail(HttpContext.User.Identity.Name);
            if (user != null)
            {
                userTestDetails.MarkGivenBy = user.ID;
                userTestDetails.MarkGivenByName = user.DisplayName;
            }
            userTestDetails.MarkGivenDate = DateTime.Now;
            userTestDetails.ReviwerComment = comment;
            _testSuiteService.UpdateUserTestDetails(userTestDetails);
        }

        private ReviewerQuestionViewModel GetAnswerDetails(ReviewerQuestionViewModel reviewerQuestionViewModel)
        {
            reviewerQuestionViewModel.CorrectAnswers = new List<string>();
            reviewerQuestionViewModel.CandidateAnswers = new List<string>();
            if (!string.IsNullOrWhiteSpace(reviewerQuestionViewModel.CorrectAnswer))
            {
                var CorrectAnswersString = reviewerQuestionViewModel.CorrectAnswer;
                var candidateAnswersString = reviewerQuestionViewModel.Answer;
                if (!string.IsNullOrWhiteSpace(CorrectAnswersString))
                {
                    if (CorrectAnswersString.Contains(","))
                    {
                        reviewerQuestionViewModel.CorrectAnswers = CorrectAnswersString.Split(',').ToList();
                    }
                    else
                    {
                        reviewerQuestionViewModel.CorrectAnswers.Add(CorrectAnswersString);
                    }
                }
                if (!string.IsNullOrWhiteSpace(candidateAnswersString))
                {
                    if (candidateAnswersString.Contains(","))
                    {
                        reviewerQuestionViewModel.CandidateAnswers = candidateAnswersString.Split(',').ToList();
                    }
                    else
                    {
                        reviewerQuestionViewModel.CandidateAnswers.Add(candidateAnswersString);
                    }
                }
            }
            return reviewerQuestionViewModel;
        }

        #endregion

        #region Preview
        private TestDetailsViewModel PreviewTestSuiteQuestion(int? questionId, int? testSuiteId, int questionType, int candidateId)
        {
            var viewerEmailId = User.Identity.Name;
            var viewer = _containerUserService.FindUserByEmail(viewerEmailId);
            var previewTest = new PreviewTestBusinessModel { TestSuite = new TestSuite { TestSuiteId = (int)testSuiteId }, ViewerId = viewer.ID, CandidateId = candidateId };
            TestDetailsBusinessModel userTestDetails = _testSuiteService.GetUserTestDetailsByViewerId(previewTest, questionId, questionType);
            var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
            testDetails = testDetails ?? new ReviewerQuestionViewModel();
            return testDetails;
        }


        private TestDetailsViewModel PreviewTestSuits(int? questionId, int? testSuiteId, int questionType)
        {
            var viewerEmailId = User.Identity.Name;
            var viewer = _containerUserService.FindUserByEmail(viewerEmailId);
            var previewTest = new PreviewTestBusinessModel { TestSuite = new TestSuite { TestSuiteId = (int)testSuiteId }, ViewerId = viewer.ID };
            TestDetailsBusinessModel userTestDetails = _testSuiteService.GetTestDetailsByTestSuit(previewTest, questionId, questionType);
            var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
            testDetails = testDetails ?? new ReviewerQuestionViewModel();
            return testDetails;
        }
        public ActionResult LoadPreviewQuestion(int userId, int testSuiteId)
        {
            var testSuiteQuestionModel = PreviewTestSuiteQuestion(null, testSuiteId, (int)QuestionType.Practical, userId);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }


        public ActionResult PreviewQuestion(int testSuiteId)
        {
            var testSuiteQuestionModel = PreviewTestSuits(null, testSuiteId, (int)QuestionType.Practical);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }


        public ActionResult GetQuestionForPreview(int? testSuiteId, int questionId, int userId)
        {
            int applicationDetails = _userService.GetUserLastestApplicationId(userId);
            var questionType = _testSuiteService.GetQuestionType(questionId);
            var testDetails = PreviewTestSuiteQuestion(questionId, testSuiteId, questionType, applicationDetails);
            return PartialView("_partialViewQuestion", testDetails);
        }


        public ActionResult GetQuestionForPreviewTestSuite(int? testSuiteId, int questionId)
        {
            var questionType = _testSuiteService.GetQuestionType(questionId);
            var testDetails = PreviewTestSuiteQuestion(questionId, testSuiteId, questionType);
            return PartialView("_partialViewQuestion", testDetails);
        }

        private TestDetailsViewModel PreviewTestSuiteQuestion(int? questionId, int? testSuiteId, int questionType)
        {
            var viewerEmailId = User.Identity.Name;
            var viewer = _containerUserService.FindUserByEmail(viewerEmailId);
            var previewTest = new PreviewTestBusinessModel { TestSuite = new TestSuite { TestSuiteId = (int)testSuiteId }, ViewerId = viewer.ID };
            TestDetailsBusinessModel userTestDetails = _testSuiteService.GetTestDetailsByTestSuit(previewTest, questionId, questionType);
            var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
            testDetails = testDetails ?? new ReviewerQuestionViewModel();
            return testDetails;
        }
        #endregion
    }
}