using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Services.Interfaces;
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
    [Authorize]
    public class CandidateController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IQuestionService _questionService;
        private readonly IMappingService _mappingService;
        private readonly IUserService _userService;
        private readonly ITestSuiteService _testSuiteService;

        public CandidateController(IEmailService emailService, IQuestionService questionService, MappingService mappingService, IUserService userService, ITestSuiteService testSuiteService)
        {
            _emailService = emailService;
            _questionService = questionService;
            _mappingService = mappingService;
            _userService = userService;
            _testSuiteService = testSuiteService;
        }

        public ActionResult Welcome()
        {
            if (!ModelState.IsValid)
                return RedirectToAction("LogOff", "CandidateAccount");

            ViewBag.Status = 0;
            return View();
        }

        public ActionResult OnlineTest()
        {
            if (!ModelState.IsValid)
                return RedirectToAction("LogOff", "CandidateAccount");

            var userEmail = User.Identity.Name.Trim();
            User user = _userService.GetUserByEmail(userEmail);
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

            return View(testSuiteCandidateModel);
        }

        private QuestionNavigationViewModel GetNavigationDetails(int userTestSuiteId)
        {
            var navigationDetailsBusinessModel = _testSuiteService.GetNavigationDetails(userTestSuiteId);
            var navigationDetails = _mappingService.Map<QuestionNavigationBusinessModel, QuestionNavigationViewModel>(navigationDetailsBusinessModel);
            return navigationDetails;
        }

        private CandidateInfoViewModel GetCandidateInfo(Ensure.Models.DataObjects.User user)
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

        public ActionResult LoadQuestion(int userTestSuiteId)
        {
            TestDetailsViewModel testSuiteQuestionModel = TestSuiteQuestion(null, userTestSuiteId, (int)QuestionType.Practical);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult GetQuestionDetails(QuestionDetailsViewModel questionDetails)
        {
            questionDetails.QuestionType = _testSuiteService.GetQuestionType(questionDetails.QuestionId);
            questionDetails.Answer = HttpUtility.HtmlDecode(questionDetails.Answer);
            UpdateAnswer(questionDetails.Answer, questionDetails.UserTestDetailId);
            var testSuiteQuestionModel = TestSuiteQuestion(questionDetails.QuestionId, questionDetails.UserTestSuiteId, questionDetails.QuestionType);

            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }



        public ActionResult OnSubmitTest(int testSuiteId, int userTestSuiteId, int? userTestDetailId, int userId, string answer)
        {
            // Update last question answer of test.
            answer = HttpUtility.HtmlDecode(answer);
            UpdateAnswer(answer, userTestDetailId);

            // Update candidate status as Test "Submitted".
            User candidate = _userService.GetUserById(userId);
            candidate.TestStatus = TestStatus.Submitted.ToString();
            candidate.CandidateStatus = CandidateStatus.TestSubmitted.ToString();
            _userService.Update(candidate);

            // Update total time utilization for test back to UserTestSuite.
            TestSuite suite = _testSuiteService.GetTestSuitById(testSuiteId);
            UserTestSuite testSuit = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
            testSuit.Duration = suite.Duration + (testSuit.ExtraCount * 10);
            testSuit.StatusId = Convert.ToInt32(TestStatus.Submitted);
            _testSuiteService.UpdateUserTestSuite(testSuit);

            // Calculate marks on test submit.
            CalculateMarks(userTestSuiteId);

            // Get All admin to send mail.
            List<User> userAdmin = _userService.GetUserByRole("ADMIN").ToList();
            SendSubmittedTestMail(userAdmin, candidate.FirstName + " " + candidate.LastName);

            return RedirectToAction("LogOff", "CandidateAccount");
        }

        [HttpPost]
        public JsonResult AddMoreTime(int count, int userTestSuiteId)
        {
            count = count + 1;
            UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
            userTestSuite.ExtraCount = count;
            _testSuiteService.UpdateUserTestSuite(userTestSuite);
            return Json(count);
        }

        [HttpPost]
        public JsonResult SumbmitCandidateResult(CandidateResultViewmodel candidateResultViewmodel)
        {
            var user = _userService.GetUserById(candidateResultViewmodel.CandidateUserId);
            user.CandidateStatus = candidateResultViewmodel.Status.ToString();
            _userService.Update(user);
            return Json(true);
        }

        [HttpPost]
        public JsonResult UpdateTimeCounter(int time, int userTestSuiteId)
        {
            UserTestSuite userTestSuite = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
            userTestSuite.Duration = time / 60;
            _testSuiteService.UpdateUserTestSuite(userTestSuite);
            return Json(1);
        }

        private void UpdateAnswer(string answer, int? userTestDetailId)
        {
            UserTestDetails userTestDetails = _testSuiteService.GetUserTestDetailsId(userTestDetailId);
            userTestDetails.Answer = answer;
            _testSuiteService.UpdateUserTestDetails(userTestDetails);
        }

        private TestDetailsViewModel TestSuiteQuestion(int? questionId, int? userTestSuiteId, int questionType)
        {
            TestDetailsBusinessModel userTestDetails = _testSuiteService.GetUserTestDetailsByUserTestSuitId(userTestSuiteId, questionId, questionType);
            var testDetails = _mappingService.Map<TestDetailsBusinessModel, TestDetailsViewModel>(userTestDetails);
            testDetails = testDetails ?? new TestDetailsViewModel();
            return testDetails;
        }

        private void SendSubmittedTestMail(List<User> userAdmin, string fullname)
        {
            string subject = "Test Submitted for " + fullname;

            foreach (var usr in userAdmin)
            {
                string body = "Dear " + usr.FirstName + " " + usr.LastName + "," +
                              "The Online Test has been submitted for Candidate " + fullname + " on " + DateTime.Now + ". Please review, evatuate and add your valuable feedback of the Test in order to conduct first round of interview." +
                              "This is an auto-generated email sent by Ensure. Please do not reply to this email." +
                              "Regards," +
                              "Ensure, IT Support";


                _emailService.SendEmailAsync(usr.Email, subject, body);
            }
        }

        private void CalculateMarks(int userTestSuiteId)
        {
            List<UserTestDetails> userTestDetails = _testSuiteService.GetUserTestDetailsListByUserTestSuitId(userTestSuiteId).ToList();
            foreach (UserTestDetails testDetail in userTestDetails)
            {
                Question question = _questionService.GetSingleQuestion(testDetail.QuestionId);
                if (question.QuestionType == 1)
                {
                    if (question.CorrectAnswer.Trim() == testDetail.Answer.Trim())
                        testDetail.Mark = question.Marks;
                    else
                        testDetail.Mark = 0;

                    _testSuiteService.UpdateUserTestDetails(testDetail);
                }
            }
        }
    }
}