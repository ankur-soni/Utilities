using Kendo.Mvc.UI;
using Silicus.Ensure.Models;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

namespace Silicus.Ensure.Web.Controllers
{
    public class TechnologyController : Controller
    {
        // GET: Technology
        private readonly ITechnologyService _technologyService;
        private readonly IMappingService _mappingService;
        private readonly UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        public TechnologyController(ITechnologyService technologyService, IMappingService mappingService, UtilityContainer.Services.Interfaces.IUserService containerUserService)
        {
            _technologyService = technologyService;
            _mappingService = mappingService;
            _containerUserService = containerUserService;
        }

        public ActionResult GetAllTechnologies([DataSourceRequest] DataSourceRequest request)
        {
            var technologies = _technologyService.GetAllTechnologies();
            var technologiesViewModel = _mappingService.Map<IEnumerable<TechnologyBusinessModel>, IEnumerable<TechnologyViewModel>>(technologies);
            var jsonResult = technologiesViewModel.ToDataSourceResult(request);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            return View("Technologies");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save([DataSourceRequest] DataSourceRequest dsRequest, TechnologyViewModel technology)
        {
            var technologies = _technologyService.GetAllTechnologies().Where(model => model.TechnologyName == technology.TechnologyName && model.TechnologyId != technology.TechnologyId);
            if (technologies.Any())
                ModelState.AddModelError(string.Empty, "The Technology already exists, please create with other name.");
            if (technology != null)
            {
                var userEmailId = User.Identity.Name;
                var user = _containerUserService.FindUserByEmail(userEmailId);
                if (user != null)
                {
                    technology.CreatedBy = user.ID;
                    technology.CreatedDate = DateTime.Now;
                    technology.IsActive = true;
                    technology.Description = HttpUtility.HtmlDecode(technology.Description);
                    var technologyBusinessModel = _mappingService.Map<TechnologyViewModel, TechnologyBusinessModel>(technology);
                    _technologyService.Add(technologyBusinessModel);
                }
                return Json(technology);
            }
            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest dsRequest, TechnologyViewModel technology)
        {
           
            if (technology != null)
            {
                var userEmailId = User.Identity.Name;
                var user = _containerUserService.FindUserByEmail(userEmailId);
                if (user != null)
                {
                    technology.ModifiedBy = user.ID;
                    technology.ModifiedDate = DateTime.Now;
                    technology.IsActive = true;
                    technology.Description = HttpUtility.HtmlDecode(technology.Description);
                    var technologyBusinessModel = _mappingService.Map<TechnologyViewModel, TechnologyBusinessModel>(technology);
                    _technologyService.Update(technologyBusinessModel);
                }
            }
            return Json(true);
        }

        public JsonResult IsDuplicateTechnologyName(string technologyName, int technologyId)
        {
            bool isAvailable = true;
            if (!string.IsNullOrWhiteSpace(technologyName) && ModelState.IsValid)
            {
                var technology = _technologyService.GetTechnologyByName(technologyName);
                if (technology != null && technology.TechnologyId != technologyId)
                {
                    isAvailable = false;
                }
            }
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTechnologyAssosiatedWithQuetion(string technologyName)
        {
            bool isTagAssosiatedWithQuetion = false;
            if (!string.IsNullOrWhiteSpace(technologyName))
            {
                isTagAssosiatedWithQuetion = _technologyService.IsTechnologyAssosiatedWithQuetion(technologyName);
            }
            return Json(isTagAssosiatedWithQuetion, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllTechnologiesForDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var technologies = _technologyService.GetAllTechnologies();
            var technologiesViewModel = _mappingService.Map<IEnumerable<TechnologyBusinessModel>, IEnumerable<TechnologyViewModel>>(technologies);
            //var jsonResult = technologiesViewModel.ToDataSourceResult(request);
            return Json(technologiesViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}