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
    public class NominationController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly INominationService _nominationService;
        private readonly ICommonDbService _commonDbService;
        private readonly ICommonDataBaseContext _commonDbContext;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.Encourage.DAL.Interfaces.IDataContextFactory _dataContextFactory;

        public NominationController(INominationService nominationService, Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, IAwardService awardService)
        {
            _nominationService = nominationService;
            _commonDbService = commonDbService;
            _commonDbContext = _commonDbService.GetCommonDataBaseContext();
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
        }

        // GET: Nomination/Create
        public ActionResult AddNomination()
        {
            var userEmailAddress = Session["UserEmailAddress"] as string;
            ViewBag.Awards = new SelectList(_awardService.GetAllAwards(), "Id", "Name");

            //ViewBag.ProjectsUnderCurrentUser
            //    = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager(userEmailAddress), "Id", "Name"); 
            //ViewBag.ManagerId = _awardService.GetUserIdFromEmail(userEmailAddress);

            ViewBag.ProjectsUnderCurrentUser
                = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name");
            ViewBag.ManagerId = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
            ViewBag.DepartmentsUnderCurrentUser = new SelectList(_awardService.GetDepartmentsUnderCurrentUserAsManager("tushar.surve@silicus.com"), "Id", "Name");
            ViewBag.Resources = new SelectList(new List<User>(), "Id", "DisplayName");
            return View();
        }

        [HttpPost]
        public ActionResult AddNomination(NominationViewModel model, string submit)
        {
            var nomination = new Nomination();
            nomination.AwardId = model.AwardId;
            nomination.ManagerId = model.ManagerId;
            nomination.UserId = model.ResourceId;

            if (model.SelectResourcesBy.Equals("Project"))
                nomination.ProjectID = model.ProjectID;
            else if (model.SelectResourcesBy.Equals("Department"))
                nomination.DepartmentId = model.DepartmentId;

            nomination.NominationDate = DateTime.Now.Date;
            nomination.IsPLC = model.IsPLC;

            if (submit.Equals("Submit"))
                nomination.IsSubmitted = true;
            else
                nomination.IsSubmitted = false;

            foreach (var criteria in model.Comments)
            {
                if (criteria.Comment != null)
                {
                    nomination.ManagerComments.Add(
                        new ManagerComment()
                        {
                            CriteriaId = criteria.Id,
                            Comment = criteria.Comment
                        }
                        );
                }
            }
            var isNominated = _awardService.AddNomination(nomination);
            return RedirectToAction("Dashboard", "Dashboard");
        }


        [HttpPost]
        public JsonResult CriteriasForAward(int awardId)
        {
            var criteriaList = _awardService.GetCriteriasForAward(awardId);
            return Json(criteriaList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditSavedNomination(int nominationId)
        {
            var savedNomination = _nominationService.GetNomination(nominationId);
            var nominationViewModel = new NominationViewModel();

            var userEmailAddress = Session["UserEmailAddress"] as string;
            ViewBag.Awards = new SelectList(_awardService.GetAllAwards(), "Id", "Name");

            ViewBag.ProjectsUnderCurrentUser
                = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name");
            var currentUserId = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");
            ViewBag.ManagerId = currentUserId;

            ViewBag.DepartmentsUnderCurrentUser = new SelectList(_awardService.GetDepartmentsUnderCurrentUserAsManager("tushar.surve@silicus.com"), "Id", "Name");

            if (savedNomination.ProjectID != null)
            {
                nominationViewModel.SelectResourcesBy = "Project";
                ViewBag.Resources = new SelectList(_awardService.GetResourcesInEngagement(savedNomination.ProjectID.Value, currentUserId), "Id", "DisplayName");
            }
            else if (savedNomination.DepartmentId != null)
            {
                nominationViewModel.SelectResourcesBy = "Department";
                ViewBag.Resources = new SelectList(_awardService.GetResourcesUnderDepartment(savedNomination.DepartmentId.Value, _awardService.GetUserIdFromEmail("tushar.surve@silicus.com")), "Id", "DisplayName");
            }

            //IN FUTURE GOING TO USE MAPPER
            nominationViewModel.AwardId = savedNomination.AwardId;
            nominationViewModel.ManagerId = savedNomination.ManagerId;
            nominationViewModel.ProjectID = savedNomination.ProjectID;
            nominationViewModel.DepartmentId = savedNomination.DepartmentId;
            nominationViewModel.IsPLC = savedNomination.IsPLC.Value;
            nominationViewModel.ResourceId = savedNomination.UserId;
            nominationViewModel.IsSubmitted = savedNomination.IsSubmitted;

            return View("EditNomination", nominationViewModel);
        }

        [HttpGet]
        public JsonResult ResourcesInProject(int engagementID)
        {
            //var userIdToExcept = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string);
            var userIdToExcept = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");

            var usersInEngagement = _awardService.GetResourcesInEngagement(engagementID, userIdToExcept);
            return Json(usersInEngagement, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ResourcesInDepartment(int departmentID)
        {
            //var userIdToExcept = _awardService.GetUserIdFromEmail(Session["UserEmailAddress"] as string);
            var userIdToExcept = _awardService.GetUserIdFromEmail("tushar.surve@silicus.com");

            var usersInDepartment = _awardService.GetResourcesUnderDepartment(departmentID, userIdToExcept);
            return Json(usersInDepartment, JsonRequestBehavior.AllowGet);
        }

        // GET: Nomination
        [HttpGet]
        public ActionResult ReviewNominations()
        {
            var nominations = _nominationService.GetAllSubmitedNominations();
            var reviewNominations = new List<NominationListViewModel>();
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
                    Id = nomination.Id
                };
                reviewNominations.Add(reviewNominationViewModel);
            }
            return View(reviewNominations);
        }

        [HttpGet]
        public ActionResult SavedNomination()
        {
            var nominations = _nominationService.GetAllNominations();
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
    }
}
