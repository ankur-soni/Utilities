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
        private readonly ITagsService _tagService;

        public CommonController(IPanelService panelService, ITagsService tagService)
        {
            _panelService = panelService;
            _tagService = tagService;
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

        public ActionResult GetAllTagDetails()
        {
            var taglist = _tagService.GetTagsDetails();
            if (taglist.Any())
            {
                taglist = taglist.OrderByDescending(model => model.TagId);
            }
            return Json(taglist, JsonRequestBehavior.AllowGet);
        }
    }
}