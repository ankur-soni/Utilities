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
            var positionlist = _positionService.GetPositionDetails().OrderByDescending(model => model.PositionId);
            return Json(positionlist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Positions()
        {
            return PartialView();
        }

        public ActionResult PositionDelete(int PositionId)
        {
            var position = _positionService.GetPositionDetails().Where(model => model.PositionId == PositionId).SingleOrDefault();
            if (position != null)
            {
                _positionService.Delete(position);
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
            if (_positionService.GetPositionByName(positionName) != null)
                flag = false;

            return Json(flag);
        }

        public ActionResult PositionSave(Position position)
        {
            if (ModelState.IsValid)
            {
                if (position.PositionId == 0)
                    return Json(_positionService.Add(position));
                else
                {
                    _positionService.Update(position);
                    return Json(1);
                }
            }
            return null;
        }
        #endregion Position

    }
}