using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services;
using System.Web.Mvc;

namespace Silicus.UtilityContainer.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly UtilityService _utilityService=new UtilityService();
        private readonly RoleService _roleService = new RoleService();
        private readonly UserService _userService = new UserService();

        //public HomeController(IUtilityService utilityService)
        //{
        //    _utilityService = utilityService;
        //}

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