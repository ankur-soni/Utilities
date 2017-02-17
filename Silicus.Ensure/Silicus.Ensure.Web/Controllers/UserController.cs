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
using System.IO;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Services;
using System.Collections.Generic;
using System.Web.Configuration;
using Silicus.Ensure.Web.Application;
using Silicus.Ensure.Models;

namespace Silicus.Ensure.Web.Controllers
{

    //[Authorize]
    public class UserController : Controller
    {
        private readonly IPanelMemberService _panelMemberService;
        private readonly IUserService _userService;
        private readonly IMappingService _mappingService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly IPositionService _positionService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        private ApplicationUserManager _userManager;
        private Silicus.UtilityContainer.Services.Interfaces.IUtilityService _utilityService;
        private Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService _utilityUserRoleService;
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

        public UserController(IUserService userService, MappingService mappingService, ITestSuiteService testSuiteService, PositionService positionService, Silicus.UtilityContainer.Services.Interfaces.IUserService containerUserService,
            Silicus.UtilityContainer.Services.Interfaces.IUtilityService utilityService, Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService utilityUserRoleService, IPanelMemberService panelMemberService)
        {
            _positionService = positionService;
            _userService = userService;
            _mappingService = mappingService;
            _testSuiteService = testSuiteService;
            _containerUserService = containerUserService;
            _utilityService = utilityService;
            _utilityUserRoleService = utilityUserRoleService;
            _panelMemberService = panelMemberService;
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
        public ActionResult GetUserDetails([DataSourceRequest] DataSourceRequest request)
        {
            var userlist = _containerUserService.GetAllUsers();
            var userlistViewModel = _mappingService.Map<List<Silicus.UtilityContainer.Models.DataObjects.User>, List<UserDetailViewModel>>(userlist);
            var UtilityId = getUtilityId();
            var userRoles = _utilityUserRoleService.GetAllUserRolesForUtility(UtilityId);

            var userWithRoles = (from userinRoles in userRoles
                                 join allUsers in userlistViewModel
                                 on userinRoles.UserId equals allUsers.UserId into temp
                                 from j in temp.DefaultIfEmpty()
                                 select new UserDetailViewModel
                                 {
                                     RoleName = userinRoles.Role.Name,
                                     UserName = j.UserName,
                                     Department = j.Department,
                                     Designation = j.Designation,
                                     Email = j.Email,
                                     FirstName = j.FirstName,
                                     FullName = j.FullName,
                                     LastName = j.LastName,
                                     MiddleName = j.MiddleName,
                                     RoleId = userinRoles.Role.ID,
                                     UserId = j.UserId
                                 }).ToList();

            DataSourceResult result = userWithRoles.ToDataSourceResult(request);
            return Json(result);
        }

        private UserDetailViewModel GetAdUserDetails(string email)
        {
            var user = _containerUserService.FindUserByEmail(email);
            var userViewModel = _mappingService.Map<Silicus.UtilityContainer.Models.DataObjects.User, UserDetailViewModel>(user);
            var UtilityId = getUtilityId();
            var userRole = _utilityUserRoleService.GetAllRolesForUser(email);

            var assignedRole = userRole.FirstOrDefault(y => y.Id == userViewModel.UserId);

            if (assignedRole != null)
                userViewModel.RoleName = assignedRole.Role.Name;

            return userViewModel;
        }



        private int getUtilityId()
        {
            var utilityProductId = WebConfigurationManager.AppSettings["ProductId"];
            if (string.IsNullOrWhiteSpace(utilityProductId))
            {
                throw new ArgumentNullException();
            }

            return Convert.ToInt32(utilityProductId);
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUser(UserViewModel userModel)
        {
            if (userModel != null && userModel.UserId > 0)
            {

                _userService.Delete(userModel.UserId);
                return Json(1);
            }

            return Json(-1);
        }

        /// <summary>
        /// Showing all candidate list in grid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetCandidateDetails([DataSourceRequest] DataSourceRequest request, string RoleName)
        {
            _testSuiteService.TestSuiteActivation();

            var userlist = _userService.GetUserDetails().Where(p => p.Role.ToLower() == RoleName.ToLower()).ToArray().Reverse().ToArray();
            var currentUserRoles = MvcApplication.getCurrentUserRoles();
            if (currentUserRoles.Count == 1 && currentUserRoles.Contains(Silicus.Ensure.Models.Constants.RoleName.Panel.ToString()))
            {
                var currentUserMail = HttpContext.User.Identity.Name;
                var user = _containerUserService.FindUserByEmail(currentUserMail);
                if (user != null)
                {
                    userlist = userlist.Where(x => x.PanelId != null && x.PanelId.Contains(Convert.ToString(user.ID))).ToArray();
                }
            }
            var viewModels = _mappingService.Map<UserBusinessModel[], UserViewModel[]>(userlist);
            bool userInRole = User.IsInRole(Silicus.Ensure.Models.Constants.RoleName.Admin.ToString());
            for (int index = 0; index < viewModels.Count(); index++)
            {
                var testSuitId = _testSuiteService.GetUserTestSuiteByUserApplicationId(viewModels[index].UserId);
                viewModels[index].IsAdmin = userInRole;
                viewModels[index].TestSuiteId = testSuitId != null ? testSuitId.TestSuiteId : 0;
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
            if (UserId == 0 && (UserManager.FindByEmailAsync(Email).Result != null || userDetails.Any(x => x.Email == Email)))
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
        /// <param name="user"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CandidateSave(UserViewModel user)
        {
            string actionErrorName = user.Role.ToLower() == RoleName.Candidate.ToString().ToLower() ? "CandidateAdd" : "PanelAdd";
            string controllerName = user.Role.ToLower() == RoleName.Candidate.ToString().ToLower() ? "Admin" : "Panel";
            try
            {
                if (user.ResumeFile != null)
                {
                    UploadResume(user);
                }

                if (user.ProfilePhotoFile != null)
                {
                    UploadProfilePhoto(user);
                }

                if (user.UserId != 0 && !user.IsCandidateReappear)
                {
                    UpdateUserMethod(user);
                }
                else if(user.IsCandidateReappear)
                {
                    CandidateReappear(user);
                }
                else
                {
                    user = await CreateUserMethod(user);

                    if (!string.IsNullOrWhiteSpace(user.ErrorMessage)) { return RedirectToAction(actionErrorName, controllerName, new { UserId = user.UserId }); }

                    var organizationUserDomainModel = _mappingService.Map<UserViewModel, UserBusinessModel>(user);
                    organizationUserDomainModel.IsDeleted = false;
                    _userService.Add(organizationUserDomainModel);
                    TempData["Success"] = "User created successfully!";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                user.ErrorMessage = ex.Message;
                TempData["UserViewModel"] = user;
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                return RedirectToAction(actionErrorName, controllerName, new { UserId = user.UserId });

            }
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return RedirectToAction(user.Role.ToLower() == RoleName.Candidate.ToString().ToLower() ? "Candidates" : "Index", controllerName);
        }

        /// <summary>
        /// update user 
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        private void UpdateUserMethod(UserViewModel vuser)
        {
            var user = _userService.GetUserById(vuser.UserId);
            if (user != null)
            {
                var organizationUserDomainModel = _mappingService.Map<UserViewModel, UserBusinessModel>(vuser);
                organizationUserDomainModel.TestStatus = user.TestStatus;
                organizationUserDomainModel.CandidateStatus = user.CandidateStatus;
                organizationUserDomainModel.IsDeleted = false;
                _userService.Update(organizationUserDomainModel);
                TempData["Success"] = "User updated successfully!";
            }

        }

        /// <summary>
        /// Candidate Reappear 
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        private void CandidateReappear(UserViewModel vuser)
        {
            var user = _userService.GetUserById(vuser.UserId);
            if (user != null)
            {
                var organizationUserDomainModel = _mappingService.Map<UserViewModel, UserBusinessModel>(vuser);
                organizationUserDomainModel.TestStatus = user.TestStatus;
                organizationUserDomainModel.CandidateStatus = user.CandidateStatus;
                organizationUserDomainModel.IsDeleted = false;
                _userService.UpdateUserAndCreateNewApplication(organizationUserDomainModel);
                TempData["Success"] = "User updated successfully!";
            }

        }
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private async Task<UserViewModel> CreateUserMethod(UserViewModel vuser)
        {
            var user = new ApplicationUser { UserName = vuser.Email, Email = vuser.Email };
            if (vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
            {
                vuser.TestStatus = CandidateStatus.New.ToString();
                vuser.CandidateStatus = CandidateStatus.New.ToString();
            }

            vuser.NewPassword = vuser.FirstName.ToUpper() + vuser.LastName.ToLower() + "@123456";
            vuser.ConfirmPassword = vuser.FirstName.ToUpper() + vuser.LastName.ToLower() + "@123456";
            var userResult = await UserManager.CreateAsync(user, vuser.NewPassword);
            if (userResult.Succeeded)
            {
                vuser.IdentityUserId = new Guid(user.Id);
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

        private void UploadProfilePhoto(UserViewModel user)
        {
            var fileModel = new FileUploadModel
            {
                File = user.ProfilePhotoFile,
                FolderName = AppConstants.ProfilePhotoFolderName,
                FileName = user.UserId.ToString() + Path.GetExtension(user.ProfilePhotoFile.FileName)
            };
            UploadFile(fileModel);
            user.ProfilePhotoFilePath = fileModel.FilePath;
        }

        private void UploadResume(UserViewModel user)
        {
            var fileModel = new FileUploadModel
            {
                File = user.ResumeFile,
                FolderName = AppConstants.ResumeFolderName,
                FileName = Guid.NewGuid() + AppConstants.ResumeNameSeparationCharacter + user.ResumeFile.FileName
            };
            UploadFile(fileModel);
            user.ResumePath = fileModel.FilePath;
            user.ResumeName = fileModel.FileName;
        }

        /// <summary>
        /// Return file path
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private void UploadFile(FileUploadModel fileModel)
        {
            fileModel.FilePath = Path.Combine(Server.MapPath(fileModel.FolderName), fileModel.FileName);
            Directory.CreateDirectory(Server.MapPath(fileModel.FolderName));
            fileModel.File.SaveAs(fileModel.FilePath);
            fileModel.FilePath = Path.Combine(fileModel.FolderName, fileModel.FileName);
        }
    }
}
