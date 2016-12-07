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

        public JsonResult IsDuplicatePositionName(string positionName)
        {
            bool flag = true;
            if (_positionService.GetPositionByName(positionName) != null)
                flag = false;

            return Json(flag);
        }

        public ActionResult PositionSave(Position position)
        {
            var result = 0;
            var positionList = _positionService.GetPositionDetails().Where(x => x.PositionName.ToLower() == position.PositionName.ToLower()).ToList();
            if (positionList.Any())
                result = positionList.Count;

            if (ModelState.IsValid)
            {
                if (position.PositionId == 0 && Convert.ToInt32(result) == 0)
                {
                    _positionService.Add(position);
                    return Json(_positionService.GetPositionDetails().LastOrDefault().PositionId);
                }
                else if (position.PositionId != 0 && Convert.ToInt32(result) == 1)
                {
                    _positionService.Update(position);
                    return Json(position.PositionId);
                }
            }
            return null;
        }
        #endregion Position


        public ActionResult LocalStorage()
        {
            return View();
        }

    }
}