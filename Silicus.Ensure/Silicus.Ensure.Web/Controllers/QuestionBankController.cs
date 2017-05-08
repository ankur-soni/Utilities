using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using Silicus.Ensure.Models.Constants;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class QuestionBankController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ITagsService _tagsService;
        private readonly ITechnologyService _technologyService;
        private readonly IMappingService _mappingService;
        private readonly UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        public QuestionBankController(IQuestionService questionService, ITagsService tagService,
            MappingService mappingService, UtilityContainer.Services.Interfaces.IUserService containerUserService, ITechnologyService technologyService)
        {
            _questionService = questionService;
            _tagsService = tagService;
            _mappingService = mappingService;
            _containerUserService = containerUserService;
            _technologyService = technologyService;
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
                var userEmailId = User.Identity.Name;
                var user = _containerUserService.FindUserByEmail(userEmailId);
                bool isEdit = false;

                que.Status = QuestionStatus.ReadyForReview;
                if (question.Edit)
                {
                    isEdit = true;
                    que.ModifiedBy = user.ID;
                    que.ModifiedOn = DateTime.Now;
                    _questionService.Update(que);
                }
                else
                {
                    que.CreatedOn = DateTime.Now;
                    que.CreatedBy = user.ID;
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
        public ActionResult GetAllQuestions([DataSourceRequest] DataSourceRequest request)
        {
            var questions = _questionService.GetQuestion().OrderByDescending(x => x.ModifiedOn);
            DataSourceResult result = questions.ToDataSourceResult(request, qu => new QuestionModel
            {
                Id = qu.Id,
                AnswerType = qu.AnswerType,
                OptionCount = qu.OptionCount,
                Option1 = qu.Option1,
                Option2 = qu.Option2,
                Option3 = qu.Option3,
                Option4 = qu.Option4,
                Option5 = qu.Option5,
                Option6 = qu.Option6,
                Option7 = qu.Option7,
                Option8 = qu.Option8,
                Answer = qu.Answer,
                TechnologyId = qu.TechnologyId,
                Duration = qu.Duration,
                Marks = qu.Marks,
                IsPublishd = qu.IsPublishd,
                IsDeleted = qu.IsDeleted,
                CreatedOn = qu.CreatedOn,
                CreatedBy = qu.CreatedBy,
                ModifiedOn = qu.ModifiedOn,
                ModifiedBy = qu.ModifiedBy,
                Status = qu.Status,
                Technology = qu.Technology,
                CreatedByName = GetCreatedByName(qu.CreatedBy),
                QuestionDescription = qu.QuestionDescription.Substring(0, Math.Min(qu.QuestionDescription.Length, 100)),
                QuestionType = qu.QuestionType.ToString(),
                QuestionTypeString = GetQuestionType(qu.QuestionType.ToString()),
                TechnologyName = GetTechnologyName(qu.TechnologyId),
                StatusName = GetEnumDescription(qu.Status),
                Tags = qu.Tags,
                TagsString = GetTagNames(qu.Tags),
                ProficiencyLevel = qu.ProficiencyLevel.ToString(),
                ProficiencyLevelString = GetCompetency(qu.ProficiencyLevel.ToString())
            });

            return Json(result);
        }

        private string GetCreatedByName(int createdById)
        {
            var user=_containerUserService.GetUserByID(createdById);
            return user != null ? user.FirstName + " " + user.LastName : "";
        }

        private string GetTagNames(string tags)
        {
            var tagNamesString = "";
            var tagNames = new List<string>();
            if (tags != null)
            {
                int temp;
                var tagIds = tags.Split(',')
    .Select(s => new { P = int.TryParse(s, out temp), I = temp })
    .Where(x => x.P)
    .Select(x => x.I)
    .ToList();
                tagNames = _tagsService.GetTagNames(tagIds);
            }
            tagNamesString = string.Join(" | ", tagNames);
            return tagNamesString;
        }

        public ActionResult QuestionBank()
        {
            return View();
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

                if (queModel.CreatedBy > 0)
                {
                    var Creatoruser = _containerUserService.GetUserByID(queModel.CreatedBy);
                    queModel.CreatedByName = Creatoruser.FirstName + " " + Creatoruser.LastName + "(" + Creatoruser.EmployeeID + ")";
                }

                if (queModel.ModifiedBy.HasValue)
                {
                    if (queModel.ModifiedBy.Value > 0)
                    {
                        var Modifieruser = _containerUserService.GetUserByID(queModel.ModifiedBy.Value);
                        queModel.ModifiedByName = Modifieruser.FirstName + " " + Modifieruser.LastName + "(" + Modifieruser.EmployeeID + ")";
                    }
                }

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

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])field.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        private string GetTechnologyName(int technologyId)
        {
            var technology = _technologyService.GetTechnologyById(technologyId);
            return technology?.TechnologyName;
        }

    }
}