using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Models;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Encourage.Web.Controllers
{
    public class NominationController : Controller
    {
        IAwardService _awardService;

        public NominationController(IAwardService awardService)
        {
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
            ViewBag.DepartmentsUnderCurrentUser = new SelectList(_awardService.GetDepartmentsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name");
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
            var userIdToExcept = _awardService.GetUserIdFromEmail("shailendra.birthare@silicus.com");

            var usersInDepartment = _awardService.GetResourcesUnderDepartment(departmentID, userIdToExcept);
            return Json(usersInDepartment, JsonRequestBehavior.AllowGet);
        }
    }
}
