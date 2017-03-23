﻿using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                que.CorrectAnswer = setCorrectAnswer(question);
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
                var btnValue = Request["btnSaveAndAddNewQuestion"];
                int success = 1;
                if (!string.IsNullOrWhiteSpace(btnValue) && btnValue.Equals("save & add another question", StringComparison.OrdinalIgnoreCase))
                {
                    success = 2;
                }
                question = new QuestionModel() { Success = success, Edit = isEdit, QuestionType = "0", SkillTagsList = Tags() };
            }
            else
            {
                question = new QuestionModel { QuestionType = "0", SkillTagsList = Tags() };
            }
            ModelState.Clear();
            return View(question);
        }

        public ActionResult QuestionBank()
        {
            return View();
        }

        public ActionResult GetAllQuestions([DataSourceRequest] DataSourceRequest request)
        {
            var que = _questionService.GetQuestion().OrderByDescending(x => x.ModifiedOn);
            var queModel = _mappingService.Map<IEnumerable<Question>, IEnumerable<QuestionModel>>(que);
            foreach (var q in queModel)
            {
                q.QuestionDescription = q.QuestionDescription.Substring(0, Math.Min(q.QuestionDescription.Length, 100));
                q.QuestionType = GetQuestionType(q.QuestionType);
                q.Tag = string.Join(" | ", Tags().Where(t => que.Where(x => x.Id == q.Id).Select(p => p.Tags).First().ToString().Split(',').Contains(t.TagId.ToString())).Select(l => l.TagName).ToList());
                q.ProficiencyLevel = GetCompetency(q.ProficiencyLevel);
            }
            DataSourceResult result = queModel.ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult EditQuestion(string questionId)
        {
            ModelState.Clear();
            if (!string.IsNullOrEmpty(questionId))
            {
                Question question = _questionService.GetSingleQuestion(Convert.ToInt32(questionId));
                QuestionModel queModel = _mappingService.Map<Question, QuestionModel>(question);
                queModel.QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription);
                //queModel.CorrectAnswer = (question.CorrectAnswer != null) ? question.CorrectAnswer.Split(',').ToList() : null;

                if (question.CorrectAnswer != null)
                {
                    setAnsToOptions(question.CorrectAnswer, queModel);
                }

                queModel.Answer = HttpUtility.HtmlDecode(question.Answer);
                queModel.SkillTag = question.Tags.Split(',').ToList();
                queModel.Success = 0;
                queModel.Edit = true;
                queModel.SkillTagsList = Tags();
                return View("AddQuestions", queModel);
            }
            return View("AddQuestions", null);
        }

        public JsonResult DeleteQuestion(QuestionModel question)
        {
            if (question != null)
            {
                _questionService.Delete(question.Id);
                return Json(1);
            }
            else
            {
                return Json(-1);
            }
        }

        private void setAnsToOptions(string correctAnswer, QuestionModel queModel)
        {
            queModel.IsAnsOption1 = correctAnswer.Contains("1");
            queModel.IsAnsOption2 = correctAnswer.Contains("2");
            queModel.IsAnsOption3 = correctAnswer.Contains("3");
            queModel.IsAnsOption4 = correctAnswer.Contains("4");
            queModel.IsAnsOption5 = correctAnswer.Contains("5");
            queModel.IsAnsOption6 = correctAnswer.Contains("6");
            queModel.IsAnsOption7 = correctAnswer.Contains("7");
            queModel.IsAnsOption8 = correctAnswer.Contains("8");
        }

        private string setCorrectAnswer(QuestionModel queModel)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(queModel.IsAnsOption1 ? "1," : "");
            ans.Append(queModel.IsAnsOption2 ? "2," : "");
            ans.Append(queModel.IsAnsOption3 ? "3," : "");
            ans.Append(queModel.IsAnsOption4 ? "4," : "");
            ans.Append(queModel.IsAnsOption5 ? "5," : "");
            ans.Append(queModel.IsAnsOption6 ? "6," : "");
            ans.Append(queModel.IsAnsOption7 ? "7," : "");
            ans.Append(queModel.IsAnsOption8 ? "8," : "");

            return ans.ToString();
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