using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
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
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ReviewFeedbackList()
        {
            var reviewFeedbacks = new List<ReviewFeedbackListViewModel>();
            //if (string.IsNullOrEmpty(rejectAll))
            //{
                var uniqueReviewedNomination = _encourageDatabaseContext.Query<Review>().Where(r => r.IsSubmited == true).GroupBy(x => x.NominationId).Select(group => group.FirstOrDefault()).ToList();

                foreach (var reviewNomination in uniqueReviewedNomination)
                {
                    var isShortlisted = false;
                    var isWinner = false;
                    var nomination = _encourageDatabaseContext.Query<Nomination>().Where(x => x.Id == reviewNomination.NominationId).SingleOrDefault();
                    var awardCode = _encourageDatabaseContext.Query<Award>().Where(award => award.Id == nomination.AwardId).SingleOrDefault().Code;
                    var nominee = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                    var nominationTime = nomination.NominationDate;
                    string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                    var totalReviews = _reviewService.GetReviewsForNomination(reviewNomination.NominationId).Count();

                    var totalCreditPoints = _encourageDatabaseContext.Query<ReviewerComment>().
                                         Where(model => model.NominationId == nomination.Id).
                                         Sum(model => model.Credit.Value);
                    var averageCredits = 0;
                    if (totalReviews != 0 && totalCreditPoints != 0)
                    {
                         averageCredits = totalCreditPoints / totalReviews;
                    }

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
                               IsWinner = isWinner,
                               numberOfReviews = totalReviews,
                               averageCredits = averageCredits
                           }
                        );
                }
           // }
            return View(reviewFeedbacks);
        }

        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult RejectAll()
        {
            var rejectAllRviews = _encourageDatabaseContext.Query<Review>().Where(r => r.IsSubmited == true).ToList();
          
            var shortlist = _encourageDatabaseContext.Query<Shortlist>().Where(s => s.IsWinner == true);

            foreach (var shortListedEmployee in shortlist)
            {
                rejectAllRviews.RemoveAll(r => r.NominationId == shortListedEmployee.NominationId);
            }

            foreach (var item in rejectAllRviews)
            {
              
               var Comments = _encourageDatabaseContext.Query<ReviewerComment>().Where(rc => rc.ReviewId == item.Id).ToList();

               foreach (var reviewComment in Comments)
               {
                   _encourageDatabaseContext.Delete<ReviewerComment>(reviewComment);
               }

               _encourageDatabaseContext.Delete<Review>(item);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
      [CustomeAuthorize(AllowedRole = "Admin")]
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
                IsShortlisted = nominationModel.IsShortlisted,
                IsWinner = nominationModel.IsWinner,
                reviewerComments = allReviewerComments,
                Criterias = _nominationService.GetCriteriaForNomination(nomination.Id),
                ManagerComments = nomination.ManagerComments.ToList(),

            };

            return View(shortlistViewModel);
        }

        [HttpPost]
       [CustomeAuthorize(AllowedRole = "Admin")]
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

 [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult RemoveShortlistNomination(int nominationId)
        {
            try
            {
                _resultService.UnShortlistNomination(nominationId);
                return RedirectToAction("ReviewFeedbackList");
            }
            catch
            {
                return RedirectToAction("ReviewFeedbackList");
            }
        }

        [HttpPost]
       [CustomeAuthorize(AllowedRole = "Admin")]
        public bool SelectWinner(int nominationId,string winningComment)
        {
            try
            {
                _resultService.SelectWinner(nominationId, winningComment);
                return true;                                                                               
            }
            catch
            {
                return false;
            }
        }

    }
}