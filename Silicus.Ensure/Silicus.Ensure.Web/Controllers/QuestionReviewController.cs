using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.ReviewQuestion;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Web.Models.ReviewQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class QuestionReviewController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ITagsService _tagService;
        private readonly IMappingService _mappingService;
        private readonly UtilityContainer.Services.Interfaces.IUserService _containerUserService;

        public QuestionReviewController(IQuestionService questionService, ITagsService tagService,
        MappingService mappingService, UtilityContainer.Services.Interfaces.IUserService containerUserService)
        {
            _questionService = questionService;
            _mappingService = mappingService;
            _containerUserService = containerUserService;
            _tagService = tagService;
        }
        // GET: QuestionReview
        public ActionResult Index(TabSelectionViewModel tabSelection)
        {
            return View(tabSelection);
        }

        public ActionResult ReviewQuestion(int? questionId, int technologyId, QuestionStatus questionStatusType)
        {
            var userEmailId = User.Identity.Name;
            var user = _containerUserService.FindUserByEmail(userEmailId);
            if (user != null && technologyId != 0)
            {
                var reviewQuestion = _questionService.GetQuestionDetailsForReview(questionId, technologyId, user.ID, questionStatusType);
                if (reviewQuestion != null && reviewQuestion.QuestionDetails != null)
                {
                    var reviewQuestionViewModel = _mappingService.Map<ReviewQuestionBusinessModel, ReviewQuestionViewModel>(reviewQuestion);
                    reviewQuestionViewModel.QuestionDetails.QuestionDescription = HttpUtility.HtmlDecode(reviewQuestionViewModel.QuestionDetails.QuestionDescription);
                    if (!string.IsNullOrWhiteSpace(reviewQuestion.QuestionDetails.CorrectAnswer))
                    {
                        SetAnsToOptions(reviewQuestion.QuestionDetails.CorrectAnswer, reviewQuestionViewModel.QuestionDetails);
                    }
                    reviewQuestionViewModel.QuestionDetails.Answer = HttpUtility.HtmlDecode(reviewQuestionViewModel.QuestionDetails.Answer);
                    reviewQuestionViewModel.QuestionDetails.SkillTag = reviewQuestion.QuestionDetails.Tags.Split(',').ToList();
                    reviewQuestionViewModel.QuestionDetails.Success = 0;
                    reviewQuestionViewModel.QuestionDetails.Edit = true;
                    reviewQuestionViewModel.QuestionDetails.SkillTagsList = _tagService.GetTagsDetails().ToList();
                    reviewQuestionViewModel.QuestionStatusType = questionStatusType;
                    reviewQuestionViewModel.TechnologyId = technologyId;
                    return View("_ReviewQuestion", reviewQuestionViewModel);
                }
            }
            return View("_ReviewQuestion", null);
        }

        public ActionResult SubmitQuestionReview(SubmitQuestionReviewViewModel review)
        {
            if (review != null)
            {
                UpdateQuestionStatus(review);
                if (review.NextQuestionId != 0)
                {
                    return RedirectToAction("ReviewQuestion", new { questionId = review.NextQuestionId, technologyId = review.TechnologyId, questionStatusType = review.QuestionStatusType });
                }
            }
            return null;
        }

        private void UpdateQuestionStatus(SubmitQuestionReviewViewModel review)
        {
            var userEmailId = User.Identity.Name;
            var user = _containerUserService.FindUserByEmail(userEmailId);
            var question = _questionService.GetSingleQuestion(review.QuestionId);
            if (question != null)
            {
                question.Status = review.Status;
                _questionService.Update(question);
                var questionStatusDetails = new QuestionStatusDetails();
                questionStatusDetails.QuestionId = review.QuestionId;
                questionStatusDetails.Status = review.Status;
                questionStatusDetails.Comment = review.Comment;
                questionStatusDetails.ChangedBy = user.ID;
                questionStatusDetails.ChangedDate = DateTime.Now;
                _questionService.AddQuestionStatusDetails(questionStatusDetails);
            }
        }

        private void SetAnsToOptions(string correctAnswer, QuestionModel queModel)
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

        [HttpPost]
        public ActionResult EditAndApproveQuestion(QuestionModel question)
        {
            var userEmailId = User.Identity.Name;
            var user = _containerUserService.FindUserByEmail(userEmailId);
            UpdateQuestion(question);
            var questionStatusDetails = new QuestionStatusDetails();
            questionStatusDetails.QuestionId = question.Id;
            if (question.Status == QuestionStatus.OnHold)
            {
                questionStatusDetails.Status = QuestionStatus.Approved;
            }
            else
            {
                questionStatusDetails.Status = QuestionStatus.ReadyForReview;
            }
            questionStatusDetails.ChangedBy = user.ID;
            questionStatusDetails.ChangedDate = DateTime.Now;
            _questionService.AddQuestionStatusDetails(questionStatusDetails);
            return RedirectToAction("Index", new TabSelectionViewModel { QuestionId = question.NextQuestionId, TechnologyId = question.TechnologyId, IsOnHold = true });
        }

        private void UpdateQuestion(QuestionModel question)
        {
            Question que = _mappingService.Map<QuestionModel, Question>(question);
            que.QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription);
            que.CorrectAnswer = setCorrectAnswer(question);
            que.Answer = HttpUtility.HtmlDecode(question.Answer);
            que.Tags = string.Join(",", question.SkillTag);
            que.IsPublishd = true;
            var userEmailId = User.Identity.Name;
            var user = _containerUserService.FindUserByEmail(userEmailId);
            que.Status = QuestionStatus.Approved;
            que.ModifiedBy = user.ID;
            que.ModifiedOn = DateTime.Now;
            _questionService.Update(que);
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
    }
}