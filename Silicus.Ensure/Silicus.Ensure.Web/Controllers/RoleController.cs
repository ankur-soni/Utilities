using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Services.Interfaces;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;

namespace Silicus.Ensure.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRolesService _rolesService;

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public RoleController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public ActionResult GetRoleDetails([DataSourceRequest] DataSourceRequest request)
        {
            var userdetails = RoleManager.Roles.ToList();

            IList<RoleViewModel> modelList = userdetails.Select(userdetail => new RoleViewModel()
            {   
                RoleName = userdetail.Name,
                Description = userdetail.Name
            }).ToList();

            return Json(modelList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole(role.RoleName);
                var roleresult = await RoleManager.CreateAsync(identityRole);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return Json(-1);
                }
                return Json(_rolesService.Add(role));
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateRole(Role role)
        {
            if (role != null && ModelState.IsValid)
            {
                _rolesService.Update(role);
                return Json(1);
            }

            return Json(-1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRole(Role role)
        {
            if (role != null && ModelState.IsValid)
            {
                //var role = _rolesService.GetRoleDetails().FirstOrDefault(x => x.RoleId == roleId);
                _rolesService.Delete(role);
                return Json(1);
            }

            return Json(-1);
        }
    }
}
