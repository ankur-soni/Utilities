using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectDetailService _projectDetailService;

        public ProjectController(IProjectDetailService projectDetailService)
        {
            _projectDetailService = projectDetailService;
        }

        public ActionResult GetProjectDetails([DataSourceRequest] DataSourceRequest request)
        {
            var projectDetails = _projectDetailService.GetProjectDetails();
            return Json(projectDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProject(ProjectDetail projectDetail)
        {
            if (projectDetail != null && ModelState.IsValid)
            {
                return Json(_projectDetailService.Add(projectDetail));
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateProject(ProjectDetail projectDetail)
        {
            if (projectDetail != null && ModelState.IsValid)
            {
                _projectDetailService.Update(projectDetail);
                return Json(1);
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProject(ProjectDetail projectDetail)
        {
            if (projectDetail != null && ModelState.IsValid)
            {
                _projectDetailService.Delete(projectDetail);
                return Json(1);
            }

            return Json(-1);
        }
    }
}
