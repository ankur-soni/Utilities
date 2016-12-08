﻿using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Silicus.Ensure.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMappingService _mappingService;
        private readonly IPositionService _positionService;

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

        public ManageUserController(IUserService userService, MappingService mappingService, PositionService positionService)
        {
            _positionService = positionService;
            _userService = userService;
            _mappingService = mappingService;
        }
        // GET: ManageUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUser(int UserId, string RoleN)
        {
            UserViewModel currUser = new UserViewModel();
            currUser.UserId = UserId;

            if (UserId != 0)
            {
                var userList = _userService.GetUserDetails();
                var user = userList.FirstOrDefault(x => x.UserId == UserId);
                currUser = _mappingService.Map<User, UserViewModel>(user);
            }
            else if (TempData["UserViewModel"] != null)
            {
                currUser = TempData["UserViewModel"] as UserViewModel;
            }

            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            currUser.PositionList = positionDetails.ToList();
            return View(currUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> SaveUser(UserViewModel userViewModel, HttpPostedFileBase files)
        {
            string actionErrorName = "AddUser";
            string controllerName = "ManageUser";
            try
            {
                if (userViewModel.UserId != 0)
                {
                    UpdateUserMethod(userViewModel, files);
                }
                else
                {
                    userViewModel = await CreateUserMethod(userViewModel, files);

                    if (!string.IsNullOrWhiteSpace(userViewModel.ErrorMessage)) { return RedirectToAction(actionErrorName, controllerName, new { UserId = userViewModel.UserId, RoleN = userViewModel.Role }); }

                    var organizationUserDomainModel = _mappingService.Map<UserViewModel, User>(userViewModel);
                    userViewModel.UserId = _userService.Add(organizationUserDomainModel);

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                userViewModel.ErrorMessage = ex.Message;
                TempData["UserViewModel"] = userViewModel;
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                return RedirectToAction(actionErrorName, controllerName, new { UserId = userViewModel.UserId, RoleN = userViewModel.Role });

            }
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return RedirectToAction("Dashboard", "Admin");
        }

        /// <summary>
        /// update user 
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        private void UpdateUserMethod(UserViewModel vuser, HttpPostedFileBase files)
        {
            var user = _userService.GetUserById(vuser.UserId);
            if (files != null && vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
            {
                vuser.ResumePath = GetFilePath(files);
            }
            if (user != null)
            {
                var organizationUserDomainModel = _mappingService.Map<UserViewModel, User>(vuser);
                organizationUserDomainModel.TestStatus = user.TestStatus;
                _userService.Update(organizationUserDomainModel);
            }

        }
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private async Task<UserViewModel> CreateUserMethod(UserViewModel vuser, HttpPostedFileBase files)
        {
            var user = new ApplicationUser { UserName = vuser.Email, Email = vuser.Email };
            if (vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
            {
                vuser.TestStatus = TestStatus.NotAssigned.ToString();
            }

            vuser.NewPassword = vuser.FirstName.ToUpper() + vuser.LastName.ToLower() + "@123456";
            vuser.ConfirmPassword = vuser.FirstName.ToUpper() + vuser.LastName.ToLower() + "@123456";
            var userResult = await UserManager.CreateAsync(user, vuser.NewPassword);
            if (userResult.Succeeded)
            {
                vuser.IdentityUserId = new Guid(user.Id);
                if (files != null)
                {
                    vuser.ResumePath = GetFilePath(files);
                }
                var result = await UserManager.AddToRoleAsync(user.Id, vuser.Role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                }

            }
            else
            {
                ModelState.AddModelError("", userResult.Errors.First());
                vuser.ErrorMessage = userResult.Errors.First();
                TempData["UserViewModel"] = vuser;
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            }
            return vuser;
        }
        /// <summary>
        /// Return file path
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private string GetFilePath(HttpPostedFileBase files)
        {
            var fileName = Path.GetFileName(files.FileName);
            var path = Path.Combine(Server.MapPath("~/CandidateResume"), fileName);
            files.SaveAs(path);
            string fl = path.Substring(path.LastIndexOf("\\"));
            string[] split = fl.Split('\\');
            string newpath = split[1];
            string resumepath = "/CandidateResume/" + newpath;
            return resumepath;
        }
    }
}