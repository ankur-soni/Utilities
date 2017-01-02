using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System.Linq;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class PositionsController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        #region Position

        public ActionResult GetPositionDetails([DataSourceRequest] DataSourceRequest request)
        {
            var positionlist = _positionService.GetPositionDetails().Where(y => y.IsDeleted != true).OrderByDescending(model => model.PositionId);
            return Json(positionlist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Positions()
        {
            return PartialView();
        }

        public ActionResult PositionDelete(Position position)
        {
            //var position = _positionService.GetPositionDetails().Where(model => model.PositionId == PositionId).SingleOrDefault();
            if (position != null && ModelState.IsValid)
            {
                _positionService.Delete(position);
                return Json(1);
            }
            else
            {
                return Json(-1);
            }
        }

        public JsonResult IsDuplicatePositionName([Bind(Prefix = "PanelName")]string positionName)
      {
            bool flag = true;
            if (_positionService.GetPositionByName(positionName) != null)
                flag = false;

            return Json(flag);
        }

        public ActionResult PositionSave([DataSourceRequest] DataSourceRequest dsRequest, Position position)
        {
            var positionDetails = _positionService.GetPositionDetails().Where(model => model.PositionName == position.PositionName && model.PositionName != position.PositionName);
            if (positionDetails.Count() > 0)
                ModelState.AddModelError(string.Empty, "Postion already exists, please create with other name.");
            else if (ModelState.IsValid )
            {
                if (position.PositionId == 0)
                {
                    _positionService.Add(position);
                    return Json(position);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult PositionUpdate([DataSourceRequest] DataSourceRequest dsRequest, Position position)
        {
            if (ModelState.IsValid)
            {
                if (position.PositionId != 0)
                {
                    _positionService.Update(position);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }
        #endregion Position


        public ActionResult LocalStorage()
        {
            return View();
        }

    }
}