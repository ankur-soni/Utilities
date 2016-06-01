using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Models;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Encourage.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly IResultService _resultService;
        private readonly IReviewService _reviewService;
        private readonly INominationService _nominationService;
        private readonly ICommonDbService _commonDbService;
        private readonly ICommonDataBaseContext _commonDbContext;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.Encourage.DAL.Interfaces.IDataContextFactory _dataContextFactory;

        public ReviewController(IResultService resultService, INominationService nominationService, ICommonDbService commonDbService, Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, IAwardService awardService, IReviewService reviewService)
        {
            _commonDbService = commonDbService;
            _commonDbContext = _commonDbService.GetCommonDataBaseContext();
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
            _reviewService = reviewService;
            _nominationService = nominationService;
            _resultService = resultService;
        }

        [HttpGet]
        public ActionResult ReviewFeedbackList()
        {
            var reviewFeedbacks = new List<ReviewFeedbackListViewModel>();
            var uniqueReviewedNomination = _encourageDatabaseContext.Query<Review>().GroupBy(x => x.NominationId).Select(group => group.FirstOrDefault()).ToList();

            foreach (var reviewNomination in uniqueReviewedNomination)
            {
                var isShortlisted = false;
                var isWinner = false;
                var nomination = _encourageDatabaseContext.Query<Nomination>().Where(x => x.Id == reviewNomination.NominationId).SingleOrDefault();
                var awardCode = _encourageDatabaseContext.Query<Award>().Where(award => award.Id == nomination.AwardId).SingleOrDefault().Code;
                var nominee = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = nomination.NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();

                var totalCreditPoints = _encourageDatabaseContext.Query<ReviewerComment>().
                                     Where(model => model.NominationId == nomination.Id).
                                     Sum(model => model.Credit.Value);

                var checkResultStatus = _resultService.IsShortlistedOrWinner(nomination.Id);
                if (checkResultStatus == 1)
                    isWinner = true;
                else if (checkResultStatus == 2)
                    isShortlisted = true;

                reviewFeedbacks.Add(
                     new ReviewFeedbackListViewModel()
                       {
                           AwardName = awardCode,
                           Credits = totalCreditPoints,
                           DisplayName = nominee.DisplayName,
                           Intials = nominee.FirstName.Substring(0, 1) + "" + nominee.LastName.Substring(0, 1),
                           NominationTime = nominationTimeToDisplay,
                           NominationId = nomination.Id,
                           IsShortlisted = isShortlisted,
                           IsWinner = isWinner
                       }
                    );
            }
            return View(reviewFeedbacks);
        }


        [HttpGet]
        public ActionResult ViewNominationForShortlist(ReviewFeedbackListViewModel nominationModel)
        {
            var reviews = _reviewService.GetReviewsForNomination(nominationModel.NominationId).ToList();
            var nomination = _nominationService.GetNomination(reviews.FirstOrDefault().NominationId);
            var allReviewerComments = new List<List<ReviewerCommentViewModel>>();


            foreach (var review in reviews)
            {
                var allreviewerComment = _encourageDatabaseContext.Query<ReviewerComment>().Where(model => model.ReviewId == review.Id);
                var reviewer = _encourageDatabaseContext.Query<Reviewer>().Where(model => model.Id == review.ReviewerId).FirstOrDefault();
                var reviewerObj = _commonDbContext.Query<User>().Where(u => u.ID == reviewer.UserId).FirstOrDefault();


                var reviewerCommentList = new List<ReviewerCommentViewModel>();
                foreach (var reviewerComment in allreviewerComment)
                {

                    var singleReviewerComent = new ReviewerCommentViewModel()
                      {
                          CriteriaID = reviewerComment.CriteriaId,
                          Comment = reviewerComment.Comment,
                          Credit = Convert.ToBoolean(reviewerComment.Credit.Value),
                          ReviewerName = reviewerObj.DisplayName

                      };
                    reviewerCommentList.Add(singleReviewerComent);
                }
                allReviewerComments.Add(reviewerCommentList);
            }


            var shortlistViewModel = new ViewShortlistDetailsViewModel()
            {
                nominationId = nomination.Id,
                userName = nominationModel.DisplayName,
                totalCredits = nominationModel.Credits,
                Manager = _nominationService.GetManagerNameOfCurrentNomination(reviews.FirstOrDefault().NominationId),
                projectOrDepartment = nomination.ProjectID != null ?
                                           _nominationService.GetProjectNameOfCurrentNomination(nomination.Id) :
                                           _nominationService.GetDeptNameOfCurrentNomination(nomination.Id),
                nominationComment = nomination.Comment,
                IsShortlisted=nominationModel.IsShortlisted,
                IsWinner=nominationModel.IsWinner,
                reviewerComments = allReviewerComments,
                Criterias = _nominationService.GetCriteriaForNomination(nomination.Id),
                ManagerComments = nomination.ManagerComments.ToList(),

            };

            return View(shortlistViewModel);
        }

        [HttpPost]
        public bool ShortlistNomination(int nominationId)
        {
            try
            {
                _resultService.ShortlistNomination(nominationId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool SelectWinner(int nominationId)
        {
            try
            {
                _resultService.SelectWinner(nominationId);
                return true;                                                                               
            }
            catch
            {
                return false;
            }
        }

    }
}