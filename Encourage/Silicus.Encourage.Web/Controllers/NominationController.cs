using Silicus.Encourage.Services.Interface;
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

        public NominationController( IAwardService awardService)
        {
            _awardService = awardService;
        }
               
        // GET: Nomination/Create
        public ActionResult AddNomination()
        {
            ViewBag.Awards= new SelectList(_awardService.GetAllAwards(), "Id", "Name");
            //ViewBag.ProjectsUnderCurrentUser
            //    = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager(Session["UserEmailAddress"] as string), "Id", "Name"); 

            ViewBag.ProjectsUnderCurrentUser
                = new SelectList(_awardService.GetProjectsUnderCurrentUserAsManager("shailendra.birthare@silicus.com"), "Id", "Name"); 
 
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
