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
        private readonly IPositionService _positionService;

        public CommonController(IPanelService panelService, ITagsService tagService, IPositionService positionService)
        {
            _panelService = panelService;
            _tagService = tagService;
            _positionService = positionService;
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

        public ActionResult GetAllPositionDetails()
        {
            var positionlist = _positionService.GetPositionDetails().Where(y => y.IsDeleted != true).OrderByDescending(model => model.PositionId);
            return Json(positionlist, JsonRequestBehavior.AllowGet);
        }
    }
}