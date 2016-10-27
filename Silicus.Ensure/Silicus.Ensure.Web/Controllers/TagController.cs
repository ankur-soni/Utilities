using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagsService _tagsService;
        private readonly IMappingService _mappingService;

        public TagController(ITagsService tagService, MappingService mappingService)
        {
            _tagsService = tagService;
            _mappingService = mappingService;
        }      

        public ActionResult GetTagsDetails([DataSourceRequest] DataSourceRequest request)
        {
            var tagDetails = _tagsService.GetTagsDetails();
            return Json(tagDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateTag(Tags tag)
        {
            if (tag != null && ModelState.IsValid)
            {
                return Json(_tagsService.Add(tag));
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateTag(Tags tag)
        {
            if (tag != null && ModelState.IsValid)
            {
                _tagsService.Update(tag);
                return Json(1);
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteTag(Tags tag)
        {
            if (tag != null && ModelState.IsValid)
            {
                _tagsService.Delete(tag);
                return Json(1);
            }

            return Json(-1);
        }       
    }
}
