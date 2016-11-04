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
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;
        private readonly IMappingService _mappingService;

        public SkillController(ISkillService skillService, MappingService mappingService)
        {
            _skillService = skillService;
            _mappingService = mappingService;
        }

        public ActionResult GetSkillDetails([DataSourceRequest] DataSourceRequest request)
        {
            var skillDetails = _skillService.GetSkillDetails();
            return Json(skillDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSkill(Skill skill)
        {
            if (skill != null && ModelState.IsValid)
            {
                return Json(_skillService.Add(skill));
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSkill(Skill skill)
        {
            if (skill != null && ModelState.IsValid)
            {
                _skillService.Update(skill);
                return Json(1);
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSkill(Skill skill)
        {
            if (skill != null && ModelState.IsValid)
            {
                _skillService.Delete(skill);
                return Json(1);
            }

            return Json(-1);
        }       
    }
}
