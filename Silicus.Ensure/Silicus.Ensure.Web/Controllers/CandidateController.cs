using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
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
                return RedirectToAction("LogOff", "Account");

            ViewBag.Status = 0;
            return View();
        }

        public ActionResult OnlineTest()
        {
            if (!ModelState.IsValid)
                return RedirectToAction("LogOff", "Account");

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
            testSuiteCandidateModel.TotalQuestionCount = testSuiteCandidateModel.PracticalCount + testSuiteCandidateModel.ObjectiveCount;
            testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.Duration;

            return View(testSuiteCandidateModel);
        }

        public ActionResult LoadQuestion(int userTestSuiteId)
        {
            TestSuiteQuestionModel testSuiteQuestionModel = TestSuiteQuestion(1, userTestSuiteId);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult OnNextPrevious(int qNumber, char move, int? userTestSuiteId, int? userTestDetailId, string answer)
        {
            answer = HttpUtility.HtmlDecode(answer);
            UpdateAnswer(answer, userTestDetailId);
            var testSuiteQuestionModel = (move == 'N') ? TestSuiteQuestion(qNumber += 1, userTestSuiteId) : TestSuiteQuestion(qNumber -= 1, userTestSuiteId);

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
            _userService.Update(candidate);

            // Update total time utilization for test back to UserTestSuite.
            TestSuite suite = _testSuiteService.GetTestSuitById(testSuiteId);
            UserTestSuite testSuit = _testSuiteService.GetUserTestSuiteId(userTestSuiteId);
            testSuit.Duration = suite.Duration + (testSuit.ExtraCount * 10);
            _testSuiteService.UpdateUserTestSuite(testSuit);

            // Calculate marks on test submit.
            CalculateMarks(userTestSuiteId);

            // Get All admin to send mail.
            List<User> userAdmin = _userService.GetUserByRole("ADMIN").ToList();
            SendSubmittedTestMail(userAdmin, candidate.FirstName + " " + candidate.LastName);

            return RedirectToAction("LogOff", "Account");
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

        private TestSuiteQuestionModel TestSuiteQuestion(int qNumber, int? userTestSuiteId)
        {
            var count = 0;
            dynamic userTestDetails = _testSuiteService.GetUserTestDetailsByUserTestSuitId(userTestSuiteId);
            List<TestSuiteQuestionModel> testSuiteQuestionModel = new List<TestSuiteQuestionModel>();
            TestSuiteQuestionModel model;

            foreach (var obj in userTestDetails)
            {
                model = new TestSuiteQuestionModel();
                model.QuestionNumber = count += 1;
                model.UserTestDetailId = obj.GetType().GetProperty("TestDetailId").GetValue(obj);
                model.Answer = obj.GetType().GetProperty("Answer").GetValue(obj);
                model.Id = obj.GetType().GetProperty("Id").GetValue(obj);
                model.QuestionType = obj.GetType().GetProperty("QuestionType").GetValue(obj);
                model.AnswerType = obj.GetType().GetProperty("AnswerType").GetValue(obj);
                model.QuestionDescription = obj.GetType().GetProperty("QuestionDescription").GetValue(obj);
                model.OptionCount = obj.GetType().GetProperty("OptionCount").GetValue(obj);
                model.Option1 = obj.GetType().GetProperty("Option1").GetValue(obj);
                model.Option2 = obj.GetType().GetProperty("Option2").GetValue(obj);
                model.Option3 = obj.GetType().GetProperty("Option3").GetValue(obj);
                model.Option4 = obj.GetType().GetProperty("Option4").GetValue(obj);
                model.Option5 = obj.GetType().GetProperty("Option5").GetValue(obj);
                model.Option6 = obj.GetType().GetProperty("Option6").GetValue(obj);
                model.Option7 = obj.GetType().GetProperty("Option7").GetValue(obj);
                model.Option8 = obj.GetType().GetProperty("Option8").GetValue(obj);
                model.Marks = obj.GetType().GetProperty("Marks").GetValue(obj);
                testSuiteQuestionModel.Add(model);
            }

            int totalQCount = testSuiteQuestionModel.Count;
            TestSuiteQuestionModel tQuestion = testSuiteQuestionModel.Where(p => p.QuestionNumber == qNumber).First();
            if (qNumber == 1)
                tQuestion.IsLast = true;
            if (qNumber == totalQCount)
                tQuestion.IsFirst = true;
            return tQuestion;
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