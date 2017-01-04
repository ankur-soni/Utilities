using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IPanelService _panelService;

        public CommonController(IPanelService panelService)
        {
            _panelService = panelService;
        }

        public ActionResult GetPanelDetails()
        {
            var panellist = _panelService.GetPanelDetails();
            if (panellist.Any())
            {
                panellist = panellist.OrderByDescending(model => model.PanelId);
            }
            return Json(panellist, JsonRequestBehavior.AllowGet);
        }
    }
}