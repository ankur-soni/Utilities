using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using Silicus.EncourageWithAzureAd.Web.Enums;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
    public class NominationController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly IReviewService _reviewService;
        private readonly INominationService _nominationService;
        private readonly ICommonDataBaseContext _commonDbContext;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly TextInfo _textInfo;
        private readonly ILogger _logger;

        public NominationController(INominationService nominationService, Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory,
            ICommonDbService commonDbService, IAwardService awardService, IReviewService reviewService, ILogger logger)
        {
            _nominationService = nominationService;
            _commonDbContext = commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
            _reviewService = reviewService;
            _textInfo = new CultureInfo("en-US", false).TextInfo;
            _logger = logger;
        }

        #region Nomination
        [HttpGet]
        public ActionResult GetLockedAwardCategories()
        {
            return Json(_nominationService.GetNominationLockStatus(), JsonRequestBehavior.AllowGet);
        }

        // GET: Nomination/Create
        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult AddNomination()
        {
            _logger.Log("Nomination-AddNomination-GET");

            var model = new NominationViewModel();
            var userEmailAddress = User.Identity.Name;
            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress);

            if (projects.Any())
            {
                model.ProjectsUnderCurrentUser = new SelectList(projects, "Id", "Name");
            }

            var depts = _awardService.GetDepartmentsUnderCurrentUserAsManager(userEmailAddress);
            if (depts.Count > 0)
            {
                model.DepartmentsUnderCurrentUser = new SelectList(depts, "Id", "Name");
            }

            model.Resources = new SelectList(new List<User>(), "Id", "DisplayName");
            model.ListOfAwards = new SelectList(_awardService.GetAllAwards(), "Id", "Name");
            model.ManagerId = _awardService.GetUserIdFromEmail(userEmailAddress);

            return View(model);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult AddNomination(NominationViewModel model, string submit)
        {
            _logger.Log("Nomination-AddNomination-POST");
            try
            {
                _logger.Log("Nomination-AddNomination-POST-try");
                var awardOfCurrentNomination = _awardService.GetAwardById(model.AwardId);
                var currentAwardFrequency = _nominationService.GetAwardFrequencyById(awardOfCurrentNomination.FrequencyId);
                #region RestrictManagerLogic

                var noOfNominationForManager = Convert.ToInt32(ConfigurationManager.AppSettings["noOfNominationForManager"]);
                var startDate = new DateTime();
                var endDate = new DateTime();
                var countOfNomination = 0;
                if (currentAwardFrequency.Code == FrequencyCode.MON.ToString())
                {
                    var firstDateOfCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    startDate = firstDateOfCurrentMonth.AddMonths(-1);
                    endDate = firstDateOfCurrentMonth.AddDays(-1);
                    countOfNomination = _nominationService.GetNominationCountByManagerIdForSom(model.ManagerId, startDate, endDate, model.AwardId);
                }
                else if (currentAwardFrequency.Code == FrequencyCode.YEAR.ToString())
                {
                    var firstDateOfCurrentYear = new DateTime(DateTime.Today.Year, 1, 1);
                    startDate = firstDateOfCurrentYear.AddYears(-1);
                    countOfNomination = _nominationService.GetNominationCountByManagerIdForPinnacle(model.ManagerId, startDate, model.AwardId);
                }

                if (noOfNominationForManager <= countOfNomination)
                {
                    _logger.Log("Nomination-AddNomination-POST-try-if noOfNominationForManager <= countOfNomination");
                    return Json(new { success = true, nominationExceed = true, message = "Only " + noOfNominationForManager + " nominations are allowed for " + awardOfCurrentNomination.Name + " !" }, JsonRequestBehavior.AllowGet);
                }

                #endregion RestrictManagerLogic

                var nomination = new Nomination
                {
                    AwardId = model.AwardId,
                    ManagerId = model.ManagerId,
                    UserId = model.ResourceId
                };

                switch (model.SelectResourcesBy)
                {
                    case "Project":
                        nomination.ProjectID = model.ProjectID;
                        break;
                    case "Department":
                        nomination.DepartmentId = model.DepartmentId;
                        break;
                    case "Other":
                        nomination.Other = true;
                        nomination.OtherNominationReason = model.OtherNominationReason;
                        break;
                    default:
                        break;
                }

                if (currentAwardFrequency.Code == FrequencyCode.YEAR.ToString())
                {
                    nomination.NominationDate = DateTime.Now.Date.AddYears(-1);
                }
                else
                {
                    nomination.NominationDate = DateTime.Now.Date.AddMonths(-1);

                }

                nomination.IsSubmitted = submit.Equals("Submit");

                foreach (var criteria in model.Comments)
                {
                    if (criteria.Comment != null || criteria.Rating != 0)
                    {
                        nomination.ManagerComments.Add(
                            new ManagerComment
                            {
                                CriteriaId = criteria.Id,
                                Comment = criteria.Comment != null ? _textInfo.ToTitleCase(criteria.Comment) : "",
                                Rating = criteria.Rating,
                                Weightage = criteria.Weightage,
                                FinalScore = 0,
                                AdminComment = ""
                            }
                        );
                    }
                }
                nomination.Comment = model.MainComment != null ? _textInfo.ToTitleCase(model.MainComment) : "";

                nomination.IsLocked = false;
                _awardService.AddNomination(nomination);
            }
            catch (Exception ex)
            {
                _logger.Log("Nomination-AddNomination-POST-catch");
                _logger.Log("Nomination-AddNomination-POST-" + ex.Message);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CriteriasForAward(int awardId)
        {
            _logger.Log("Nomination-CriteriasForAward-POST");
            var criteriaList = _awardService.GetCriteriasForAward(awardId);
            return Json(criteriaList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CriteriasForAwardPartialView(int awardId)
        {
            _logger.Log("Nomination-CriteriasForAward-POST");
            var criteriaList = _awardService.GetCriteriasForAward(awardId);
            return PartialView("~/Views/Nomination/Shared/_criteriaForAwards.cshtml", criteriaList);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult DiscardNomination(int nominationId)
        {
            _logger.Log("Nomination-DiscardNomination-POST");
            _nominationService.DiscardNomination(nominationId);
            return RedirectToAction("GetNominationList");
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult EditSavedNomination(int nominationId)
        {
            _logger.Log("Nomination-EditSavedNomination-GET");
            var savedNomination = _nominationService.GetNomination(nominationId);
            var nominationViewModel = new NominationViewModel();
            var userEmailAddress = User.Identity.Name;

            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress);
            var departments = _awardService.GetDepartmentsUnderCurrentUserAsManager(userEmailAddress);
            var nominatedUser = _awardService.GetUserById(savedNomination.UserId);

            var currentUserId = _awardService.GetUserIdFromEmail(userEmailAddress);
            nominationViewModel.ManagerId = currentUserId;

            if (savedNomination.ProjectID != null)
            {
                nominationViewModel.SelectResourcesBy = "Project";
                var firstOrDefault = projects.FirstOrDefault(p => p.ID == savedNomination.ProjectID);
                if (firstOrDefault != null) nominationViewModel.ProjectOrDeptName = projects.Any() ? firstOrDefault.Name : "";
            }
            else if (savedNomination.DepartmentId != null)
            {
                nominationViewModel.SelectResourcesBy = "Department";
                var firstOrDefault = departments.FirstOrDefault(d => d.ID == savedNomination.DepartmentId);
                if (firstOrDefault != null) nominationViewModel.ProjectOrDeptName = departments.Any() ? firstOrDefault.Name : "";
            }
            else
            {
                nominationViewModel.SelectResourcesBy = "Other";
            }

            nominationViewModel.ResourceName = nominatedUser != null ? nominatedUser.DisplayName : "";

            //IN FUTURE GOING TO USE MAPPER
            nominationViewModel.AwardId = savedNomination.AwardId;
            nominationViewModel.NominationId = savedNomination.Id;
            nominationViewModel.ManagerId = savedNomination.ManagerId;
            nominationViewModel.ProjectID = savedNomination.ProjectID;
            nominationViewModel.DepartmentId = savedNomination.DepartmentId;
            nominationViewModel.ResourceId = savedNomination.UserId;
            nominationViewModel.IsSubmitted = savedNomination.IsSubmitted;
            nominationViewModel.IsOther = savedNomination.Other;
            nominationViewModel.MainComment = savedNomination.Comment;
            nominationViewModel.OtherNominationReason = savedNomination.OtherNominationReason;
            nominationViewModel.AwardName = _awardService.GetAwardNameById(savedNomination.AwardId);

            var criterias = _awardService.GetCriteriasForAward(nominationViewModel.AwardId);

            int index = 1;
            foreach (var criteria in criterias)
            {
                var managerComment = savedNomination.ManagerComments.FirstOrDefault(c => c.CriteriaId == criteria.Id);

                nominationViewModel.Comments.Add(new CriteriaCommentViewModel
                {
                    Id = criteria.Id,
                    Title = criteria.Title,
                    Comment = managerComment != null ? managerComment.Comment : string.Empty,
                    Rating = managerComment != null ? managerComment.Rating : 0,
                    Weightage = managerComment != null ? managerComment.Weightage : 0,
                    IndexId = index
                });
                index++;
            }
            return View("EditNomination", nominationViewModel);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public void EditSavedNomination(NominationViewModel model, string submit)
        {
            _logger.Log("Nomination-EditSavedNomination-POST");
            Nomination nomination = new Nomination
            {
                Id = model.NominationId,
                AwardId = model.AwardId,
                DepartmentId = model.DepartmentId,
                ProjectID = model.ProjectID,
                IsSubmitted = submit.Equals("Submit"),
                ManagerId = model.ManagerId,
                UserId = model.ResourceId,
                Other = model.IsOther,
                OtherNominationReason = model.OtherNominationReason
            };

            var awardOfCurrentNomination = _awardService.GetAwardById(model.AwardId);
            var currentAwardFrequency = _nominationService.GetAwardFrequencyById(awardOfCurrentNomination.FrequencyId);

            if (currentAwardFrequency.Code == FrequencyCode.YEAR.ToString())
            {
                nomination.NominationDate = DateTime.Now.Date.AddYears(-1);
            }
            else
            {
                nomination.NominationDate = DateTime.Now.Date.AddMonths(-1);
            }

            foreach (var comment in model.Comments)
            {
                if (comment.Comment != null || comment.Rating != 0)
                {
                    nomination.ManagerComments.Add(new ManagerComment
                    {
                        CriteriaId = comment.Id,
                        Comment = comment.Comment != null ? _textInfo.ToTitleCase(comment.Comment) : "",
                        NominationId = model.NominationId,
                        Rating = comment.Rating,
                        Weightage = comment.Weightage,
                        FinalScore = 0,
                        AdminComment = ""
                    });
                }
            }

            nomination.Comment = model.MainComment != null ? _textInfo.ToTitleCase(model.MainComment) : "";

            _nominationService.DeletePrevoiusManagerComments(model.NominationId);
            _nominationService.UpdateNomination(nomination);
        }

        [HttpPost]
        public JsonResult ResourcesInProject(int engagementId, int awardId)
        {
            _logger.Log("Nomination-ResourcesInProject-POST");

            var name = User.Identity.Name;
            _awardService.GetProjectsUnderCurrentUserAsManager(name);
            var managerId = _awardService.GetUserIdFromEmail(name);
            var usersInEngagement = _awardService.GetResourcesInEngagement(engagementId, managerId, awardId);
            return Json(usersInEngagement, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ResourcesInDepartment(int departmentId, int awardId)
        {
            _logger.Log("Nomination-ResourcesInDepartment-GET");
            var email = User.Identity.Name;
            var userIdToExcept = _awardService.GetUserIdFromEmail(email);
            var usersInDepartment = _awardService.GetResourcesUnderDepartment(departmentId, userIdToExcept);
            return Json(usersInDepartment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllResources(bool getAllResources)
        {
            _logger.Log("Nomination-GetAllResources-POST");
            var allResources = new List<User>();
            if (getAllResources)
            {
                allResources = _nominationService.GetAllResources();
            }
            return Json(allResources, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllResourcesForOtherReason(int awardId)
        {
            _logger.Log("Nomination-GetAllResources-POST");
            int managerId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            var filteredResources = _nominationService.GetAllResourcesForOtherReason(awardId, managerId);
            return Json(filteredResources, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult SavedNomination(int managerId = 0)
        {
            _logger.Log("Nomination-SavedNomination-GET");
            var email = User.Identity.Name;
            _awardService.GetProjectsUnderCurrentUserAsManager(email);
            _awardService.GetDepartmentsUnderCurrentUserAsManager(email);
            
            var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUser(managerId);
            var savedNominations = new List<NominationListViewModel>();

            foreach (var nomination in nominations)
            {
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                if (award != null)
                {
                    var awardFrequencyCode = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                    var awardName = award.Code;
                    var nomineeName = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);
                    var nominationTime = nomination.NominationDate;
                    string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year;
                    if (nomineeName != null)
                    {
                        var reviewNominationViewModel = new NominationListViewModel()
                        {
                            Intials = nomineeName.FirstName.Substring(0, 1) + "" + nomineeName.LastName.Substring(0, 1),
                            AwardName = awardName,
                            DisplayName = nomineeName.DisplayName,
                            NominationTime = nominationTimeToDisplay,
                            Id = nomination.Id,
                            IsSubmitted = nomination.IsSubmitted,
                            AwardFrequencyCode = awardFrequencyCode.Code
                        };
                        savedNominations.Add(reviewNominationViewModel);
                    }
                }
            }
            return View(savedNominations);
        }

        #region Overloaded for list of nominations under edit

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult GetNominationList()
        {
            _logger.Log("Nomination-GetNominationList-GET");
            var savedNominations = GetNominations(true);
            return View("SavedNomination", savedNominations);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult GetNominationListPartialView(bool forCurrentMonth)
        {
            _logger.Log("Nomination-GetNominationListPartialView-GET");
            var savedNominations = GetNominations(forCurrentMonth);
            return PartialView("~/Views/Nomination/Shared/_savedNominationList.cshtml", savedNominations);
        }

        private List<NominationListViewModel> GetNominations(bool forCurrentMonth)
        {
            _logger.Log("Nomination-GetNominations-GET");
            var email = User.Identity.Name;
            _awardService.GetProjectsUnderCurrentUserAsManager(email);

            var managerId = _awardService.GetUserIdFromEmail(email);
            
            var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUserAndMonth(managerId, forCurrentMonth);

            var savedNominations = new List<NominationListViewModel>();

            foreach (var nomination in nominations)
            {
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                var awardName = award.Code;
                var nominee = _nominationService.GetNomineeDetails(nomination.UserId);
                var nominationTime = nomination.NominationDate;
                var awardFrequency = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year;

                var reviewNominationViewModel = new NominationListViewModel()
                {
                    Intials = nominee.FirstName.Substring(0, 1) + "" + nominee.LastName.Substring(0, 1),
                    AwardName = awardName,
                    DisplayName = nominee.DisplayName,
                    NominationTime = nominationTimeToDisplay,
                    Id = nomination.Id,
                    IsSubmitted = nomination.IsSubmitted,
                    AwardFrequencyCode = awardFrequency.Code
                };
                savedNominations.Add(reviewNominationViewModel);
            }
            return savedNominations;
        }

        #endregion Overloaded for list of nominations under edit

        #region ReviewNomination

        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult EditReview(int nominationId, string details)
        {
            _logger.Log("Nomination-EditReview");
            int totalCredit = 0;
            var result = _nominationService.GetReviewNomination(nominationId);
            var userEmailAddress = User.Identity.Name;
            var reviewerId = _nominationService.GetReviewerIdOfCurrentNomination(userEmailAddress);
            var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(rc => rc.NominationId == nominationId && rc.ReviewerId == reviewerId);

            var reviewNominationViewModel = new ReviewSubmitionViewModel()
            {
                ManagerComments = _nominationService.GetManagerCommentsForNomination(nominationId),
                Manager = _nominationService.GetManagerNameOfCurrentNomination(nominationId),
                NomineeName = _nominationService.GetNomineeNameOfCurrentNomination(nominationId),
                ProjectOrDepartment = _nominationService.GetProjectNameOfCurrentNomination(nominationId),
                Criterias = _nominationService.GetCriteriaForNomination(nominationId),
                ReviewerId = reviewerId,
                NominationId = result.Id,
                ManagerComment = result.Comment
            };
            foreach (var item in reviewerComments)
            {
                reviewNominationViewModel.Comments.Add(
                    new ReviewerCommentViewModel()
                    {
                        CriteriaId = item.CriteriaId,
                        Comment = item.Comment,
                        Credit = Convert.ToInt32(item.Credit),
                        Id = item.Id,
                    });
                if (item.Credit == 1)
                {
                    totalCredit = totalCredit + Convert.ToInt32(item.Credit);
                }
            }

            reviewNominationViewModel.TotalCredit = totalCredit;

            if (details == "Details")
            {
                return View("ReviewDetails", reviewNominationViewModel);
            }

            return View(reviewNominationViewModel);
        }

        public ActionResult ReviewDetails(int nominationId)
        {
            _logger.Log("Nomination-ReviewDetails");

            var allSubmittedReviewForCurrentNomination = _nominationService.GetAllSubmitedReviewsForCurrentNomination(nominationId);
            var result = _nominationService.GetReviewNomination(nominationId);
            var userEmailAddress = User.Identity.Name;
            var reviewerId = _nominationService.GetReviewerIdOfCurrentNomination(userEmailAddress);
            var listOfAllSubmittedRevierComments = new List<List<ReviewerComment>>();

            foreach (var submittedreview in allSubmittedReviewForCurrentNomination)
            {
                var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(rc => rc.NominationId == nominationId && rc.ReviewerId == submittedreview.ReviewerId).ToList();
                listOfAllSubmittedRevierComments.Add(reviewerComments);
            }

            var listOfReviewSubmitionViewModel = new List<ReviewSubmitionViewModel>();

            foreach (var item in listOfAllSubmittedRevierComments)
            {
                var reviewNominationViewModel = new ReviewSubmitionViewModel()
                {
                    ManagerComments = _nominationService.GetManagerCommentsForNomination(nominationId),
                    Manager = _nominationService.GetManagerNameOfCurrentNomination(nominationId),
                    NomineeName = _nominationService.GetNomineeNameOfCurrentNomination(nominationId),
                    ProjectOrDepartment = _nominationService.GetProjectNameOfCurrentNomination(nominationId),
                    Criterias = _nominationService.GetCriteriaForNomination(nominationId),
                    ReviewerId = reviewerId,
                    NominationId = result.Id,
                    ManagerComment = result.Comment
                };
                foreach (var d in item)
                {
                    reviewNominationViewModel.Comments.Add(
                        new ReviewerCommentViewModel
                        {
                            CriteriaId = d.CriteriaId,
                            Comment = d.Comment,
                            Credit = Convert.ToInt32(d.Credit),
                            Id = d.Id,
                        });
                    if (d.Credit == 1)
                    {
                        reviewNominationViewModel.TotalCredit = reviewNominationViewModel.TotalCredit + Convert.ToInt32(d.Credit);
                    }
                }
                listOfReviewSubmitionViewModel.Add(reviewNominationViewModel);
            }

            return View("ReviewDetails", listOfReviewSubmitionViewModel);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult EditReview(ReviewSubmitionViewModel model, string Submit)
        {
            _logger.Log("Nomination-EditReview-POST");
            var alreadyReviewed = _encourageDatabaseContext.Query<Review>().Where(r => r.ReviewerId == model.ReviewerId && r.NominationId == model.NominationId).FirstOrDefault();
            var previousComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(r => r.ReviewerId == model.ReviewerId && r.NominationId == model.NominationId).ToList();
            foreach (var previousComment in previousComments)
            {
                _encourageDatabaseContext.Delete<ReviewerComment>(previousComment);
            }
            if (alreadyReviewed != null)
            {
                _encourageDatabaseContext.Delete<Review>(alreadyReviewed);
                _encourageDatabaseContext.SaveChanges();
            }

            var review = new Review();
            review.NominationId = model.NominationId;
            review.ReviewerId = model.ReviewerId;
            review.ReviewDate = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(Submit) && Submit == "Submit")
            {
                review.IsSubmited = true;
            }

            if (!string.IsNullOrEmpty(Submit) && Submit == "Save Draft")
            {
                review.IsSubmited = false;
            }

            if (!string.IsNullOrEmpty(Submit) && Submit != "Discard Review")
            {
                _nominationService.AddReviewForCurrentNomination(review);

                foreach (var item in model.Comments)
                {
                    var revrComment = new ReviewerComment()
                    {
                        NominationId = model.NominationId,
                        ReviewerId = model.ReviewerId,
                        CriteriaId = item.CriteriaId,
                        Comment = item.Comment != null ? _textInfo.ToTitleCase(item.Comment) : "",
                        Credit = Convert.ToInt32(item.Credit),
                        ReviewId = review.Id
                    };
                    _nominationService.AddReviewerCommentsForCurrentNomination(revrComment);
                }
                if (review.IsSubmited == true)
                {
                    _nominationService.UpdateFinalScore(model.NominationId);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNominations()
        {
            _logger.Log("Nomination-ReviewNominations-GET");
            var reviewerId = _nominationService.GetReviewerIdOfCurrentNomination(User.Identity.Name);
            var reviewNominations = new List<NominationListViewModel>();
            if (reviewerId == 0)
            {
                ViewBag.erroMessage = "You are not authorized to view this page, please contact system administrator.";
                return View(reviewNominations);
            }

            var nominations = _nominationService.GetAllSubmitedNonreviewedNominations(reviewerId);

            foreach (var nomination in nominations)
            {
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                var awardFrequency = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                var orDefault = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                if (orDefault != null)
                {
                    var awardName = orDefault.Code;
                    var nomineeName = _commonDbContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);
                    var nominationDb = _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(n => n.Id == nomination.Id);
                    if (nominationDb != null)
                    {
                        var nominationTime = nominationDb.NominationDate;
                        bool islocked = false;

                        try
                        {
                            var reviewData = from r in _reviewService.GetAllReview()
                                join n in _nominationService.GetAllNominations()
                                on r.NominationId equals n.Id
                                where (n.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)
                                       &&
                                       (DateTime.Now.Month > 1 ? (DateTime.Now.Year).Equals(n.NominationDate.Value.Year) : (DateTime.Now.Year - 1).Equals(n.NominationDate.Value.Year))
                                )
                                select r;

                            var firstOrDefault = reviewData.FirstOrDefault();
                            if (firstOrDefault != null) islocked = firstOrDefault.IsLocked ?? false;
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();

                        var reviewNominationViewModel = new NominationListViewModel()
                        {
                            Intials = nomineeName.FirstName.Substring(0, 1) + "" + nomineeName.LastName.Substring(0, 1),
                            AwardName = awardName,
                            DisplayName = nomineeName.DisplayName,
                            NominationTime = nominationTimeToDisplay,
                            Id = nomination.Id,
                            IsLocked = islocked,
                            IsDrafted = _nominationService.CheckReviewIsDrafted(nomination.Id),
                            AwardFrequencyCode = awardFrequency.Code
                        };
                        reviewNominations.Add(reviewNominationViewModel);
                    }
                }
            }
            return View(reviewNominations);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNomination(int nominationId)
        {
            _logger.Log("Nomination-ReviewNomination-GET");
            var result = _nominationService.GetReviewNomination(nominationId);
            var userEmailAddress = User.Identity.Name;
            var projectOrDept = string.Empty;
            if (result.ProjectID != null)
                projectOrDept = _nominationService.GetProjectNameOfCurrentNomination(nominationId);
            else if (result.DepartmentId != null)
                projectOrDept = _nominationService.GetDeptNameOfCurrentNomination(nominationId);

            var lockedAwards = _nominationService.GetNominationLockStatus();
            var isLocked = false;
            var awardOfCurrentNomination = _awardService.GetAwardFromNominationId(nominationId);
            foreach (var lockedAward in lockedAwards)
            {
                if (lockedAward.Id == awardOfCurrentNomination.Id)
                {
                    isLocked = true;
                }
            }

            var reviewNominationViewModel = new ReviewSubmitionViewModel()
            {
                ManagerComments = _nominationService.GetManagerCommentsForNomination(nominationId),
                Manager = _nominationService.GetManagerNameOfCurrentNomination(nominationId),
                NomineeName = _nominationService.GetNomineeNameOfCurrentNomination(nominationId),
                ProjectOrDepartment = projectOrDept,
                Criterias = _nominationService.GetCriteriaForNomination(nominationId),
                ReviewerId = _nominationService.GetReviewerIdOfCurrentNomination(userEmailAddress),
                NominationId = result.Id,
                ManagerComment = result.Comment,
                IsLocked = isLocked
            };

            return View(reviewNominationViewModel);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNomination(ReviewSubmitionViewModel model, string Submit)
        {
            _logger.Log("Nomination-ReviewNomination-POST");
            var alreadyReviewed = _encourageDatabaseContext.Query<Review>().Where(r => r.ReviewerId == model.ReviewerId && r.NominationId == model.NominationId).FirstOrDefault();
            var previousComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(r => r.ReviewerId == model.ReviewerId && r.NominationId == model.NominationId).ToList();

            foreach (var previousComment in previousComments)
            {
                _encourageDatabaseContext.Delete<ReviewerComment>(previousComment);
            }

            if (alreadyReviewed != null)
            {
                _encourageDatabaseContext.Delete<Review>(alreadyReviewed);
                _encourageDatabaseContext.SaveChanges();
            }

            var review = new Review();
            review.NominationId = model.NominationId;
            review.ReviewerId = model.ReviewerId;
            review.ReviewDate = DateTime.UtcNow;
            review.IsLocked = false;

            if (!string.IsNullOrEmpty(Submit) && Submit == "Submit")
            {
                review.IsSubmited = true;
            }

            _nominationService.AddReviewForCurrentNomination(review);

            foreach (var item in model.Comments)
            {
                var revrComment = new ReviewerComment()
                {
                    NominationId = model.NominationId,
                    ReviewerId = model.ReviewerId,
                    CriteriaId = item.CriteriaId,
                    Comment = item.Comment != null ? _textInfo.ToTitleCase(item.Comment) : "",
                    Credit = item.Credit,
                    ReviewId = review.Id
                };
                _nominationService.AddReviewerCommentsForCurrentNomination(revrComment);
            }

            if (review.IsSubmited == true)
            {
                _nominationService.UpdateFinalScore(model.NominationId);
            }
            return RedirectToAction("Index", "Home");
        }

        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult SavedReviews()
        {
            _logger.Log("Nomination-SavedReviews");
            var reviewedNominations = GetSavedReviewsList(true);

            return View(reviewedNominations);
        }

        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult GetSavedReviewsPartialView(bool forCurrentMonth)
        {
            _logger.Log("Nomination-GetSavedReviewsPartialView");
            var reviewedNominations = GetSavedReviewsList(forCurrentMonth);

            return PartialView("~/Views/Nomination/Shared/_savedReviewsList.cshtml", reviewedNominations);
        }

        private List<NominationListViewModel> GetSavedReviewsList(bool forCurrentMonth)
        {
            _logger.Log("Nomination-GetSavedReviewsList");
            var reviewedNominations = new List<NominationListViewModel>();
            var reviewerId = _nominationService.GetReviewerIdOfCurrentNomination(User.Identity.Name);

            if (reviewerId == 0)
            {
                ViewBag.erroMessage = "You are not authorized to view this page, please contact system administrator.";
                return reviewedNominations;
            }

            var nominations = _nominationService.GetAllSubmitedReviewedNominations(reviewerId, forCurrentMonth);
            foreach (var nomination in nominations)
            {
                var awardName = _nominationService.GetAwardNameByAwardId(nomination.AwardId);
                var nominee = _nominationService.GetNomineeDetails(nomination.UserId);
                var nominationTime = nomination.NominationDate;
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                var awardFrequency = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                var reviewNominationViewModel = new NominationListViewModel()
                {
                    Intials = nominee.FirstName.Substring(0, 1) + "" + nominee.LastName.Substring(0, 1),
                    AwardName = awardName,
                    DisplayName = nominee.DisplayName,
                    NominationTime = nominationTimeToDisplay,
                    Id = nomination.Id,
                    IsSubmitted = nomination.IsSubmitted,
                    AwardFrequencyCode = awardFrequency.Code
                };
                reviewedNominations.Add(reviewNominationViewModel);
            }
            return reviewedNominations;
        }
        #endregion ReviewNomination
    }
}

#endregion Nomination