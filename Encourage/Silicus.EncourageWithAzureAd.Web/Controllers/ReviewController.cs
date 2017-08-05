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
using Silicus.EncourageWithAzureAd.Web.Enums;
using WebGrease.Css.Ast.Selectors;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly IResultService _resultService;
        private readonly IReviewService _reviewService;
        private readonly INominationService _nominationService;
        private readonly ICommonDataBaseContext _commonDbContext;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly IEmailNotificationOfWinner _emailNotificationOfWinner;
        private readonly ILogger _logger;
        private readonly ICustomDateService _customDateService;

        public ReviewController(IResultService resultService, INominationService nominationService, ICommonDbService commonDbService, Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, IAwardService awardService,
            IReviewService reviewService, IEmailNotificationOfWinner EmailNotificationOfWinner, ILogger logger, ICustomDateService customDateService)
        {
            _commonDbContext = commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
            _reviewService = reviewService;
            _nominationService = nominationService;
            _resultService = resultService;
            _emailNotificationOfWinner = EmailNotificationOfWinner;
            _logger = logger;
            _customDateService = customDateService;
        }

        public ActionResult GetProcessesToLockOrUnlock(int awardId, string status)
        {
            var processesToLockOrUnlock = new List<Encourage.Models.Configuration>();
            if (status == ConfigurationManager.AppSettings["Lock"])
            {
                processesToLockOrUnlock = _reviewService.GetProcessesToLock(awardId);
            }
            else if (status == ConfigurationManager.AppSettings["Unlock"])
            {
                processesToLockOrUnlock = _reviewService.GetProcessesToUnlock(awardId);
            }

            var data = new List<ProcessesToLockOrUnLockViewModel>();
            if (processesToLockOrUnlock.Count > 0)
            {
                foreach (var processToLock in processesToLockOrUnlock)
                {
                    data.Add(new ProcessesToLockOrUnLockViewModel() { Id = processToLock.Id, Name = processToLock.configurationKey });
                }
            }

            return View("~/Views/Review/Shared/_ProcessesToLockOrUnlock.cshtml", data);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ReviewFeedbackList()
        {
            _logger.Log("Review-ReviewFeedbackList-GET");
            int awardType = 0;
            var shortListedNominations = new ShortlistedNominationViewModel();
            var reviewFeedbackList = ReviewFeedbackList(true, awardType);
            var awards = _encourageDatabaseContext.Query<Award>().ToList();
            foreach (var award in awards)
            {
                shortListedNominations.Awards.Add(new LockAwardViewModel { Id = award.Id, Code = award.Code, Name = award.Name });
            }
            shortListedNominations.ReviewFeedbackList = reviewFeedbackList;
            return View(shortListedNominations);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult GetReviewFeedbackListPartialView(bool forCurrentMonth, int awardType)
        {
            _logger.Log("Review-ReviewFeedbackList-GET");
            var reviewFeedbackList = ReviewFeedbackList(forCurrentMonth, awardType);
            return PartialView("~/Views/Review/Shared/_reviewFeedbackList.cshtml", reviewFeedbackList);
        }

        private List<ReviewFeedbackListViewModel> ReviewFeedbackList(bool forCurrentMonth, int awardType)
        {
            _logger.Log("Review-ReviewFeedbackList-private-GET");

            var reviewFeedbacks = new List<ReviewFeedbackListViewModel>();
            var awardDetails = _encourageDatabaseContext.Query<Award>().FirstOrDefault(x => x.Id == awardType);

            DateTime toBeComparedDate = DateTime.Now;

            List<Shortlist> shortlistedNominations = new List<Shortlist>();

            if (awardType != 0)
            {
                if (awardDetails != null)
                {
                    switch (awardDetails.Code)
                    {
                        case "SOM":
                            toBeComparedDate = _customDateService.GetCustomDate(awardType);

                            shortlistedNominations = _encourageDatabaseContext.Query<Shortlist>()
                                .Where(r =>
                                    r.Nomination.AwardId == awardType &&
                                    (forCurrentMonth ? (r.Nomination.NominationDate.Value.Month == toBeComparedDate.Month && r.Nomination.NominationDate.Value.Year == toBeComparedDate.Year) : (r.Nomination.NominationDate < toBeComparedDate)))
                                .GroupBy(x => x.NominationId)
                                .Select(group => @group.FirstOrDefault()).ToList();
                            break;
                        case "PINNACLE":
                            toBeComparedDate = _customDateService.GetCustomDate(awardType);

                            shortlistedNominations = _encourageDatabaseContext.Query<Shortlist>()
                                .Where(r =>
                                    r.Nomination.AwardId == awardType &&
                                    (forCurrentMonth ? (r.Nomination.NominationDate.Value.Year == toBeComparedDate.Year) : (r.Nomination.NominationDate.Value.Year < toBeComparedDate.Year)))
                                .GroupBy(x => x.NominationId)
                                .Select(group => @group.FirstOrDefault()).ToList();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                var listOfAwards = _encourageDatabaseContext.Query<Award>().ToList();
                foreach (var award in listOfAwards)
                {
                    List<Shortlist> listOfNominations;
                    switch (award.Code)
                    {
                        default:
                        case "SOM":
                            toBeComparedDate = _customDateService.GetCustomDate(award.Id);
                            listOfNominations = _encourageDatabaseContext.Query<Shortlist>()
                            .Where(r =>
                                r.Nomination.AwardId == award.Id &&
                                (forCurrentMonth ? (r.Nomination.NominationDate.Value.Month == toBeComparedDate.Month && r.Nomination.NominationDate.Value.Year == toBeComparedDate.Year) : (r.Nomination.NominationDate < toBeComparedDate)))
                                    .GroupBy(x => x.NominationId)
                                .Select(group => group.FirstOrDefault()).ToList();
                            break;
                        case "PINNACLE":
                            toBeComparedDate = _customDateService.GetCustomDate(award.Id);
                            listOfNominations = _encourageDatabaseContext.Query<Shortlist>()
                            .Where(r =>
                                    r.Nomination.AwardId == award.Id &&
                                    (forCurrentMonth ? (r.Nomination.NominationDate.Value.Year == toBeComparedDate.Year) : (r.Nomination.NominationDate.Value.Year < toBeComparedDate.Year)))
                            .GroupBy(x => x.NominationId)
                            .Select(group => group.FirstOrDefault()).ToList();
                            break;
                    }

                    shortlistedNominations.AddRange(listOfNominations);
                }
            }

            foreach (var shortlistedNomination in shortlistedNominations)
            {
                var isShortlisted = false;
                var isWinner = false;
                var nomination = _encourageDatabaseContext.Query<Nomination>().SingleOrDefault(x => x.Id == shortlistedNomination.NominationId);
                var award = _encourageDatabaseContext.Query<Award>().SingleOrDefault(a => a.Id == nomination.AwardId);
                if (award != null && nomination != null)
                {
                    var awardFrequency = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                    var isHistorical = IsHistoricalNomination(nomination);
                    var isAwardLocked = GetLockStatusOfAward(nomination.AwardId);
                    var awardCode = award.Code;
                    var nominee = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);
                    var nominationTime = Convert.ToDateTime(nomination.NominationDate);
                    string nominationTimeToDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(nominationTime.Month) + "-" + nominationTime.Year.ToString();
                    var totalReviews = _reviewService.GetReviewsForNomination(shortlistedNomination.NominationId).Count();

                    var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(model => model.NominationId == nomination.Id).ToList();
                    var managerComments = _encourageDatabaseContext.Query<ManagerComment>().Where(model => model.NominationId == nomination.Id).ToList();

                    decimal totalCreditPoints = 0;
                    decimal averageCredits;

                    foreach (var rc in reviewerComments)
                    {
                        var managerCommnet = managerComments.FirstOrDefault(mc => mc.CriteriaId == rc.CriteriaId);
                        totalCreditPoints += (Convert.ToInt32(rc.Credit) * (managerCommnet != null ? managerCommnet.Weightage : 0) / 100m);
                    }
                    averageCredits = totalCreditPoints / (totalReviews <= 0 ? 1 : totalReviews);
                    var checkResultStatus = _resultService.IsShortlistedOrWinner(nomination.Id);
                    if (checkResultStatus == 1)
                    {
                        isWinner = true;
                    }
                    else if (checkResultStatus == 2)
                    {
                        isShortlisted = true;
                    }

                    if (nominee != null)
                    {
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
                                NominatedMonth = nominationTime.Month,
                                AwardFrequencyCode = awardFrequency.Code,
                                IsHistorical = isHistorical,
                                IsAwardLocked = isAwardLocked,
                                EmployeeId = nominee.EmployeeID,
                                UserId = nominee.ID
                            }
                            );
                    }
                }
            }

            var reviewFeedbackList = reviewFeedbacks.OrderByDescending(o => o.NominatedMonth).ToList();
            return reviewFeedbackList;
        }

        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult RejectAll(int awardId)
        {
            _logger.Log("Review-RejectAll-GET");
            
            var nominationInCurrentAwardPeriod = new List<Nomination>();
            var nominationIdsToReject = new List<int>();
            var awards = _awardService.GetAllAwards();
            if (awardId == 0)
            {
                var customeDatesFoAllAwards = _customDateService.GetAllCustomDates();
                if (customeDatesFoAllAwards.Count > 0)
                {
                var awardIsWithCustomdate = customeDatesFoAllAwards.Select( x => x.AwardId ).ToList();
                    foreach (var customDate in customeDatesFoAllAwards)
                    {
                        nominationInCurrentAwardPeriod.AddRange(_nominationService.GetNominationsByDate(customDate.Month.Value, customDate.Year.Value, customDate.AwardId));
                    }

                    awards = awards.Where(x => !awardIsWithCustomdate.Contains(x.Id));

                    nominationInCurrentAwardPeriod.AddRange(GetnominationInCurrentAwardPeriod(awards.ToList()));

                }
                else
                {
                    nominationInCurrentAwardPeriod = GetnominationInCurrentAwardPeriod(awards.ToList());
                }
               
            }
            else
            {
                var customDate = _customDateService.GetCustomDate(awardId);

                nominationInCurrentAwardPeriod = _nominationService.GetNominationsByDate(customDate.Month, customDate.Year, awardId);
            }

            foreach (var item in nominationInCurrentAwardPeriod)
            {
                var shortlist = _encourageDatabaseContext.Query<Shortlist>().Any(x => x.NominationId == item.Id && x.IsWinner == false);
                if (shortlist)
                {
                    nominationIdsToReject.Add(item.Id);
                }
            }
            
            foreach (var nominationIdToreject in nominationIdsToReject)
            {
                _resultService.UnShortlistNomination(nominationIdToreject);
            }
            return RedirectToAction("Index", "Home");
        }


        private List<Nomination> GetnominationInCurrentAwardPeriod(List<Award> awards)
        {
            var nominationInCurrentAwardPeriod = new List<Nomination>();
            foreach (var award in awards)
            {
                nominationInCurrentAwardPeriod.AddRange(_nominationService.GetNominationsByDate(DateTime.Now.Month - 1, DateTime.Now.Year, award.Id));
            }
            return nominationInCurrentAwardPeriod;
        }
        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ViewNominationForShortlist(int nominationId)
        {
            _logger.Log("Review-ViewNominationForShortlist-GET");
            ViewBag.NominationLockStatus = _nominationService.GetNominationLockStatus();
            var reviews = _reviewService.GetReviewsForNomination(nominationId).ToList();
            var nomination = _nominationService.GetNomination(nominationId);
            var allReviewerComments = new List<List<ReviewerCommentViewModel>>();
            decimal totalCreditPoints = 0;

            var isHistoricalNomination = IsHistoricalNomination(nomination);

            foreach (var r in reviews)
            {
                foreach (var rc in r.ReviewerComments)
                {
                    var managerCommnet = nomination.ManagerComments.FirstOrDefault(mc => mc.CriteriaId == rc.CriteriaId);
                    totalCreditPoints += (Convert.ToInt32(rc.Credit) * (managerCommnet != null ? managerCommnet.Weightage : 0) / 100m);
                }
            }

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
                        ReviewerName = reviewerObj != null ? reviewerObj.DisplayName : string.Empty
                    };
                    reviewerCommentList.Add(singleReviewerComent);
                }
                allReviewerComments.Add(reviewerCommentList);
            }

            var awardOfCurrentNomination = _awardService.GetAwardFromNominationId(nominationId);
            var isLocked = GetLockStatusOfAward(awardOfCurrentNomination.Id);

            var isShortlisted = false;
            var isWinner = false;
            var checkResultStatus = _resultService.IsShortlistedOrWinner(nomination.Id);
            if (checkResultStatus == 1)
            {
                isWinner = true;
            }
            else if (checkResultStatus == 2)
            {
                isShortlisted = true;
            }

            var nominee = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);

            var loggedInAdminId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            var hrAdminsFeedback = _resultService.GetHrAdminsFeedbackForEmployee(loggedInAdminId, nomination.Id);

            var shortlistViewModel = new ViewShortlistDetailsViewModel()
            {
                NominationId = nomination.Id,
                UserName = nominee != null ? nominee.DisplayName : string.Empty,
                TotalCredits = totalCreditPoints,
                Manager = _nominationService.GetManagerNameOfCurrentNomination(nominationId),
                ProjectOrDepartment = nomination.ProjectID != null ?
                    _nominationService.GetProjectNameOfCurrentNomination(nomination.Id) :
                    _nominationService.GetDeptNameOfCurrentNomination(nomination.Id),
                NominationComment = nomination.Comment,
                IsShortlisted = isShortlisted,
                IsWinner = isWinner,
                ReviewerComments = allReviewerComments,
                Criterias = _nominationService.GetCriteriaForNomination(nomination.Id),
                ManagerComments = nomination.ManagerComments.ToList(),
                IsLocked = isLocked,
                HrAdminsfeedback = hrAdminsFeedback,
                HrAdminName = _resultService.GetLoggedInUserName(User.Identity.Name),
                IsHistoricalNomination = isHistoricalNomination
            };

            return View(shortlistViewModel);
        }

        private bool IsHistoricalNomination(Nomination nomination)
        {
            var customDate = _customDateService.GetCustomDate(nomination.AwardId);
            var currentNomination = nomination;
            var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(n => n.Id == currentNomination.AwardId);
            var nominationDate = Convert.ToDateTime(currentNomination.NominationDate);
            bool isHistoricalNomination = false;

            if (award != null)
            {
                switch (award.Code)
                {
                    default:
                    case "SOM":
                        var prevMonth = customDate;
                        if (nominationDate.Year < prevMonth.Year)
                        {
                            isHistoricalNomination = true;
                        }
                        break;
                    case "PINNACLE":
                        if (nominationDate.Year < customDate.Year)
                        {
                            isHistoricalNomination = true;
                        }
                        break;
                }
            }
            return isHistoricalNomination;
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
            _resultService.SelectWinner(nominationId, winningComment, feedback, adminId);
            _emailNotificationOfWinner.Process();

            return true;
        }

        public bool GetLockStatusOfAward(int awardId)
        {
            var nominationLockStatus = _nominationService.GetAwardNominationLockStatus(awardId);
            var reviewLockStatus = _reviewService.GetAwardReviewLockStatus(awardId);

            return (nominationLockStatus && reviewLockStatus);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult LockNomination()
        {
            _logger.Log("Review-LockNomination-GET");
            var awards = _nominationService.GetAwardstoUnLockOrUnlock(ConfigurationManager.AppSettings["Lock"]);
            var awardsToLock = new List<LockAwardViewModel>();
            foreach (var award in awards)
            {
                var configurationKey = string.Empty;
                var configuration = award.Configurations.FirstOrDefault();
                if (configuration != null)
                {
                    configurationKey = configuration.configurationKey;
                }

                awardsToLock.Add(new LockAwardViewModel { Id = award.Id, Code = award.Code, Name = award.Name, FrequencyId = award.FrequencyId, ConfigurationKey = configurationKey });
            }

            return PartialView("~/Views/Review/Shared/_LockNominations.cshtml", awardsToLock);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public JsonResult LockNomination(int[] awardIds, int[] processIds)
        {
            _logger.Log("Review-LockNomi-Post");
            List<string> lockKeys = new List<string>();
            foreach (var processId in processIds)
            {
                lockKeys.Add(_reviewService.GetConfigurationById(processId).configurationKey);
            }
            var data = new List<Award>();

            foreach (var lockKey in lockKeys)
            {
                if (lockKey == ConfigurationManager.AppSettings["NominationLockKey"])
                {
                    data = _nominationService.LockNominations(awardIds.ToList());
                }
                else if (lockKey == ConfigurationManager.AppSettings["ReviewLockKey"])
                {
                    data = _reviewService.LockReview(awardIds.ToList());
                }
            }

            return Json(data);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult UnlockNomination()
        {
            _logger.Log("Review-UnlockNomination-GET");
            var awards = _nominationService.GetAwardstoUnLockOrUnlock(ConfigurationManager.AppSettings["UnLock"]);
            var awardsToUnlock = new List<LockAwardViewModel>();
            foreach (var award in awards)
            {
                var configurationKey = string.Empty;
                var configuration = award.Configurations.FirstOrDefault();
                if (configuration != null)
                {
                    configurationKey = configuration.configurationKey;
                }
                awardsToUnlock.Add(new LockAwardViewModel { Code = award.Code, FrequencyId = award.FrequencyId, Id = award.Id, Name = award.Name, ConfigurationKey = configurationKey });
            }
            return PartialView("~/Views/Review/Shared/_LockNominations.cshtml", awardsToUnlock);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public JsonResult UnlockNomination(int[] awardIds, int[] processIds)
        {
            _logger.Log("Review-UnlockNomination-GET");
            List<string> unlockKeys = new List<string>();
            foreach (var processId in processIds)
            {
                unlockKeys.Add(_reviewService.GetConfigurationById(processId).configurationKey);
            }
            var data = new List<Award>();
            foreach (var unlockKey in unlockKeys)
            {
                if (unlockKey == ConfigurationManager.AppSettings["NominationLockKey"])
                {
                    data = _nominationService.UnLockNominations(awardIds.ToList());
                }
                else if (unlockKey == ConfigurationManager.AppSettings["ReviewLockKey"])
                {
                    data = _reviewService.UnLockReview(awardIds.ToList());
                }
            }
            return Json(data);
        }

        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ConsolidatedNominations(ConsolidatedNominationsViewModel consolidatedNominationsViewModel)
        {
            var customDate = _customDateService.GetCustomDate(consolidatedNominationsViewModel.AwardId);
            var awards = _awardService.GetAllAwards();
            var currentAwardFrequency = consolidatedNominationsViewModel.AwardId > 0 ? GetAwardFrequency(consolidatedNominationsViewModel.AwardId).Data.ToString() : "";
            if (consolidatedNominationsViewModel.AwardId == 0)
            {
                consolidatedNominationsViewModel = new ConsolidatedNominationsViewModel();
                var award = awards.FirstOrDefault(a => a.Code == "SOM");
                if (award != null)
                {
                    customDate = _customDateService.GetCustomDate(award.Id);
                    consolidatedNominationsViewModel.AwardId = award.Id;
                    consolidatedNominationsViewModel.AwardMonth = customDate.Month;
                    consolidatedNominationsViewModel.AwardYear = customDate.Year;
                }
            }

            consolidatedNominationsViewModel.Criterias = _encourageDatabaseContext.Query<Criteria>().Where(c => c.AwardId == consolidatedNominationsViewModel.AwardId).ToList();
            consolidatedNominationsViewModel.Reviewers = new List<ReviewerViewModel>();
            consolidatedNominationsViewModel.Nominations = new List<SubmittedNomination>();
            consolidatedNominationsViewModel.ListOfAwards = new SelectList(awards, "Id", "Name");
            List<UtilityUserRoles> activeReviewers;
            var utility = _commonDbContext.Query<Utility>().FirstOrDefault(r => r.Name == "Encourage");
            if (consolidatedNominationsViewModel.AwardMonth == customDate.Month && consolidatedNominationsViewModel.AwardYear == customDate.Year)
            {
                activeReviewers = _commonDbContext.Query<UtilityUserRoles>().Where(x => x.IsActive && x.UtilityId == utility.Id && x.RoleId == (int)Roles.Reviewer).ToList();
            }
            else
            {
                activeReviewers = _commonDbContext.Query<UtilityUserRoles>().Where(x => x.RoleId == (int)Roles.Reviewer && x.UtilityId == utility.Id).ToList();
            }
          
           // var reviewers = _encourageDatabaseContext.Query<Reviewer>().ToList();

            foreach (var reviewer in activeReviewers)
            {
                var reviewerObj = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == reviewer.UserId);
                consolidatedNominationsViewModel.Reviewers.Add(new ReviewerViewModel
                {
                    Id = reviewer.Id,
                    UserId = reviewer.UserId,
                    ReviewerName = reviewerObj != null ? reviewerObj.FirstName + " " + reviewerObj.LastName : ""
                });
            }
            var nominations = currentAwardFrequency == FrequencyCode.YEAR.ToString() ? _encourageDatabaseContext.Query<Nomination>().Include(a => a.ManagerComments).Include(b => b.ReviewerComments).Where(N => N.IsSubmitted == true && N.NominationDate.Value.Year == consolidatedNominationsViewModel.AwardYear && N.AwardId == consolidatedNominationsViewModel.AwardId).ToList() : _encourageDatabaseContext.Query<Nomination>().Include(a => a.ManagerComments).Include(b => b.ReviewerComments).Where(N => N.IsSubmitted == true && N.NominationDate.Value.Month == consolidatedNominationsViewModel.AwardMonth && N.NominationDate.Value.Year == consolidatedNominationsViewModel.AwardYear && N.AwardId == consolidatedNominationsViewModel.AwardId).ToList();
            foreach (var nomination in nominations)
            {
                var nominee = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);
                var isWinner = false;
                var checkResultStatus = _resultService.IsShortlistedOrWinner(nomination.Id);
                if (checkResultStatus == 1)
                {
                    isWinner = true;
                }
                var submittednomination = new SubmittedNomination
                {
                    NominationId = nomination.Id,
                    UserName = nominee != null ? nominee.FirstName + " " + nominee.LastName : "",
                    ManagerComments = nomination.ManagerComments.ToList(),
                    ReviewerComments = new List<ReviewerCommentViewModel>(),
                    IsWinner = isWinner,
                    IsHistoricalNomination = IsHistoricalNomination(nomination)
                };

                submittednomination.IsShortListed = _encourageDatabaseContext.Query<Shortlist>().Any(s => s.NominationId == nomination.Id);

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
                consolidatedNominationsViewModel.Nominations.Add(submittednomination);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ConsolidatedNominationsPartialView", consolidatedNominationsViewModel);
            }
            else
            {
                consolidatedNominationsViewModel.CustomDate = customDate;
                return View("ConsolidatedNominations", consolidatedNominationsViewModel);
            }
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult SaveFinalScore(ConsolidatedNominationsViewModel consolidatedNominationsViewModel)
        {
            foreach (var nomination in consolidatedNominationsViewModel.Nominations)
            {
                foreach (var finalComment in nomination.ManagerComments)
                {
                    var commentDb = _encourageDatabaseContext.Query<ManagerComment>().FirstOrDefault(mc => mc.NominationId == finalComment.NominationId && mc.CriteriaId == finalComment.CriteriaId);
                    if (commentDb != null)
                    {
                        commentDb.AdminComment = finalComment.AdminComment;
                        commentDb.FinalScore = finalComment.FinalScore;
                    }

                    _encourageDatabaseContext.Update(commentDb);
                }
            }

            return Json(true);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult ShortList(int nominationId)
        {
            try
            {
                var adminId = _awardService.GetUserIdFromEmail(User.Identity.Name);
                _resultService.ShortlistNomination(nominationId, adminId);
                _emailNotificationOfWinner.Process();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }
        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult SetAwardPeriod()
        {
            var customdateViewModel = new CustomdateViewmodel();
            var allAwards = _awardService.GetAllAwards().ToList();
            customdateViewModel.Awards = allAwards;
            customdateViewModel.Months = Enumerable.Range(1, 12).ToList();
            customdateViewModel.Years = Enumerable.Range(2010, (DateTime.Today.Year + 1) - 2010).ToList();
            return View(customdateViewModel);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public JsonResult GetAwardFrequency(int awardId)
        {
            var currentAward = _awardService.GetAwardById(awardId);
            var awardFrequency = _nominationService.GetAwardFrequencyById(currentAward.FrequencyId);
            return Json(awardFrequency.Code, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetAwardPeriod(int awardId, int month, int year, bool isApplicable)
        {
            return Json(_customDateService.SetCustomDate(awardId, month, year, isApplicable), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ResetAwardPeriod(int awardId)
        {
            return Json(_customDateService.ReSetCustomDate(awardId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCustomDateDetailsForAward(int awardId)
        {
            CustomDate customDateDetails = _customDateService.CustomDateDetailsForAward(awardId);
            if (customDateDetails == null)
            {
                customDateDetails = new CustomDate();
                customDateDetails.Year = DateTime.Now.Year;
                customDateDetails.Month = DateTime.Now.Month;
                customDateDetails.IsApplicable = true;
                customDateDetails.MonthsToSubtract = 0;

            }
            customDateDetails.Award = _awardService.GetAwardById(awardId);

            var details = new
            {
                AwardName = customDateDetails.Award.Name,
                Year = customDateDetails.Year,
                Month = customDateDetails.Month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(customDateDetails.Month.Value),
                IsApplicable = customDateDetails.IsApplicable,
                MonthsToSubtract = customDateDetails.MonthsToSubtract
            };
            return Json(details, JsonRequestBehavior.AllowGet);
        }
    }
}