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
        private readonly IReviewService _reviewService;
        private readonly INominationService _nominationService;
        private readonly ICommonDbService _commonDbService;
        private readonly ICommonDataBaseContext _commonDbContext;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.Encourage.DAL.Interfaces.IDataContextFactory _dataContextFactory;

        public ReviewController(INominationService nominationService, ICommonDbService commonDbService, Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, IAwardService awardService, IReviewService reviewService)
        {
            _commonDbService = commonDbService;
            _commonDbContext = _commonDbService.GetCommonDataBaseContext();
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
            _reviewService = reviewService;
            _nominationService = nominationService;
        }

        [HttpGet]
        public ActionResult ReviewFeedbackList()
        {
            var reviewFeedbacks = new List<ReviewFeedbackListViewModel>();
            var uniqueReviewedNomination = _encourageDatabaseContext.Query<Review>().GroupBy(x => x.NominationId).Select(group => group.FirstOrDefault()).ToList();

            foreach (var reviewNomination in uniqueReviewedNomination)
            {
                var nomination = _encourageDatabaseContext.Query<Nomination>().Where(x => x.Id == reviewNomination.NominationId).SingleOrDefault();
                var awardCode = _encourageDatabaseContext.Query<Award>().Where(award => award.Id == nomination.AwardId).SingleOrDefault().Code;
                var nominee = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = nomination.NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();

                var totalCreditPoints = _encourageDatabaseContext.Query<ReviewerComment>().
                                     Where(model => model.NominationId == nomination.Id).
                                     Sum(model => model.Credit.Value);

                reviewFeedbacks.Add(
                     new ReviewFeedbackListViewModel()
                       {
                           AwardName = awardCode,
                           Credits = totalCreditPoints,
                           DisplayName = nominee.DisplayName,
                           Intials = nominee.FirstName.Substring(0, 1) + "" + nominee.LastName.Substring(0, 1),
                           NominationTime = nominationTimeToDisplay,
                           NominationId = nomination.Id
                       }
                    );
            }


            ////var allReviewedNominations = _encourageDatabaseContext.Query<Review>("Nomination").Where(model => model.IsSubmited == true).ToList();
            ////var uniqueReviewedNomination=allReviewedNominations.GroupBy(x=>x.NominationId).Select(m=>m.FirstOrDefault()).ToList();

            //foreach (var reviewedNomination in uniqueReviewedNomination)
            //{
            //    var awardCode = _encourageDatabaseContext.Query<Award>().Where(award => award.Id == reviewedNomination.Nomination.AwardId).SingleOrDefault().Code;
            //    var nominee = _commonDbContext.Query<User>().Where(u => u.ID == reviewedNomination.Nomination.UserId).FirstOrDefault();
            //    var nominationTime = reviewedNomination.Nomination.NominationDate;
            //    string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
            //    var totalCreditPoints = _encourageDatabaseContext.Query<ReviewerComment>().
            //                            Where(model => model.NominationId == reviewedNomination.NominationId).
            //                            Sum(model => model.Credit.Value);


            //    reviewFeedbacks.Add(
            //          new ReviewFeedbackListViewModel()
            //            {
            //                AwardName = awardCode,
            //                Credits = totalCreditPoints,
            //                DisplayName = nominee.DisplayName,
            //                Intials = nominee.FirstName.Substring(0, 1) + "" + nominee.LastName.Substring(0, 1),
            //                NominationTime = nominationTimeToDisplay,
            //                NominationId = reviewedNomination.Nomination.Id
            //            }
            //         );
            //}

            return View(reviewFeedbacks);
        }


        [HttpGet]
        public ActionResult ViewNominationForShortlist(ReviewFeedbackListViewModel nominationModel)
        {
            var reviews = _reviewService.GetReviewsForNomination(nominationModel.NominationId);
            var nomination = _nominationService.GetNomination(reviews.FirstOrDefault().NominationId);
            var allReviewerComments = new List<List<ReviewerCommentViewModel>>();


            foreach (var review in reviews)
            {
                var allreviewerComment = _encourageDatabaseContext.Query<ReviewerComment>().Where(model => model.ReviewId == review.Id);
              
                var reviewerCommentList = new List<ReviewerCommentViewModel>();
                foreach (var reviewerComment in allreviewerComment)
                {
                    var singleReviewerComent = new ReviewerCommentViewModel()
                      {
                          CriteriaID = reviewerComment.CriteriaId,
                          Comment = reviewerComment.Comment,
                          Credit = Convert.ToBoolean(reviewerComment.Credit.Value)
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
                reviewerComments = allReviewerComments,
                Criterias = _nominationService.GetCriteriaForNomination(nomination.Id),
                ManagerComments=nomination.ManagerComments.ToList(),
               
            };

            return View(shortlistViewModel);
        }

    }
}