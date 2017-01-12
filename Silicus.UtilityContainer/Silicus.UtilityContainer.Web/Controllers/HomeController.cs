using System;
using System.Linq;
using System.Web.Mvc;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Models.ViewModels;
using Silicus.UtilityContainer.Services.Interfaces;
using System.Collections.Generic;
using Silicus.UtilityContainer.Web.Models;
using Silicus.UtilityContainer.Web.Filters;

namespace Silicus.UtilityContainer.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IUtilityService _utilityService;

        public HomeController(IUtilityService utilityService, IRoleService roleService, IUserService userService)
        {
            _utilityService = utilityService;
            _roleService = roleService;
            _userService = userService;
        }


        public ActionResult Index(string data)
        {
            var allUtilities = _utilityService.GetAllUtilities();
            ViewBag.noRoleForCUrrentUser = data;
            return View("Dashboard", allUtilities);
        }


        public FileContentResult GetImg(int id)
        {
            var byteArray = _utilityService.FindUtility(id).UtilityIcon;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }

        [SuperUserOnly]
        public ActionResult AddRolesToUserForAUtility()
        {
            var newUserRole = new UtilityUserRoleViewModel();
            // ViewData["User"] = new SelectList(_userService.GetAllUsers(), "ID", "DisplayName", "Select");

            var selectListItems =
                _userService.GetAllUsers()
                    .Select(u => new SelectListItem {Text = u.ID.ToString(), Value = u.DisplayName})
                    .ToList();

            ViewData["User"] = selectListItems;

            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name", "Select");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name", "Select");
            //var role = _roleService.GetAllRoles();
            return View(newUserRole);
        }

        public ActionResult GetAllUsersByRoleInUtility(int utilityId,int roleId)
        {
           var users = _userService.GetAllUsersByRoleInUtility(utilityId, roleId);
            var allUsers = _userService.GetAllUsers();
            var availableUsers = new List<UsersWithRolesPerUtilityViewModel>();
            var selectedUsers = new List<UsersWithRolesPerUtilityViewModel>();
            foreach (var item in allUsers)
            {
                if (users.Find( x => x.ID == item.ID) != null)
                {
                    availableUsers.Add(new UsersWithRolesPerUtilityViewModel { UserName = item.DisplayName, UserId = item.ID, Status = true});
                }
                else
                {
                    availableUsers.Add(new UsersWithRolesPerUtilityViewModel { UserName = item.DisplayName, UserId = item.ID });
                }
                
            }
            foreach (var item in users)
            {
                selectedUsers.Add(new UsersWithRolesPerUtilityViewModel { UserName = item.DisplayName, UserId = item.ID });
            }
            
            return Json(new { availableItems = availableUsers, selectedItems = selectedUsers }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SuperUserOnly]
        public ActionResult AddRolesToUserForAUtility(int utilityId, int roleId, int[] userIds)
        {
            if (roleId != 0)
            {
                _userService.AddRolesToUserForAUtility(new UtilityUserRoleViewModel
                {
                    UtilityId = utilityId,
                    RoleId = roleId,
                    UserId = userIds.ToList()
                });
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [SuperUserOnly]
        public ActionResult AddRoleToUtility()
        {
            var newUtilityRole = new UtilityRole();
            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name");
           // ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name");
            return View(newUtilityRole);
        }

        [HttpPost]
        [SuperUserOnly]
        public ActionResult AddRoleToUtility(int utilityId, int[] roleIds)
        {
           // ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name");
            //ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name");
            _utilityService.SaveUtilityRole(new UtilityRoleViewModel { UtilityId = utilityId, RoleIds = roleIds.ToList() });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllRoles(int utilityId)
        {
            var allRoles = _roleService.GetAllRoles();
            var utilityRoles = _utilityService.GetAllRolesForAnUtility(utilityId);
            var rolesToSend = new List<RolesViewModel>();
            foreach (var role in allRoles)
            {
                if (utilityRoles.Find( x => x.RoleID == role.ID) != null)
                {
                    rolesToSend.Add(new RolesViewModel { Id = role.ID, Name = role.Name, AlreadyExistsInSelectedUtility = true });

                }
                else
                {
                    rolesToSend.Add(new RolesViewModel { Id = role.ID, Name = role.Name });
                }
            }
            return Json(rolesToSend, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillRoles(int utilityId)
        {
            var roles = _utilityService.GetAllRolesForAnUtility(utilityId);
            //SelectList obgroles = new SelectList(roles, "Id", "RoleName", 0);
            var roleData = roles.Select(m => new SelectListItem
            {
                Text = _roleService.GetRoleName(m.RoleID),
                Value = m.RoleID.ToString()
            });
            return Json(roleData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public string GetUserByID(string userId)
        {
            var user = _userService.GetUserByID(Convert.ToInt32(userId));


            return user.DisplayName;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}