using Kendo.Mvc.UI;
using System.Web.Mvc;
using Silicus.FrameworxProject.Services.Interfaces;
using AutoMapper;
using Silicus.FrameworxProject.Models;
using Silicus.Reusable.Web.Models.ViewModel;
using System.Collections;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Silicus.UtilityContainer.Security;
using System.Web.Configuration;

namespace Silicus.FrameworxProject.Web.Controllers
{
    [Authorize]
    public class ProductBacklogController : Controller
    {
        private readonly IProductBacklogService _productBacklogService;
        private readonly IMapper _mapper;
        private readonly ICommonDbService _commonDbService;

        public ProductBacklogController(IProductBacklogService productBacklogService, IMapper mapper, ICommonDbService commonDbService)
        {
            _productBacklogService = productBacklogService;
            _commonDbService = commonDbService;
            _mapper = mapper;
        }

        public ActionResult GetProductBacklogs([DataSourceRequest] DataSourceRequest request, string projectName)
        {
            var productBacklogs = _productBacklogService.GetAllProductBacklog(projectName);
            var productBacklogViewModels = _mapper.Map<IEnumerable<ProductBacklog>, IEnumerable<ProductBacklogViewModel>>(productBacklogs);
            //foreach (var item in productBacklogViewModels)
            //{
            //    item.IsTaskAssignedToUser = item.Assignee.ToLower().Contains(User.Identity.Name.ToLower());
            //}
            return Json(productBacklogViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // GET: AllProductBacklog
        public ActionResult ShowAllProductBacklog()
        {
            string utility = WebConfigurationManager.AppSettings["ProductName"];
            var _authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
            var userRoles = _authorizationService.GetRoleForUtility(User.Identity.Name, utility);
            ViewBag.IsRolePm = userRoles.Contains("Project Manager");
            return View();
        }

        public JsonResult GetProjects([DataSourceRequest] DataSourceRequest request)
        {
            var result = _productBacklogService.GetTeamProjects();

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTimeAllocated(int Id, double time)
        {
            var result = _productBacklogService.UpdateTimeAllocated(Id, time);

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTimeSpent(int Id, double time)
        {
            _productBacklogService.UpdateTimeSpent(Id, time);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkItemDetails(int Id)
        {
            var productBacklog  = _productBacklogService.GetWorkItemDetails(Id);

            var productBacklogViewModel = _mapper.Map<ProductBacklog, ProductBacklogViewModel>(productBacklog);

            return PartialView("_Details", productBacklogViewModel);            
        }
    }
}
