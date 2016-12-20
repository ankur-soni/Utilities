using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
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
        private readonly IEmailNotificationOfWinner _emailNotificationOfWinner;
        private readonly ILogger _logger;

        public ReviewController(IResultService resultService, INominationService nominationService, ICommonDbService commonDbService, Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, IAwardService awardService, IReviewService reviewService, IEmailNotificationOfWinner EmailNotificationOfWinner, ILogger logger)
        {
            _commonDbService = commonDbService;
            _commonDbContext = _commonDbService.GetCommonDataBaseContext();
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
            _reviewService = reviewService;
            _nominationService = nominationService;
            _resultService = resultService;
            _emailNotificationOfWinner = EmailNotificationOfWinner;
            _logger = logger;
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ReviewFeedbackList()
        {
            _logger.Log("Review-ReviewFeedbackList-GET");
            var reviewFeedbackList = ReviewFeedbackList(true);

            return View(reviewFeedbackList);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult GetReviewFeedbackListPartialView(bool forCurrentMonth)
        {
            _logger.Log("Review-ReviewFeedbackList-GET");
            var reviewFeedbackList = ReviewFeedbackList(forCurrentMonth);
            // }
            return PartialView("~/Views/Review/Shared/_reviewFeedbackList.cshtml", reviewFeedbackList);
        }

        private List<ReviewFeedbackListViewModel> ReviewFeedbackList(bool forCurrentMonth)
        {
            _logger.Log("Review-ReviewFeedbackList-private-GET");
            var reviewFeedbacks = new List<ReviewFeedbackListViewModel>();

            var today = DateTime.Today;
            var prevMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);

            var uniqueReviewedNomination = _encourageDatabaseContext.Query<Review>().Where(r => r.IsSubmited == true && (forCurrentMonth ? (r.Nomination.NominationDate >= prevMonth) : (r.Nomination.NominationDate < prevMonth))).GroupBy(x => x.NominationId).Select(group => group.FirstOrDefault()).ToList();

            foreach (var reviewNomination in uniqueReviewedNomination)
            {
                var isShortlisted = false;
                var isWinner = false;
                var nomination = _encourageDatabaseContext.Query<Nomination>().SingleOrDefault(x => x.Id == reviewNomination.NominationId);
                var award = _encourageDatabaseContext.Query<Award>().SingleOrDefault(a => a.Id == nomination.AwardId);
                var awardFrequency = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                if (award != null && nomination!= null)
                {
                    var awardCode = award.Code;
                    var nominee = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);
                    var nominationTime = nomination.NominationDate;
                    string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                    var totalReviews = _reviewService.GetReviewsForNomination(reviewNomination.NominationId).Count();



                    var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(model => model.NominationId == nomination.Id).ToList();
                    var managerComments = _encourageDatabaseContext.Query<ManagerComment>().Where(model => model.NominationId == nomination.Id).ToList();

                    decimal totalCreditPoints = 0;
                    decimal averageCredits = 0;

                    foreach (var rc in reviewerComments)
                    {
                        var managerCommnet = managerComments.FirstOrDefault(mc => mc.CriteriaId == rc.CriteriaId);
                        totalCreditPoints += (Convert.ToInt32(rc.Credit)*(managerCommnet != null ? managerCommnet.Weightage : 0)/100m);
                    }
                    averageCredits = totalCreditPoints / (totalReviews <= 0 ? 1 : totalReviews);
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
                            NumberOfReviews = totalReviews,
                            AverageCredits = averageCredits,
                            NominatedMonth = nominationTime.Value != null ? nominationTime.Value.Month : 0,
                            AwardFrequencyCode = awardFrequency.Code
                        }
                        );
                }
            }

            var reviewFeedbackList = reviewFeedbacks.OrderByDescending(o => o.NominatedMonth).ToList();
            return reviewFeedbackList;
        }

        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult RejectAll()
        {
            _logger.Log("Review-RejectAll-GET");
            var rejectAllRviews = _encourageDatabaseContext.Query<Review>().Where(r => r.IsSubmited == true && r.ReviewDate.Value.Month == DateTime.Now.Month && r.ReviewDate.Value.Year == DateTime.Now.Year).ToList();
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
                _resultService.UnShortlistNomination(item.NominationId);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ViewNominationForShortlist(ReviewFeedbackListViewModel nominationModel)
        {
            _logger.Log("Review-ViewNominationForShortlist-GET");
            ViewBag.NominationLockStatus = _nominationService.GetNominationLockStatus();
            var reviews = _reviewService.GetReviewsForNomination(nominationModel.NominationId).ToList();
            var nomination = _nominationService.GetNomination(reviews.FirstOrDefault().NominationId);
            var allReviewerComments = new List<List<ReviewerCommentViewModel>>();

            foreach (var review in reviews)
            {
                var allreviewerComment = _encourageDatabaseContext.Query<ReviewerComment>().Where(model => model.ReviewId == review.Id);
                var reviewer = _encourageDatabaseContext.Query<Reviewer>().FirstOrDefault(model => model.Id == review.ReviewerId);
                var reviewerObj = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == reviewer.UserId);

                var reviewerCommentList = new List<ReviewerCommentViewModel>();
                foreach (var reviewerComment in allreviewerComment)
                {
                    var singleReviewerComent = new ReviewerCommentViewModel()
                    {
                        CriteriaId = reviewerComment.CriteriaId,
                        Comment = reviewerComment.Comment,
                        Credit = Convert.ToInt32(reviewerComment.Credit),
                        ReviewerName = reviewerObj.DisplayName
                    };
                    reviewerCommentList.Add(singleReviewerComent);
                }
                allReviewerComments.Add(reviewerCommentList);
            }
            //  var isLocked = _nominationService.GetAllNominations().FirstOrDefault(x => (x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1) && x.NominationDate.Value.Year.Equals(DateTime.Now.Month > 1 ? DateTime.Now.Year : DateTime.Now.Year - 1))).IsLocked ?? false;
            var lockedAwards = _nominationService.GetNominationLockStatus();
            var isLocked = false;
            var awardOfCurrentNomination = _awardService.GetAwardFromNominationId(nominationModel.NominationId);
            foreach (var lockedAward in lockedAwards)
            {
                if (lockedAward.Id == awardOfCurrentNomination.Id)
                {
                    isLocked = true;
                }
            }

            var loggedInAdminId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            var hrAdminsFeedback = _resultService.GetHrAdminsFeedbackForEmployee(loggedInAdminId, nomination.Id);
            var shortlistViewModel = new ViewShortlistDetailsViewModel()
            {
                NominationId = nomination.Id,
                UserName = nominationModel.DisplayName,
                TotalCredits = nominationModel.Credits,
                Manager = _nominationService.GetManagerNameOfCurrentNomination(reviews.FirstOrDefault().NominationId),
                ProjectOrDepartment = nomination.ProjectID != null ?
                                           _nominationService.GetProjectNameOfCurrentNomination(nomination.Id) :
                                           _nominationService.GetDeptNameOfCurrentNomination(nomination.Id),
                NominationComment = nomination.Comment,
                IsShortlisted = nominationModel.IsShortlisted,
                IsWinner = nominationModel.IsWinner,
                ReviewerComments = allReviewerComments,
                Criterias = _nominationService.GetCriteriaForNomination(nomination.Id),
                ManagerComments = nomination.ManagerComments.ToList(),
                IsLocked = isLocked,
                HrAdminsfeedback = hrAdminsFeedback,
                HrAdminName = _resultService.GetLoggedInUserName(User.Identity.Name)
            };

            return View(shortlistViewModel);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public bool ShortlistNomination(int nominationId)
        {
            var adminId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            _logger.Log("Review-ShortlistNomination-POST");
            try
            {
                _resultService.ShortlistNomination(nominationId, adminId);
                _emailNotificationOfWinner.Process();
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
            _logger.Log("Review-RemoveShortlistNomination");
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
        public bool SelectWinner(int nominationId, string winningComment, string feedback)
        {
            _logger.Log("Review-SelectWinner-POST");
            var adminId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            try
            {
                _resultService.SelectWinner(nominationId, winningComment, feedback, adminId);
                _emailNotificationOfWinner.Process();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet]
        //public ActionResult LockNomination()
        //{
        //    _logger.Log("Review-LockNomination-GET");
        //    _nominationService.LockNominations();
        //    _reviewService.LockReview();

        //    return new EmptyResult();
        //}


        [HttpGet]
        public ActionResult LockNomination()
        {
            _logger.Log("Review-LockNomination-GET");
           var awards = _nominationService.GetAwardstoUnLockOrUnlock(ConfigurationManager.AppSettings["Lock"]);
            var awardsToLock = new List<LockAwardViewModel>();
            foreach (var award in awards)
            {
                awardsToLock.Add(new LockAwardViewModel { Id = award.Id, Code = award.Code, Name = award.Name, FrequencyId = award.FrequencyId});
            }
            return PartialView("~/Views/Review/Shared/_LockNominations.cshtml", awardsToLock);
        }

            [HttpPost]
            public JsonResult LockNomination(int[] awardIds)
            {
            _logger.Log("Review-LockNomi-Post");
                var data = _nominationService.LockNominations(awardIds.ToList());
                _reviewService.LockReview(awardIds.ToList());
                return Json(data);
           
            }

        [HttpGet]
        public ActionResult UnlockNomination()
        {
            _logger.Log("Review-UnlockNomination-GET");
           var awards = _nominationService.GetAwardstoUnLockOrUnlock(ConfigurationManager.AppSettings["UnLock"]);
            var awardsToUnlock = new List<LockAwardViewModel>();
            foreach (var award in awards)
            {
                awardsToUnlock.Add(new LockAwardViewModel { Code = award.Code, FrequencyId = award.FrequencyId, Id = award.Id, Name = award.Name});
            }
            return PartialView("~/Views/Review/Shared/_LockNominations.cshtml", awardsToUnlock);
        }

        [HttpPost]
        public JsonResult UnlockNomination(int[] awardIds)
        {
            _logger.Log("Review-UnlockNomination-GET");
            var lockedNominations = _nominationService.UnLockNominations(awardIds.ToList());
            var lockedReviews = _reviewService.UnLockReview(awardIds.ToList());
            return Json(lockedNominations);
        }

        public ActionResult ConsolidatedNominations(int awardId = 1)
        {
            var consolidatedNominations = new ConsolidatedNominationsViewModel
            {
                Criterias = _encourageDatabaseContext.Query<Criteria>().Where(c => c.AwardId == awardId).ToList(),
                Reviewers = _encourageDatabaseContext.Query<Reviewer>().ToList(),
                Nominations = new List<SubmittedNomination>()
            };

            var nominations = _encourageDatabaseContext.Query<Nomination>().Include(a => a.ManagerComments).Include(b => b.ReviewerComments).Where(N => N.IsSubmitted == true && N.NominationDate.Value.Month == (DateTime.Now.Month - 1) && N.NominationDate.Value.Year == DateTime.Now.Year).ToList();
            foreach (var nomination in nominations)
            {
                var nominee = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);

                var submittednomination = new SubmittedNomination
                {
                    NominationId = nomination.Id,
                    UserName = nominee != null ? nominee.FirstName + " " + nominee.LastName : "",
                    ManagerComments = nomination.ManagerComments.ToList(),
                    ReviewerComments = new List<ReviewerCommentViewModel>()
                };
                

                foreach (var reviewerComment in nomination.ReviewerComments)
                {
                    var managerComment = nomination.ManagerComments.FirstOrDefault(m => m.CriteriaId == reviewerComment.CriteriaId);
                    var reviewComment = new ReviewerCommentViewModel()
                    {
                        CriteriaId = reviewerComment.CriteriaId,
                        Comment = reviewerComment.Comment,
                        Credit = Convert.ToInt32(reviewerComment.Credit),
                        ReviewerId = reviewerComment.ReviewerId,
                        Weightage = managerComment != null ? managerComment.Weightage : 0
                    };

                    submittednomination.ReviewerComments.Add(reviewComment);
                }

                consolidatedNominations.Nominations.Add(submittednomination);
            }
            return View("ConsolidatedNominations", consolidatedNominations);
        }
    }
}