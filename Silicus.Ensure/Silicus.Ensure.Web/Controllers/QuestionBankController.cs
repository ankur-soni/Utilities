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
    public class QuestionBankController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ITagsService _tagsService;
        private readonly IMappingService _mappingService;

        public QuestionBankController(IQuestionService questionService, ITagsService tagService, MappingService mappingService)
        {
            _questionService = questionService;
            _tagsService = tagService;
            _mappingService = mappingService;
        }

        public ActionResult AddQuestions(string QuestionId)
        {
            QuestionModel Que = new QuestionModel { QuestionType = "0", SkillTagsList = Tags() };
            return View(Que);
        }

        [HttpPost]
        public ActionResult AddQuestions(QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                Question Que = _mappingService.Map<QuestionModel, Question>(question);
                Que.QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription);
                Que.CorrectAnswer = (question.CorrectAnswer != null) ? string.Join(",", question.CorrectAnswer) : null;
                Que.Answer = HttpUtility.HtmlDecode(question.Answer);
                Que.Tags = string.Join(",", question.SkillTag);
                Que.IsPublishd = true;
                Que.ModifiedOn = DateTime.Now;
                Que.ModifiedBy = 0;
                bool isEdit = false;


                if (question.Edit)
                {
                    isEdit = true;
                    Que.CreatedOn = question.CreatedOn;
                    Que.CreatedBy = question.CreatedBy;
                    _questionService.Update(Que);
                }
                else
                {
                    Que.CreatedOn = DateTime.Now;
                    Que.CreatedBy = 0;
                    _questionService.Add(Que);
                }
                question = new QuestionModel() { Success = 1, Edit = isEdit, QuestionType = "0", SkillTagsList = Tags() };
            }
            else
            {
                question = new QuestionModel { QuestionType = "0", SkillTagsList = Tags() };
            }
            return View(question);
        }

        public ActionResult QuestionBank()
        {
            var Que = _questionService.GetQuestion().ToList();
            var QueModel = _mappingService.Map<List<Question>, List<QuestionModel>>(Que);
            foreach (var q in QueModel)
            {
                q.QuestionDescription = q.QuestionDescription.Substring(0, Math.Min(q.QuestionDescription.Length, 100));
                q.QuestionType = GetQuestionType(q.QuestionType);
                q.Tag = string.Join(" | ", Tags().Where(t => Que.Where(x => x.Id == q.Id).Select(p => p.Tags).FirstOrDefault().ToString().Split(',').Contains(t.TagId.ToString())).Select(l => l.TagName).ToList());
                q.Competency = GetCompetency(q.Competency);
            }
            return View(QueModel);
        }

        public ActionResult EditQuestion(string QuestionId)
        {
            if (!string.IsNullOrEmpty(QuestionId))
            {
                Question question = _questionService.GetSingleQuestion(Convert.ToInt32(QuestionId));
                QuestionModel QueModel = _mappingService.Map<Question, QuestionModel>(question);
                QueModel.QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription);
                QueModel.CorrectAnswer = (question.CorrectAnswer != null) ? question.CorrectAnswer.Split(',').ToList() : null;
                QueModel.Answer = HttpUtility.HtmlDecode(question.Answer);
                QueModel.SkillTag = question.Tags.Split(',').ToList();
                QueModel.Success = 0;
                QueModel.Edit = true;
                QueModel.SkillTagsList = Tags();
                return View("AddQuestions", QueModel);
            }
            return View("AddQuestions", null);
        }

        [HttpDelete]
        public JsonResult DeleteQuestion(int QuestionId)
        {
            _questionService.Delete(QuestionId);
            return Json(1);
        }

        private List<Tags> Tags()
        {
            List<Tags> tags = _tagsService.GetTagsDetails().ToList();
            return tags;
        }

        private string GetQuestionType(string type)
        {
            if (type == "1")
                return "Objective";
            else
                return "Practical";
        }

        private string GetCompetency(string type)
        {
            if (type == "1")
                return "Beginner";
            else if (type == "2")
                return "Intermediate";
            else
                return "Expert";
        }

    }
}