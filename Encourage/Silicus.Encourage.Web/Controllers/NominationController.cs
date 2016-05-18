using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models.DataObjects;
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

        // GET: Nomination
        public ActionResult ReviewNominations()
        {
            var nominations = _nominationService.GetAllNominations();
            var reviewNominations = new List<ReviewNominationViewModel>();
            foreach (var nomination in nominations)
            {

                var awardName = _encourageDatabaseContext.Query<Award>().Where(a => a.Id == nomination.AwardId).FirstOrDefault().Code;
                var nomineeName = _commonDbContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault();
                var nominationTime = _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nomination.Id).FirstOrDefault().NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
                var reviewNominationViewModel = new ReviewNominationViewModel()
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


        // GET: Nomination/Create
        public ActionResult AddNomination()
        {
            ViewBag.Awards= new SelectList(_awardService.GetAllAwards(), "Id", "Name");
            //ViewBag.ProjectsUnderCurrentUser
            //    = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager(Session["UserEmailAddress"] as string), "Id", "Name"); 

            ViewBag.ProjectsUnderCurrentUser
                = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name"); 
 
            ViewBag.Resources=new SelectList(new List<User>(),"Id","DisplayName");

            
            return View();
        }

        [HttpPost]
        public ActionResult AddNomination(NominationViewModel model)
        {
                 
            return View();
        }

        [HttpGet]
        public JsonResult CriteriasForAward(int awardId)
        {
            var criteriaList = _awardService.GetCriteriasForAward(awardId);
            return Json(criteriaList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ResourcesInProject(int engagementID)
        {
            var usersInEngagement = _awardService.GetResourcesInEngagement(engagementID);
            return Json(usersInEngagement,JsonRequestBehavior.AllowGet);
        }    
    }
}
