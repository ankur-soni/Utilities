using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Mappings;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private readonly IPanelService _panelService;

        public PanelController(IPanelService panelService)
        {
            _panelService = panelService;
        }

        #region Position

        public ActionResult GetPositionDetails([DataSourceRequest] DataSourceRequest request)
        {
            var positionlist = _panelService.GetPanelDetails().Where(y => y.IsDeleted != true).OrderByDescending(model => model.PanelId);
            return Json(positionlist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Positions()
        {
            return PartialView();
        }

        public ActionResult PositionDelete(Panel panel)
        {
            if (panel != null && ModelState.IsValid)
            {
                _panelService.Delete(panel);
                return Json(1);
            }
            else
            {
                return Json(-1);
            }
        }

        public JsonResult IsDuplicatePositionName(string positionName)
        {
            bool flag = true;
            if (_panelService.GetPositionByName(positionName) != null)
                flag = false;

            return Json(flag);
        }

        public ActionResult PositionSave([DataSourceRequest] DataSourceRequest dsRequest, Panel panel)
        {
            if (ModelState.IsValid)
            {
                if (panel.PanelId == 0)
                {
                    _panelService.Add(panel);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult PositionUpdate([DataSourceRequest] DataSourceRequest dsRequest, Panel panel)
        {
            if (ModelState.IsValid)
            {
                if (panel.PanelId != 0)
                {
                    _panelService.Update(panel);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }
        #endregion Position
    }
}