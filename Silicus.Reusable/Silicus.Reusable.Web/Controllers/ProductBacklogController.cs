using Silicus.FrameworxProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.FrameworxProject.Web.Controllers
{
    public class ProductBacklogController : Controller
    {
        private readonly IProductBacklogService _productBacklogService;

        public ProductBacklogController(IProductBacklogService productBacklogService)
        {
            _productBacklogService = productBacklogService;
        }

        // GET: AllProductBacklog
        public ActionResult ShowAllProductBacklog()
        {
            var productbacklogList = _productBacklogService.GetAllProductBacklog().ToList();
            return View(productbacklogList);
        }
    }
}
