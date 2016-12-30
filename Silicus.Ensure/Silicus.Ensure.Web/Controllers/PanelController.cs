using System.Linq;
using System.Web.Mvc;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Models.DataObjects;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

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

        #region Panel

        public ActionResult GetPanelDetails([DataSourceRequest] DataSourceRequest request)
        {
            var panellist = _panelService.GetPanelDetails();
            if (panellist.Any())
            {
                panellist = panellist.OrderByDescending(model => model.PanelId);
            }
            return Json(panellist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Panels()
        {
            return PartialView();
        }

        public ActionResult PanelDelete(Panel panel)
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

        public JsonResult IsDuplicatePanelName(string panelName)
        {
            bool flag = true;
            if (_panelService.GetPanelByName(panelName) != null)
                flag = false;

            return Json(flag);
        }

        public ActionResult PanelSave([DataSourceRequest] DataSourceRequest dsRequest, Panel panel)
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

        public ActionResult PanelUpdate([DataSourceRequest] DataSourceRequest dsRequest, Panel panel)
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
        #endregion Panel
    }
}