using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services;
using Silicus.UtilityContainer.Services.Interfaces;
using Silicus.UtilityContainer.Web.Filters;
using System.Web.Mvc;

namespace Silicus.UtilityContainer.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUtilityService _utilityService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public HomeController(IUtilityService utilityService,IRoleService roleService,IUserService userService)
        {
            _utilityService = utilityService;
            _roleService = roleService;
            _userService = userService;
        }

        [CustomeAuthorize]
        public ActionResult Index(string data)
        {
            var allUtilities = _utilityService.GetAllUtilities();
            ViewBag.noRoleForCUrrentUser = data;
            return View("Dashboard",allUtilities);
        }

        public FileContentResult GetImg(int id)
        {
            byte[] byteArray = _utilityService.FindUtility(id).UtilityIcon;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }

        [HttpGet]
        public ActionResult AddRolesToUserForAUtility()
        {

            var newUserRole = new UtilityUserRoles();
            ViewData["User"] = new SelectList(_userService.GetAllUsers(), "ID", "DisplayName", "Select");
            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name","Select");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name", "Select");
            //var role = _roleService.GetAllRoles();
            return View(newUserRole);
        }

        [HttpPost]
        public ActionResult AddRolesToUserForAUtility(UtilityUserRoles newUserRole)
        {
            _userService.AddRolesToUserForAUtility(newUserRole);
            return RedirectToAction("Index");
        }

    }
}