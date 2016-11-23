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

        public CandidateController(IEmailService emailService, IQuestionService questionService, MappingService mappingService)
        {
            _emailService = emailService;
            _questionService = questionService;
            _mappingService = mappingService;
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult OnlineTest()
        {
            return View();
        }

        public ActionResult LoadQuestion()
        {
            TestSuiteQuestionModel testSuiteQuestionModel = TestSuiteQuestion(1);
            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult OnNextPrevious(int Qnumber, char Move)
        {
            TestSuiteQuestionModel testSuiteQuestionModel = new TestSuiteQuestionModel();
            if (Move == 'N')
            {
                testSuiteQuestionModel = TestSuiteQuestion(Qnumber += 1);
            }
            else
            {
                testSuiteQuestionModel = TestSuiteQuestion(Qnumber -= 1);
            }

            return PartialView("_partialViewQuestion", testSuiteQuestionModel);
        }

        public ActionResult OnSubmitTest()
        {
            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        public JsonResult AddMoreTime(int count)
        {
            count++;
            return Json(count);
        }

        private TestSuiteQuestionModel TestSuiteQuestion(int Qnumber)
        {
            int Count = 0;
            List<Question> questions = _questionService.GetQuestion().OrderBy(o => o.QuestionType).ToList();
            List<TestSuiteQuestionModel> testSuiteQuestionModel = _mappingService.Map<List<Question>, List<TestSuiteQuestionModel>>(questions);
            for (int i = 0; i < testSuiteQuestionModel.Count; i++)
            {
                Count += 1;
                testSuiteQuestionModel[i].QuestionNumber = Count;
            }
            int totalQCount = questions.Count;
            TestSuiteQuestionModel TQuestion = testSuiteQuestionModel.Where(p => p.QuestionNumber == Qnumber).First();
            if (Qnumber == 1)
                TQuestion.IsLast = true;
            if (Qnumber == totalQCount)
                TQuestion.IsFirst = true;
            return TQuestion;
        }

        private void SendSubmittedTestMail(string email, string fullname)
        {
            string subject = "Test Submitted for " + fullname;

            string body = "Dear " + fullname + "," +
                          "The Online Test has been submitted for <Candidate First Name + Last Name> on <DD/MM/YYYY HH:MM>. Please review, evatuate and add your valuable feedback of the Test in order to conduct first round of interview." +
                          "This is an auto-generated email sent by Ensure. Please do not reply to this email." +
                          "Regards," +
                          "Ensure, IT Support";

            _emailService.SendEmailAsync(email, subject, body);
        }
    }
}