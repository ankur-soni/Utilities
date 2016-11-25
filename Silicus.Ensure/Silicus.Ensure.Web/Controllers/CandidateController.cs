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
            testSuiteCandidateModel.TotalCount = testSuiteCandidateModel.PracticalCount + testSuiteCandidateModel.ObjectiveCount;
            testSuiteCandidateModel.DurationInMin = testSuiteCandidateModel.Duration / 60;

            return View(testSuiteCandidateModel);
        }

        public ActionResult LoadQuestion(int userTestSuiteId)
        {
            TestSuiteQuestionModel testSuiteQuestionModel = TestSuiteQuestion(1, userTestSuiteId);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult OnNextPrevious(int Qnumber, char Move, int? userTestSuiteId, int? userTestDetailId, string answer)
        {
            answer = HttpUtility.HtmlDecode(answer);
            UpdateAnswer(answer, userTestDetailId);
            TestSuiteQuestionModel testSuiteQuestionModel = new TestSuiteQuestionModel();
            if (Move == 'N')
                testSuiteQuestionModel = TestSuiteQuestion(Qnumber += 1, userTestSuiteId);
            else
                testSuiteQuestionModel = TestSuiteQuestion(Qnumber -= 1, userTestSuiteId);

            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult OnSubmitTest(int? userTestDetailId, int userId, string answer)
        {
            answer = HttpUtility.HtmlDecode(answer);
            UpdateAnswer(answer, userTestDetailId);
            List<User> userAdmin = _userService.GetUserByRole("ADMIN").ToList();

            User candidate = _userService.GetUserById(userId);
            candidate.TestStatus = "Test Submitted";
            _userService.Update(candidate);

            if (userAdmin != null)
                SendSubmittedTestMail(userAdmin, candidate.FirstName + " " + candidate.LastName);

            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        public JsonResult AddMoreTime(int count)
        {
            count++;
            return Json(count);
        }

        private void UpdateAnswer(string answer, int? userTestDetailId)
        {
            UserTestDetails userTestDetails = _testSuiteService.GetUserTestDetailsId(userTestDetailId);
            userTestDetails.Answer = answer;
            _testSuiteService.UpdateUserTestDetails(userTestDetails);
        }

        private TestSuiteQuestionModel TestSuiteQuestion(int Qnumber, int? userTestSuiteId)
        {
            int Count = 0;
            dynamic userTestDetails = _testSuiteService.GetUserTestDetailsByUserTestSuitId(userTestSuiteId);
            List<TestSuiteQuestionModel> testSuiteQuestionModel = new List<TestSuiteQuestionModel>();
            TestSuiteQuestionModel model;

            foreach (var obj in userTestDetails)
            {
                model = new TestSuiteQuestionModel();
                model.QuestionNumber = Count += 1;
                model.UserTestDetailId = obj.GetType().GetProperty("TestDetailId").GetValue(obj);
                model.Answer = obj.GetType().GetProperty("Answer").GetValue(obj);
                model.Id = obj.GetType().GetProperty("Id").GetValue(obj);
                model.QuestionType = obj.GetType().GetProperty("QuestionType").GetValue(obj);
                model.AnswerType = obj.GetType().GetProperty("AnswerType").GetValue(obj);
                model.QuestionDescription = obj.GetType().GetProperty("QuestionDescription").GetValue(obj);
                model.Option1 = obj.GetType().GetProperty("Option1").GetValue(obj);
                model.Option2 = obj.GetType().GetProperty("Option2").GetValue(obj);
                model.Option3 = obj.GetType().GetProperty("Option3").GetValue(obj);
                model.Option4 = obj.GetType().GetProperty("Option4").GetValue(obj);
                model.Marks = obj.GetType().GetProperty("Marks").GetValue(obj);
                testSuiteQuestionModel.Add(model);
            }

            int totalQCount = testSuiteQuestionModel.Count;
            TestSuiteQuestionModel TQuestion = testSuiteQuestionModel.Where(p => p.QuestionNumber == Qnumber).First();
            if (Qnumber == 1)
                TQuestion.IsLast = true;
            if (Qnumber == totalQCount)
                TQuestion.IsFirst = true;
            return TQuestion;
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
    }
}