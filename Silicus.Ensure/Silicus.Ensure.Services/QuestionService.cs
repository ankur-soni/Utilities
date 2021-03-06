﻿using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.ReviewQuestion;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Ensure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IDataContext _context;

        public QuestionService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IQueryable<Question> GetQuestion()
        {
            return _context.Query<Question>().Where(x => !x.IsDeleted);
        }

        public Question GetSingleQuestion(int id)
        {
            return _context.Query<Question>().Where(x => x.Id == id).First();
        }

        public int Add(Question question)
        {
            _context.Add(question);
            return question.Id;
        }

        public void Update(Question question)
        {
            if (question != null)
                _context.Update(question);
        }

        public void Delete(int id)
        {
            if (id != 0)
            {
                Question que = GetSingleQuestion(id);
                que.IsDeleted = true;
                _context.Update(que);
            }
        }

        public IList<string> GenerateQuestionList(string tag, long duration, Proficiency competency)
        {
            throw new NotImplementedException();
        }
        public int? AddQuestionStatusDetails(QuestionStatusDetails statusDetails)
        {
            if (statusDetails != null)
            {
                _context.Add(statusDetails);
                return statusDetails.Id;
            }
            return null;
        }
        public ReviewQuestionBusinessModel GetQuestionDetailsForReview(int? questionId, int technologyId, int userId, QuestionStatus questionStatusType)
        {
            var reviewQuestionBusinessModel = new ReviewQuestionBusinessModel();
            var questionIds = GetQuestionsReadyForReview(userId, technologyId, questionStatusType);
            if (questionIds != null && questionIds.Any())
            {
                if (questionId == null)
                {
                    questionId = questionIds.First();
                }

                reviewQuestionBusinessModel.QuestionDetails = GetSingleQuestion((int)questionId);
                reviewQuestionBusinessModel.QuestionDetails.ReviewerComment = GetLatestCommenForQuestion((int)questionId);
                var questionIndex = questionIds.IndexOf((int)questionId);
                reviewQuestionBusinessModel.NextQuestionId = questionIndex >= 0 && questionIndex + 1 != questionIds.Count ? (int?)questionIds.ElementAtOrDefault(questionIndex + 1) : null;
            }
            return reviewQuestionBusinessModel;
        }

        public IList<int> GetQuestionsReadyForReview(int userId, int technologyId, QuestionStatus questionStatusType)
        {
            switch (questionStatusType)
            {
                case QuestionStatus.ReadyForReview:
                    return _context.Query<Question>().Where(ques => ques.TechnologyId == technologyId && ques.Status == QuestionStatus.ReadyForReview && ((ques.ModifiedBy == null && ques.CreatedBy != userId) || (ques.ModifiedBy != userId)))
                        .Select(q => q.Id).OrderBy(i => i).ToList();
                case QuestionStatus.Approved:
                case QuestionStatus.Rejected:
                    return _context.Query<Question>().Where(ques => ques.TechnologyId == technologyId && ques.Status == questionStatusType).Select(q => q.Id).OrderBy(i => i).ToList();
                case QuestionStatus.OnHold:
                    return _context.Query<Question>().Where(ques => ques.TechnologyId == technologyId && ques.Status == QuestionStatus.OnHold && ((ques.ModifiedBy == null && ques.CreatedBy != userId) || (ques.ModifiedBy != userId))).Select(q => q.Id).OrderBy(i => i).ToList();
                default:
                    return null;
            }
        }

        public TabSelectionBusinessModel GetCounts(TabSelectionBusinessModel tabSelection)
        {
            tabSelection.ReadyForReviewCount = _context.Query<Question>().Count(ques => ques.TechnologyId == tabSelection.TechnologyId && ques.Status == QuestionStatus.ReadyForReview && ((ques.ModifiedBy == null && ques.CreatedBy != tabSelection.UserId) || (ques.ModifiedBy != tabSelection.UserId)));
            tabSelection.RejectedCount = _context.Query<Question>().Count(ques => ques.TechnologyId == tabSelection.TechnologyId && ques.Status == QuestionStatus.Rejected);
            tabSelection.OnHoldCount = _context.Query<Question>().Count(ques => ques.TechnologyId == tabSelection.TechnologyId && ques.Status == QuestionStatus.OnHold && ((ques.ModifiedBy == null && ques.CreatedBy != tabSelection.UserId) || (ques.ModifiedBy != tabSelection.UserId)));
            return tabSelection;
        }
        public int GetCountOfInclusion(int questionId)
        {

            var noOfTimesincluded = _context.Query<UserTestDetails>().Count(ut => ut.QuestionId == questionId);
            return noOfTimesincluded;
        }

        public int GetCountOfCorrectlyAnswered(int questionId)
        {
            var noOfTimesCorrectlyAnswered = (from q in _context.Query<Question>().Where(ques => ques.Id == questionId)
                     join ut in _context.Query<UserTestDetails>()
on q.Id equals ut.QuestionId
                     where ut.Mark > q.Marks / 2
                     select q).Count();
            return noOfTimesCorrectlyAnswered;
        }
        private string GetLatestCommenForQuestion(int questionId)
        {
            var questionStatusDetails = _context.Query<QuestionStatusDetails>().Where(ques => ques.QuestionId == questionId)
                  .OrderByDescending(t => t.ChangedDate)
                  .FirstOrDefault();

            return questionStatusDetails?.Comment;
        }
    }
}
