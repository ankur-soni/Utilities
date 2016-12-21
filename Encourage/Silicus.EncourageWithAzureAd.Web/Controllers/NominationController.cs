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

        #region Test

        //public ActionResult setLockForNomination()
        //{
        //    DateTime currentDate = System.DateTime.Now;
        //    var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
        //    var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

        //    if (currentDate == expireDateManager || currentDate > expireDateManager)
        //    {
        //        var allNominations = _nominationService.GetAllNominations();

        //        //  var nominations = allNominations.Where(x => (x.NominationDate != null) && (Object.Equals((x.NominationDate.Value.Month),(DateTime.Now.Month - 1)))).ToList();

        //        //var nominations = allNominations.Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)).ToList();
        //        var nominations = allNominations.Where(x => (x.NominationDate.Value.Month.Equals(currentDate.Month - 1) && x.NominationDate.Value.Year.Equals(currentDate.Month>1 ? currentDate.Year : currentDate.Year-1))).ToList();

        //        foreach (var nomination in nominations)
        //        {
        //            if (nomination != null)
        //            {
        //                nomination.IsLocked = true;
        //                _nominationService.UpdateNomination(nomination);
        //            }
        //        }
        //    }
        //    else
        //    {
        //    }
        //    return null;

        //}

        #endregion Test

        #region Nomination
        [HttpGet]
        public ActionResult GetLockedAwardCategories()
        {
         return   Json( _nominationService.GetNominationLockStatus(),JsonRequestBehavior.AllowGet);
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

            //bool isLocked = _nominationService.IsNominationLocked();
            
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
                    countOfNomination = _nominationService.GetNominationCountByManagerIdForSOM(model.ManagerId, startDate, endDate, model.AwardId);
                }
                else if (currentAwardFrequency.Code == FrequencyCode.YEAR.ToString())
                {
                    var firstDateOfCurrentYear = new DateTime(DateTime.Today.Year, 1 , 1);
                    startDate = firstDateOfCurrentYear;
                    countOfNomination = _nominationService.GetNominationCountByManagerIdForPINNACLE(model.ManagerId, startDate, model.AwardId);
                }

                if (noOfNominationForManager <= countOfNomination)
                {
                    // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.Log("Nomination-AddNomination-POST-try-if noOfNominationForManager <= countOfNomination");
                    return Json(new { success = true, nominationExceed = true, message = "Only " + noOfNominationForManager + " nominations are allowed for "+ awardOfCurrentNomination.Name + " !" }, JsonRequestBehavior.AllowGet);
                    //      return Json("Only " + noOfNominationForManager.ToString() + " nominations are allowed per month!", MediaTypeNames.Text.Plain);
                }

                #endregion RestrictManagerLogic

                var nomination = new Nomination
                {
                    AwardId = model.AwardId,
                    ManagerId = model.ManagerId,
                    UserId = model.ResourceId
                };

                if (model.SelectResourcesBy.Equals("Project"))
                    nomination.ProjectID = model.ProjectID;
                else if (model.SelectResourcesBy.Equals("Department"))
                    nomination.DepartmentId = model.DepartmentId;

                if (currentAwardFrequency.Code == FrequencyCode.YEAR.ToString())
                {
                    nomination.NominationDate = DateTime.Now;
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
            // return RedirectToAction("Dashboard", "Dashboard");
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
            return PartialView("~/Views/Nomination/Shared/_criteriaForAwards.cshtml",criteriaList);
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

            nominationViewModel.ListOfAwards = new SelectList(_awardService.GetAllAwards(), "Id", "Name");
            int currentUserId = 0;
            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress);
            
            if (projects.Count > 0)
            {
                nominationViewModel.ProjectsUnderCurrentUser
                                    = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress), "Id", "Name");
                currentUserId = _awardService.GetUserIdFromEmail(userEmailAddress);
                nominationViewModel.ManagerId = currentUserId;
            }

            nominationViewModel.DepartmentsUnderCurrentUser = new SelectList(_awardService.GetDepartmentsUnderCurrentUserAsManager(userEmailAddress), "Id", "Name");

            if (savedNomination.ProjectID != null)
            {
                nominationViewModel.SelectResourcesBy = "Project";
                nominationViewModel.Resources = new SelectList(_awardService.GetResourcesForEditInEngagement(savedNomination.ProjectID.Value, currentUserId), "Id", "DisplayName");
            }
            else if (savedNomination.DepartmentId != null)
            {
                nominationViewModel.SelectResourcesBy = "Department";
                nominationViewModel.Resources = new SelectList(_awardService.GetResourcesForEditInDepartment(savedNomination.DepartmentId.Value, currentUserId), "Id", "DisplayName");
            }

            //IN FUTURE GOING TO USE MAPPER
            nominationViewModel.AwardId = savedNomination.AwardId;
            nominationViewModel.NominationId = savedNomination.Id;
            nominationViewModel.ManagerId = savedNomination.ManagerId;
            nominationViewModel.ProjectID = savedNomination.ProjectID;
            nominationViewModel.DepartmentId = savedNomination.DepartmentId;
            nominationViewModel.ResourceId = savedNomination.UserId;
            nominationViewModel.IsSubmitted = savedNomination.IsSubmitted;
            nominationViewModel.MainComment = savedNomination.Comment;

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
                NominationDate = DateTime.Now.Date.AddMonths(-1)
            };

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

            // return RedirectToAction("SavedNomination");
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
            //var userIdToExcept = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string);
            var email = User.Identity.Name;
            var userIdToExcept = _awardService.GetUserIdFromEmail(email);
            var usersInDepartment = _awardService.GetResourcesUnderDepartment(departmentId, userIdToExcept);
            return Json(usersInDepartment, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult SavedNomination(int managerId = 0)
        {
            _logger.Log("Nomination-SavedNomination-GET");
            //  var projects = _awardService.GetProjectsUnderCurrentUserAsManager(Session["UserEmailAddress"] as string);
            var email = User.Identity.Name;
            _awardService.GetProjectsUnderCurrentUserAsManager(email);
            _awardService.GetDepartmentsUnderCurrentUserAsManager(email);

            #region New Changes

            //var managerId = 0;
            //if (projects.Count>0)
            //{
            //    // managerId = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string);
            //    managerId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            //}
            //else
            //{
            //    managerId = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
            //}

            //if (depts.Count > 0)
            //{
            //    // managerId = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string);
            //    managerId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            //}
            //else
            //{
            //    managerId = _awardService.GetUserIdFromEmail("tushar.surve@silicus.com");
            //}

            #endregion New Changes

            var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUser(managerId);
            var savedNominations = new List<NominationListViewModel>();

            foreach (var nomination in nominations)
            {
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                var awardFrequencyCode = _nominationService.GetAwardFrequencyById(award.FrequencyId);
                if (award != null)
                {
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
            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(email);

            var managerId = 0;
            if (projects.Count > 0)
            {
                managerId = _awardService.GetUserIdFromEmail(email);
            }
            else
            {
                managerId = _awardService.GetUserIdFromEmail(email);
            }
            var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUserAndMonth(managerId, forCurrentMonth);

            var savedNominations = new List<NominationListViewModel>();

            foreach (var nomination in nominations)
            {
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
                var awardName = award.Code;
                var nominee = _nominationService.GetNomineeDetails(nomination.UserId);
                var nominationTime = nomination.NominationDate; //_encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nomination.Id).FirstOrDefault().NominationDate;
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
            //var userEmailAddress = Session["UserEmailAddress"] as string;
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
            //var userEmailAddress = Session["UserEmailAddress"] as string;
            var userEmailAddress = User.Identity.Name;
            var reviewerId = _nominationService.GetReviewerIdOfCurrentNomination(userEmailAddress);
            // var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(rc => rc.NominationId == nominationId && rc.ReviewerId == reviewerId);

            var listOfAllSubmittedRevierComments = new List<List<ReviewerComment>>();

            foreach (var submittedreview in allSubmittedReviewForCurrentNomination)
            {
                // var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(rc => rc.NominationId == nominationId && rc.ReviewerId == submittedreview.ReviewerId).ToList();
                var reviewerComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(rc => rc.NominationId == nominationId && rc.ReviewerId == submittedreview.ReviewerId).ToList();
                listOfAllSubmittedRevierComments.Add(reviewerComments);
            }

            var listOfReviewSubmitionViewModel = new List<ReviewSubmitionViewModel>();

            foreach (var item in listOfAllSubmittedRevierComments)
            {
                var data = item.Where(i => i.ReviewerId == i.ReviewerId);
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
                //  int totalCredit = 0;
                foreach (var d in data)
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
                var awardName = _encourageDatabaseContext.Query<Award>().Where(a => a.Id == nomination.AwardId).FirstOrDefault().Code;
                var nomineeName = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nomination.Id).FirstOrDefault().NominationDate;
                var reviews = _reviewService.GetAllReview();
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

                    islocked = reviewData.ToList().FirstOrDefault().IsLocked ?? false;
                }
                catch (Exception) { }

                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();

                var reviewNominationViewModel = new NominationListViewModel()
                {
                    Intials = nomineeName.FirstName.Substring(0, 1) + "" + nomineeName.LastName.Substring(0, 1),
                    AwardName = awardName,
                    DisplayName = nomineeName.DisplayName,
                    NominationTime = nominationTimeToDisplay,
                    Id = nomination.Id,
                    IsLocked = islocked,
                    IsDrafted = _nominationService.checkReviewIsDrafted(nomination.Id),
                    AwardFrequencyCode = awardFrequency.Code
                };
                reviewNominations.Add(reviewNominationViewModel);
            }
            return View(reviewNominations);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNomination(int nominationId)
        {
            _logger.Log("Nomination-ReviewNomination-GET");
            var result = _nominationService.GetReviewNomination(nominationId);
            // var userEmailAddress = Session["UserEmailAddress"] as string;
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
                if (review.IsSubmited == true)
                {
                    _nominationService.UpdateFinalScore(model.NominationId);
                }
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