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
    [Authorize]
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

        public ActionResult AddQuestions(string questionId)
        {
            QuestionModel que = new QuestionModel { QuestionType = "0", OptionCount = 2, SkillTagsList = Tags() };
            return View(que);
        }

        [HttpPost]
        public ActionResult AddQuestions(QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                Question que = _mappingService.Map<QuestionModel, Question>(question);
                que.QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription);
                que.CorrectAnswer = (question.CorrectAnswer != null) ? string.Join(",", question.CorrectAnswer) : null;
                que.Answer = HttpUtility.HtmlDecode(question.Answer);
                que.Tags = string.Join(",", question.SkillTag);
                que.IsPublishd = true;
                que.ModifiedOn = DateTime.Now;
                que.ModifiedBy = 0;
                bool isEdit = false;


                if (question.Edit)
                {
                    isEdit = true;
                    que.CreatedOn = question.CreatedOn;
                    que.CreatedBy = question.CreatedBy;
                    _questionService.Update(que);
                }
                else
                {
                    que.CreatedOn = DateTime.Now;
                    que.CreatedBy = 0;
                    _questionService.Add(que);
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
            var que = _questionService.GetQuestion().ToList();
            var queModel = _mappingService.Map<List<Question>, List<QuestionModel>>(que);
            foreach (var q in queModel)
            {
                q.QuestionDescription = q.QuestionDescription.Substring(0, Math.Min(q.QuestionDescription.Length, 100));
                q.QuestionType = GetQuestionType(q.QuestionType);
                q.Tag = string.Join(" | ", Tags().Where(t => que.Where(x => x.Id == q.Id).Select(p => p.Tags).First().ToString().Split(',').Contains(t.TagId.ToString())).Select(l => l.TagName).ToList());
                q.ProficiencyLevel = GetCompetency(q.ProficiencyLevel);
            }
            return View(queModel);
        }

        public ActionResult EditQuestion(string questionId)
        {
            if (!string.IsNullOrEmpty(questionId))
            {
                Question question = _questionService.GetSingleQuestion(Convert.ToInt32(questionId));
                QuestionModel queModel = _mappingService.Map<Question, QuestionModel>(question);
                queModel.QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription);
                queModel.CorrectAnswer = (question.CorrectAnswer != null) ? question.CorrectAnswer.Split(',').ToList() : null;
                queModel.Answer = HttpUtility.HtmlDecode(question.Answer);
                queModel.SkillTag = question.Tags.Split(',').ToList();
                queModel.Success = 0;
                queModel.Edit = true;
                queModel.SkillTagsList = Tags();
                return View("AddQuestions", queModel);
            }
            return View("AddQuestions", null);
        }

        [HttpDelete]
        public JsonResult DeleteQuestion(int questionId)
        {
            _questionService.Delete(questionId);
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