using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
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

        public CandidateController(IQuestionService questionService)
        {
            _questionService = questionService;
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
            int QuestionId = 1;
            Question question = _questionService.GetSingleQuestion(Convert.ToInt32(QuestionId));
            return PartialView("_partialViewQuestion", question);
        }

        [HttpPost]
        public JsonResult AddMoreTime(int count)
        {
            count++;
            return Json(count);
        }
    }
}