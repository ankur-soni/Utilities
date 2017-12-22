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

        public ActionResult Dashboard()
        {
            var questionsList = _questionService.GetQuestion().GroupBy(q => new { q.Technology, q.ProficiencyLevel })
                .Select(y => new
                {
                    Technology = y.Key.Technology.TechnologyName,
                    Level = y.Key.ProficiencyLevel,
                    Count = y.Count()
                }
                );

            var catList = questionsList.Select(q => q.Technology).Distinct().ToList();

            var BeginerList = new List<int>();
            var ItermidiateList = new List<int>();
            var ExpertList = new List<int>();

            foreach (var cat in catList)
            {
                BeginerList.Add(questionsList.Where(c => c.Technology == cat && c.Level == 1).Select(q => q.Count).FirstOrDefault());
                ItermidiateList.Add(questionsList.Where(c => c.Technology == cat && c.Level == 2).Select(q => q.Count).FirstOrDefault());
                ExpertList.Add(questionsList.Where(c => c.Technology == cat && c.Level == 3).Select(q => q.Count).FirstOrDefault());

            }
            // var ConsolidatedQList = questionsList.Select(t => t.)
            var ChartData = new { catList = catList, BeginerList = BeginerList, ItermidiateList = ItermidiateList, ExpertList = ExpertList };

            var fromJson = Json(ChartData);

            return View(fromJson);
        }

        public ActionResult GetAllQuestionsStastistics([DataSourceRequest] DataSourceRequest request)
        {
            var questions = _questionService.GetQuestion().OrderByDescending(x => x.ModifiedOn);
            var QList = questions.GroupBy(q => new { q.TechnologyId, q.Status }).Select(qu => new Questionstatistics()
            {
                //CreatedBy = qu.Key.CreatedBy.ToString(),
                Technology = qu.Key.TechnologyId.ToString(),
                ProficiencyLevel = qu.Key.Status.ToString(),
                Count = qu.Count()
            }).ToList();

            foreach (var obj in QList)
            {
                //obj.CreatedBy = GetCreatedByName(int.Parse(obj.CreatedBy));
                obj.Technology = GetTechnologyName(int.Parse(obj.Technology));
                //obj.ProficiencyLevel = GetCompetency(obj.ProficiencyLevel);
                //obj.Technology = obj.Technology;
            }

            //var jsonResult = Json(QList.ToArray().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            return Json(QList.ToDataSourceResult(request));
        }

        public ActionResult GetEmployeeWiseQuestionsStastistics([DataSourceRequest] DataSourceRequest request)
        {
            var questions = _questionService.GetQuestion().OrderByDescending(x => x.ModifiedOn);
            var QList = questions.GroupBy(q => new { q.CreatedBy, q.TechnologyId }).Select(qu => new Questionstatistics()
            {
                CreatedBy = qu.Key.CreatedBy.ToString(),
                Technology = qu.Key.TechnologyId.ToString(),             
                Count = qu.Count()
            }).ToList();

            foreach (var obj in QList)
            {
                obj.CreatedBy = GetCreatedByName(int.Parse(obj.CreatedBy));
                obj.Technology = GetTechnologyName(int.Parse(obj.Technology));               
            }

            //var jsonResult = Json(QList.ToArray().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            return Json(QList.ToDataSourceResult(request));
        }

        public ActionResult GetTagWiseQuestionsStastistics([DataSourceRequest] DataSourceRequest request)
        {
            var questions = _questionService.GetQuestion().OrderByDescending(x => x.ModifiedOn);
            var QList = questions.GroupBy(q => new {q.TechnologyId, q.Tags }).Select(qu => new Questionstatistics()
            {
               // CreatedBy = qu.Key.CreatedBy.ToString(),
                Technology = qu.Key.Tags.ToString(),
                ProficiencyLevel = qu.Key.TechnologyId.ToString(),
                Count = qu.Count()
            }).ToList();

            List<Questionstatistics> updatedList = new List<Questionstatistics>();

            foreach (var obj in QList)
            {               
                if (obj.Technology.IndexOf(",") > 0)
                {                  
                        int temp;
                        var tagIds = obj.Technology.Split(',')
                                      .Select(s => new { P = int.TryParse(s, out temp), I = temp })
                                      .Where(x => x.P)
                                      .Select(x => x.I)
                                      .ToList();

                        var tagNames = _tagsService.GetTagNames(tagIds);

                        foreach (var item in tagNames)
                        {
                            var AlreadyPresentTag = updatedList.FirstOrDefault(x => x.Technology == item && x.ProficiencyLevel == obj.ProficiencyLevel);

                            if (AlreadyPresentTag == null)
                                updatedList.Add(new Questionstatistics() { Technology = item, Count = obj.Count, ProficiencyLevel = GetTechnologyName(int.Parse(obj.ProficiencyLevel))});
                            else
                                AlreadyPresentTag.Count += obj.Count;
                        }                        
                }
                else
                {
                    var AlreadyPresentTag = updatedList.FirstOrDefault(x => x.Technology == obj.Technology && x.ProficiencyLevel == obj.ProficiencyLevel);

                    if (AlreadyPresentTag == null)
                        updatedList.Add(new Questionstatistics() { Technology = GetTagNames(obj.Technology), Count = obj.Count, ProficiencyLevel = GetTechnologyName(int.Parse(obj.ProficiencyLevel)) });
                    else
                        AlreadyPresentTag.Count += obj.Count;                    
                }              
            }

            updatedList = updatedList.GroupBy(q => new { q.ProficiencyLevel, q.Technology }).Select(qu => new Questionstatistics()
            {            
                ProficiencyLevel = qu.Key.ProficiencyLevel,
                Technology = qu.Key.Technology.ToString(),              
                Count = qu.Sum(c => c.Count)
            }).ToList();

            return Json(updatedList.ToDataSourceResult(request));
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

        private int GetCountOfCorrectlyAnswered(int questionId)
        {
            var count = _questionService.GetCountOfCorrectlyAnswered(questionId);
            return count;
        }

        private int GetCountOfInclusion(int questionId)
        {
            var count = _questionService.GetCountOfInclusion(questionId);
            return count;
        }

        private string GetCreatedByName(int createdById)
        {
            var user = _containerUserService.GetUserByID(createdById);
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

        //    private string GetTagNamesWithCount(string tags, int count,string createdBy, List<Questionstatistics> list)
        //    {
        //        var tagNamesString = "";
        //        var tagNames = new List<string>();
        //        if (tags != null)
        //        {
        //            int temp;
        //            var tagIds = tags.Split(',')
        //.Select(s => new { P = int.TryParse(s, out temp), I = temp })
        //.Where(x => x.P)
        //.Select(x => x.I)
        //.ToList();
        //            tagNames = _tagsService.GetTagNames(tagIds);
        //        }

        //        foreach(var item in tagNames)
        //        {
        //            list.Add(new Questionstatistics() { Technology = item, Count = count, CreatedBy = createdBy });
        //        }

        //        tagNamesString = string.Join(" | ", tagNames);
        //        return tagNamesString;
        //    }


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