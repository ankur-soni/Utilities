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
        private readonly IQuestionService _questionService;
        private readonly IMappingService _mappingService;

        public CandidateController(IQuestionService questionService, MappingService mappingService)
        {
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
    }
}