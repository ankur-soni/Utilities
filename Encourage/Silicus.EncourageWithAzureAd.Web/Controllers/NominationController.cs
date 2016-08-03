﻿using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using Silicus.EncourageWithAzureAd.Web.Models;
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

        public NominationController(INominationService nominationService, Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, IAwardService awardService, IReviewService reviewService)
        {
            _nominationService = nominationService;
            _commonDbContext = commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
            _reviewService = reviewService;
            _textInfo = new CultureInfo("en-US", false).TextInfo;
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



        #endregion

        #region Nomination

        // GET: Nomination/Create
        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult AddNomination()
        {
            var userEmailAddress = User.Identity.Name;
            ViewBag.Awards = new SelectList(_awardService.GetAllAwards(), "Id", "Name");

            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress);

            if (projects.Any())
            {
                ViewBag.ProjectsUnderCurrentUser = new SelectList(projects, "Id", "Name");
                ViewBag.ManagerId = _awardService.GetUserIdFromEmail(userEmailAddress);
                ViewBag.ManagerIdByProject = _awardService.GetUserIdFromEmail(userEmailAddress);
            }
            else
            {
                ViewBag.ProjectsUnderCurrentUser = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name");
                ViewBag.ManagerId = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
                ViewBag.ManagerIdByProject = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
            }

            
            var depts = _awardService.GetDepartmentsUnderCurrentUserAsManager(userEmailAddress);
            if (depts.Count > 0)
            {
                ViewBag.DepartmentsUnderCurrentUser = new SelectList(depts, "Id", "Name");
                //ViewBag.ManagerId = _awardService.GetUserIdFromEmail(userEmailAddress);
                ViewBag.ManagerIdByDepartment = _awardService.GetUserIdFromEmail(userEmailAddress);
            }
            else
            {
                ViewBag.DepartmentsUnderCurrentUser = new SelectList(_awardService.GetDepartmentsUnderCurrentUserAsManager("tushar.surve@silicus.com"), "Id", "Name");
                //ViewBag.ManagerId = _awardService.GetUserIdFromEmail("tushar.surve@silicus.com");
                ViewBag.ManagerIdByDepartment = _awardService.GetUserIdFromEmail("tushar.surve@silicus.com");
            }

            ViewBag.Resources = new SelectList(new List<User>(), "Id", "DisplayName");

            //bool isLocked = _nominationService.IsNominationLocked();
            //var model = new NominationViewModel() { IsLocked = isLocked };

            return View(new NominationViewModel());
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult AddNomination(NominationViewModel model, string submit)
        {
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

            nomination.NominationDate = DateTime.Now.Date.AddMonths(-1);
            
            nomination.IsSubmitted = submit.Equals("Submit");

            foreach (var criteria in model.Comments)
            {
                if (criteria.Comment != null)
                {
                    nomination.ManagerComments.Add(
                        new ManagerComment()
                        {
                            CriteriaId = criteria.Id,
                            Comment = criteria.Comment != null ? _textInfo.ToTitleCase(criteria.Comment) : ""
                        }
                        );
                }
            }
            nomination.Comment = model.MainComment != null ? _textInfo.ToTitleCase(model.MainComment) : "";

            nomination.IsLocked = false;
            _awardService.AddNomination(nomination);
            // return RedirectToAction("Dashboard", "Dashboard");
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CriteriasForAward(int awardId)
        {
            var criteriaList = _awardService.GetCriteriasForAward(awardId);
            return Json(criteriaList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult DiscardNomination(int nominationId)
        {
            _nominationService.DiscardNomination(nominationId);
            return RedirectToAction("GetNominationList");
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult EditSavedNomination(int nominationId)
        {
            var savedNomination = _nominationService.GetNomination(nominationId);
            var nominationViewModel = new NominationViewModel();

            // var userEmailAddress = Session["UserEmailAddress"] as string;
            var userEmailAddress = User.Identity.Name;
            ViewBag.Awards = new SelectList(_awardService.GetAllAwards(), "Id", "Name");
            int currentUserId;
            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress);
            if (projects.Count > 0)
            {
                ViewBag.ProjectsUnderCurrentUser
                                    = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress), "Id", "Name");
                currentUserId = _awardService.GetUserIdFromEmail(userEmailAddress);
                ViewBag.ManagerId = currentUserId;
            }
            else
            {
                ViewBag.ProjectsUnderCurrentUser
                    = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name");
                currentUserId = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
                ViewBag.ManagerId = currentUserId;
            }

            ViewBag.DepartmentsUnderCurrentUser = new SelectList(_awardService.GetDepartmentsUnderCurrentUserAsManager("tushar.surve@silicus.com"), "Id", "Name");

            if (savedNomination.ProjectID != null)
            {
                nominationViewModel.SelectResourcesBy = "Project";
                ViewBag.Resources = new SelectList(_awardService.GetResourcesForEditInEngagement(savedNomination.ProjectID.Value, currentUserId), "Id", "DisplayName");
            }
            else if (savedNomination.DepartmentId != null)
            {
                nominationViewModel.SelectResourcesBy = "Department";
                ViewBag.Resources = new SelectList(_awardService.GetResourcesForEditInDepartment(savedNomination.DepartmentId.Value, _awardService.GetUserIdFromEmail("tushar.surve@silicus.com")), "Id", "DisplayName");
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

            foreach (var criteria in criterias)
            {
                string addedComment = string.Empty;

                foreach (var comment in savedNomination.ManagerComments)
                {
                    if (criteria.Id == comment.CriteriaId)
                    {
                        addedComment = comment.Comment;
                    }
                }
                nominationViewModel.Comments.Add(new CriteriaCommentViewModel()
                {
                    Id = criteria.Id,
                    title = criteria.Title,
                    Comment = addedComment
                });
            }
            return View("EditNomination", nominationViewModel);
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public void EditSavedNomination(NominationViewModel model, string submit)
        {
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
                if (comment.Comment != null)
                {
                    nomination.ManagerComments.Add(new ManagerComment()
                    {
                        CriteriaId = comment.Id,
                        Comment = comment.Comment != null ? _textInfo.ToTitleCase(comment.Comment) : "",
                        NominationId = model.NominationId

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
            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(User.Identity.Name);
            var managerId = _awardService.GetUserIdFromEmail(projects.Count > 0 ? User.Identity.Name : "shailendra.birthare@silicus.com");


            var usersInEngagement = _awardService.GetResourcesInEngagement(engagementId, managerId, awardId);
            return Json(usersInEngagement, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ResourcesInDepartment(int departmentId, int awardId)
        {
            //var userIdToExcept = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string);

            var userIdToExcept = _awardService.GetUserIdFromEmail("tushar.surve@silicus.com");

            var usersInDepartment = _awardService.GetResourcesUnderDepartment(departmentId, userIdToExcept);
            return Json(usersInDepartment, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult SavedNomination(int managerId = 0)
        {
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
            #endregion

            var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUser(managerId);
            var savedNominations = new List<NominationListViewModel>();

            foreach (var nomination in nominations)
            {
                var awardName = _encourageDatabaseContext.Query<Award>().Where(a => a.Id == nomination.AwardId).FirstOrDefault().Code;
                var nomineeName = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nomination.Id).FirstOrDefault().NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                var reviewNominationViewModel = new NominationListViewModel()
                {
                    Intials = nomineeName.FirstName.Substring(0, 1) + "" + nomineeName.LastName.Substring(0, 1),
                    AwardName = awardName,
                    DisplayName = nomineeName.DisplayName,
                    NominationTime = nominationTimeToDisplay,
                    Id = nomination.Id,
                    IsSubmitted = nomination.IsSubmitted
                };
                savedNominations.Add(reviewNominationViewModel);
            }
            return View(savedNominations);
        }

        #region Ovrloaded for list of nominations under edit

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Manager")]
        public ActionResult GetNominationList()
        {
            var email = User.Identity.Name;
            var projects = _awardService.GetProjectsUnderCurrentUserAsManager(email);
            var depts = _awardService.GetDepartmentsUnderCurrentUserAsManager(email);

            var managerId = 0;
            if (projects.Count > 0)
            {
                // managerId = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string); 
                managerId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            }
            else
            {
                managerId = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
            }
            var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUser(managerId);
            //if (depts.Count > 0)
            //{
            //    // managerid = _awardservice.getuseridfromemail(session["useremailaddress"] as string); 
            //    managerId = _awardService.GetUserIdFromEmail(User.Identity.Name);
            //}
            //else
            //{
            //    managerId = _awardService.GetUserIdFromEmail("tushar.surve@silicus.com");
            //}
            //#endregion

            ////var nominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUser(managerId);
            //var departmentNominations = _nominationService.GetAllSubmittedAndSavedNominationsByCurrentUser(managerId);
            //if (departmentNominations != null)
            //    nominations.AddRange(departmentNominations);

            var savedNominations = new List<NominationListViewModel>();

            foreach (var nomination in nominations)
            {
                var awardName = _encourageDatabaseContext.Query<Award>().Where(a => a.Id == nomination.AwardId).FirstOrDefault().Code;
                var nomineeName = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nomination.Id).FirstOrDefault().NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                var reviewNominationViewModel = new NominationListViewModel()
                {
                    Intials = nomineeName.FirstName.Substring(0, 1) + "" + nomineeName.LastName.Substring(0, 1),
                    AwardName = awardName,
                    DisplayName = nomineeName.DisplayName,
                    NominationTime = nominationTimeToDisplay,
                    Id = nomination.Id,
                    IsSubmitted = nomination.IsSubmitted
                };
                savedNominations.Add(reviewNominationViewModel);
            }
            return View("SavedNomination", savedNominations);
        }
        #endregion

        #region ReviewNomination

        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult EditReview(int nominationId, string details)
        {
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
                        Comment = item.Comment,
                        Credit = Convert.ToBoolean(item.Credit),
                        Id = item.Id,
                    });
                if (item.Credit == 1)
                {
                    totalCredit = totalCredit + Convert.ToInt32(item.Credit);
                }
            }

            ViewBag.creditGiven = totalCredit;

            if (details == "Details")
            {
                return View("ReviewDetails", reviewNominationViewModel);
            }

            return View(reviewNominationViewModel);

        }


        public ActionResult ReviewDetails(int nominationId)
        {

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
                        new ReviewerCommentViewModel()
                        {
                            Comment = d.Comment,
                            Credit = Convert.ToBoolean(d.Credit),
                            Id = d.Id,

                        });
                    if (d.Credit == 1)
                    {

                        //  totalCredit = totalCredit + Convert.ToInt32(d.Credit);
                        // ViewBag.creditGiven = totalCredit;
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
                        CriteriaId = item.Id,
                        Comment = item.Comment != null ? _textInfo.ToTitleCase(item.Comment) : "",
                        Credit = Convert.ToInt32(item.Credit),
                        ReviewId = review.Id

                    };
                    _nominationService.AddReviewerCommentsForCurrentNomination(revrComment);
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNominations()
        {
            // var nominations = _nominationService.GetAllSubmitedNonreviewedNominations(_nominationService.GetReviewerIdOfCurrentNomination( Session["UserEmailAddress"] as string));
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

                    islocked = reviewData.ToList().FirstOrDefault()?.IsLocked ?? false;

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
                    IsDrafted = _nominationService.checkReviewIsDrafted(nomination.Id)
                };
                reviewNominations.Add(reviewNominationViewModel);
            }
            return View(reviewNominations);
        }

        [HttpGet]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNomination(int nominationId)
        {

            var result = _nominationService.GetReviewNomination(nominationId);
            // var userEmailAddress = Session["UserEmailAddress"] as string;
            var userEmailAddress = User.Identity.Name;

            var projectOrDept = string.Empty;
            if (result.ProjectID != null)
                projectOrDept = _nominationService.GetProjectNameOfCurrentNomination(nominationId);
            else if (result.DepartmentId != null)
                projectOrDept = _nominationService.GetDeptNameOfCurrentNomination(nominationId);



            var reviewNominationViewModel = new ReviewSubmitionViewModel()
            {
                ManagerComments = _nominationService.GetManagerCommentsForNomination(nominationId),
                Manager = _nominationService.GetManagerNameOfCurrentNomination(nominationId),
                NomineeName = _nominationService.GetNomineeNameOfCurrentNomination(nominationId),
                ProjectOrDepartment = projectOrDept,
                Criterias = _nominationService.GetCriteriaForNomination(nominationId),
                ReviewerId = _nominationService.GetReviewerIdOfCurrentNomination(userEmailAddress),
                NominationId = result.Id,
                ManagerComment = result.Comment
            };

            return View(reviewNominationViewModel);

        }


        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult ReviewNomination(ReviewSubmitionViewModel model, string Submit)
        {
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
                    CriteriaId = item.Id,
                    Comment = item.Comment != null ? _textInfo.ToTitleCase(item.Comment) : "",
                    Credit = Convert.ToInt32(item.Credit),
                    ReviewId = review.Id

                };
                _nominationService.AddReviewerCommentsForCurrentNomination(revrComment);
            }
            return RedirectToAction("Index", "Home");
        }


        [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult SavedReviews()
        {

            var reviewedNominations = new List<NominationListViewModel>();
            var reviewerId = _nominationService.GetReviewerIdOfCurrentNomination(User.Identity.Name);
            if (reviewerId == 0)
            {
                ViewBag.erroMessage = "You are not authorized to view this page, please contact system administrator.";
                return View(reviewedNominations);
            }

            var nominations = _nominationService.GetAllSubmitedReviewedNominations(reviewerId);
            foreach (var nomination in nominations)
            {
                var awardName = _encourageDatabaseContext.Query<Award>().Where(a => a.Id == nomination.AwardId).FirstOrDefault().Code;
                var nomineeName = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nomination.Id).FirstOrDefault().NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                var reviewNominationViewModel = new NominationListViewModel()
                {
                    Intials = nomineeName.FirstName.Substring(0, 1) + "" + nomineeName.LastName.Substring(0, 1),
                    AwardName = awardName,
                    DisplayName = nomineeName.DisplayName,
                    NominationTime = nominationTimeToDisplay,
                    Id = nomination.Id,
                    IsSubmitted = nomination.IsSubmitted
                };
                reviewedNominations.Add(reviewNominationViewModel);
            }

            return View(reviewedNominations);

        }
        #endregion


    }
}
#endregion