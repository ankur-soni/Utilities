﻿using System.Threading.Tasks;
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
using System.IO;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Entities;
using System.Configuration;

namespace Silicus.Ensure.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMappingService _mappingService;

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

        public UserController(IUserService userService, MappingService mappingService)
        {
            _userService = userService;
            _mappingService = mappingService;
        }

        public ActionResult Dashboard()
        {

            return View();
        }
        /// <summary>
        /// Get the list of all user except candidate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetUserDetails([DataSourceRequest] DataSourceRequest request, string RoleName)
        {
            var userlist = _userService.GetUserDetails().Where(x => x.Role.ToLower() != RoleName.ToLower()).ToArray();

            var viewModels = _mappingService.Map<User[], UserViewModel[]>(userlist);

            for (int j = 0; j < viewModels.Count(); j++)
            {
                var userDetails = await UserManager.FindByEmailAsync(viewModels[j].Email);
                if (userDetails != null)
                {
                    var viewUsersRole = await UserManager.GetRolesAsync(userDetails.Id);
                    viewModels[j].Role = viewUsersRole.FirstOrDefault();
                }
            }

            DataSourceResult result = viewModels.ToDataSourceResult(request);
            return Json(result);
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUser(int UserId)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserById(UserId);
                if (user != null)
                {
                    _userService.Delete(user);
                }
                return Json(1);
            }

            return Json(-1);
        }

        /// <summary>
        /// Showing all candidate list in grid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetCandidateDetails([DataSourceRequest] DataSourceRequest request, string RoleName)
        {
            var userlist = _userService.GetUserDetails().Where(p => p.Role.ToLower() == RoleName.ToLower()).ToArray();

            var viewModels = _mappingService.Map<User[], UserViewModel[]>(userlist);

            for (int j = 0; j < viewModels.Count(); j++)
            {
                var userDetails = await UserManager.FindByEmailAsync(viewModels[j].Email);
                if (userDetails != null)
                {
                    var viewUsersRole = await UserManager.GetRolesAsync(userDetails.Id);
                    viewModels[j].Role = viewUsersRole.FirstOrDefault();
                }
            }

            DataSourceResult result = viewModels.ToDataSourceResult(request);
            return Json(result);
        }

        /// <summary>
        /// Check for duplicate mail id in identity table as well as user table in the application
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult IsDuplicateEmail(string Email, int UserId)
        {
            bool flag = true;
            var userDetails = _userService.GetUserDetails();
            if (UserId == 0 && UserManager.FindByEmailAsync(Email).Result != null || userDetails.Any(x => x.Email == Email))
            {
                flag = false;
            }
            else
            {
                if (UserManager.FindByEmailAsync(Email).Result != null && userDetails.Any(x => x.Email == Email) && userDetails.Where(x => x.Email == Email).Count() > 1)
                {
                    flag = false;
                }
            }
            return Json(flag, JsonRequestBehavior.AllowGet); ;
        }

        /// <summary>
        /// Add or update user details
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CandidateSave(UserViewModel vuser, HttpPostedFileBase files)
        {
            string actionErrorName = vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower() ? "CandidateAdd" : "PanelAdd";
            string controllerName = vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower() ? "Admin" : "Panel";
            try
            {

                if (vuser.UserId != 0)
                {
                    UpdateUserMethod(vuser, files);
                }
                else
                {
                    vuser = await CreateUserMethod(vuser, files);

                    if (!string.IsNullOrWhiteSpace(vuser.ErrorMessage)) { return RedirectToAction(actionErrorName, controllerName, new { UserId = vuser.UserId }); }

                    var organizationUserDomainModel = _mappingService.Map<UserViewModel, User>(vuser);
                    vuser.UserId = _userService.Add(organizationUserDomainModel);

                }

                if (vuser.CandidateList.Any())
                {
                    foreach (int i in vuser.CandidateList)
                    {
                        var updateUser = _userService.GetUserById(i);
                        updateUser.PanelId = vuser.UserId;
                        updateUser.PanelName = vuser.FirstName + " " + vuser.LastName;
                        _userService.Update(updateUser);
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                vuser.ErrorMessage = ex.Message;
                TempData["UserViewModel"] = vuser;
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                return RedirectToAction(actionErrorName, controllerName, new { UserId = vuser.UserId });

            }
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return RedirectToAction(vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower() ? "Candidates" : "Index", controllerName);
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
                    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
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
