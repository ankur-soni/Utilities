using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services;
using Silicus.UtilityContainer.Services.Interfaces;
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

        public ActionResult Index()
        {
            var allUtilities = _utilityService.GetAllUtilities();
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

            var newUserRole = new UserRole();
            ViewData["User"] = new SelectList(_userService.GetAllUsers(), "Id", "Name");
            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "Id", "RoleName");
            return View(newUserRole);
        }

        [HttpPost]
        public ActionResult AddRolesToUserForAUtility(UserRole newUserRole)
        {
            ViewData["User"] = new SelectList(_userService.GetAllUsers(), "Id", "Name");
            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "Id", "RoleName");
            _userService.AddRolesToUserForAUtility(newUserRole);
            return RedirectToAction("Index");
        }

    }
}