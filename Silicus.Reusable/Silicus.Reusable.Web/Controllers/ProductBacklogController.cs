using Kendo.Mvc.UI;
using System.Web.Mvc;
using Silicus.FrameworxProject.Services.Interfaces;
using AutoMapper;
using Silicus.FrameworxProject.Models;
using Silicus.Reusable.Web.Models.ViewModel;
using System.Collections;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;

namespace Silicus.FrameworxProject.Web.Controllers
{
    [Authorize]
    public class ProductBacklogController : Controller
    {
        private readonly IProductBacklogService _productBacklogService;
        private readonly IMapper _mapper;


        public ProductBacklogController(IProductBacklogService productBacklogService, IMapper mapper)
        {
            _productBacklogService = productBacklogService;
            _mapper = mapper;
        }

        public ActionResult GetProductBacklogs([DataSourceRequest] DataSourceRequest request)
        {
            var productBacklogs = _productBacklogService.GetAllProductBacklog();
            var productBacklogViewModels = _mapper.Map<IEnumerable<ProductBacklog>, IEnumerable<ProductBacklogViewModel>>(productBacklogs);
            return Json(productBacklogViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // GET: AllProductBacklog
        public ActionResult ShowAllProductBacklog()
        {
            return View();
        }
    }
}
