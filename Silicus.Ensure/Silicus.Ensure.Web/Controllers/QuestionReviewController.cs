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
        public ActionResult Index(int technologyId)
        {
            return View(technologyId);
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
    }
}