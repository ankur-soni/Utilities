using System;
using System.Linq;
using System.Web.Mvc;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Mappings;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Web;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagsService _tagsService;

        public TagController(ITagsService tagsService)
        {           
            _tagsService = tagsService;           
        }

        public ActionResult GetTagsDetails([DataSourceRequest] DataSourceRequest request)
        {
            var tagDetails = _tagsService.GetTagsDetails().OrderByDescending(model => model.TagId);
            return Json(tagDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add(Int32 tagId = 0)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(Tags tag)
        {
            var tagDetails = _tagsService.GetTagsDetails().Where(model => model.TagName == tag.TagName && model.TagId != tag.TagId);
            if (tagDetails.Count() > 0)
                ModelState.AddModelError(string.Empty, "The Tag already exists, please create with other name.");
            if (tag != null && ModelState.IsValid)
            {
                tag.IsActive = true;
                tag.Description = HttpUtility.HtmlDecode(tag.Description);
                _tagsService.Add(tag);
                TempData.Add("IsNewTag", 1);
                return RedirectToAction("List");
            }
            tag.Description = HttpUtility.HtmlDecode(tag.Description);
            return View("Add", tag);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest dsRequest,Tags tag)
        {
            if (tag != null && ModelState.IsValid)
            {
                tag.IsActive = true;
                tag.Description = HttpUtility.HtmlDecode(tag.Description);
                _tagsService.Update(tag);
            }
            return Json(ModelState.ToDataSourceResult());
        }
    }
}