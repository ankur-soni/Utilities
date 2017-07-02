using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Filters;
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
    [CustomAuthorize("Admin")]
    public class ManageUserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMappingService _mappingService;
       // private readonly IPositionService _positionService;
        private readonly IPanelMemberService _panelMemberService;
        private readonly IRecruiterMeberService _recruiterMeberService;

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

        public ManageUserController(IUserService userService, MappingService mappingService,  IPanelMemberService panelMemberService, IRecruiterMeberService recruiterMeberService)
        {
            //_positionService = positionService;
            _userService = userService;
            _mappingService = mappingService;
            _panelMemberService = panelMemberService;
            _recruiterMeberService = recruiterMeberService;
        }
        // GET: ManageUser
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult LoadView(int UserId, string UserRoleName)
        //{
        //    string viewName = "";
        //    UserViewModel userViewModel = SetUserModel(UserId, UserRoleName);
        //    if (UserRoleName.ToLower() == RoleName.Admin.ToString().ToLower() || UserRoleName.ToLower() == RoleName.Panel.ToString().ToLower())
        //        viewName = "_partialAddUserView";
        //    else if (UserRoleName.ToLower() == RoleName.Candidate.ToString().ToLower())
        //        viewName = "_partialAddCandidateUserView";

        //    return View(viewName, userViewModel);
        //}

        //public ActionResult AssignPanelToUserView(UserDetailViewModel userDetailViewModel)
        //{
        //    if (userDetailViewModel != null)
        //    {
        //        var PanelMemberDetails = _panelMemberService.GetPanelMeberDetails(userDetailViewModel.UserId);

        //        var PanelMemberViewModel = _mappingService.Map<PanelMemberDetail, PanelMemberDetailViewModel>(PanelMemberDetails);

        //        if (PanelMemberViewModel == null)
        //        {
        //            PanelMemberViewModel = _mappingService.Map<UserDetailViewModel, PanelMemberDetailViewModel>(userDetailViewModel);
        //        }
        //        return PartialView(@"~\Views\ManageUser\_AssignPanel.cshtml", PanelMemberViewModel);
        //    }
        //    return null;
        //}

        //public ActionResult AssignPanelToUser(PanelMemberDetailViewModel panelMemberDetailViewModel)
        //{
        //    bool issuccess = false;
        //    if (panelMemberDetailViewModel != null)
        //    {
        //        var panelMember= _mappingService.Map<PanelMemberDetailViewModel,PanelMemberDetail >(panelMemberDetailViewModel);
        //        issuccess=_panelMemberService.UpesertPanelMeberDetail(panelMember);
        //       // return PartialView(@"~\Views\ManageUser\_AssignPanel.cshtml", PanelMemberViewModel);
        //    }
        //    return Json(issuccess,JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult AssignTypeToUserView(UserDetailViewModel userDetailViewModel)
        //{
        //    if (userDetailViewModel != null)
        //    {
        //        var RecruiterMeberDetails = _recruiterMeberService.GetRecruiterMeberDetails(userDetailViewModel.UserId);

        //        var RecruiterMeberViewModel = _mappingService.Map<RecruiterMembersDetail, RecruiterMemberDetailViewModel>(RecruiterMeberDetails);

        //        if (RecruiterMeberViewModel == null)
        //        {
        //            RecruiterMeberViewModel = _mappingService.Map<UserDetailViewModel, RecruiterMemberDetailViewModel>(userDetailViewModel);
        //        }
        //        return PartialView(@"~\Views\ManageUser\_AssignType.cshtml", RecruiterMeberViewModel);
        //    }
        //    return null;
        //}

        //public ActionResult AssignTypeToUser(RecruiterMemberDetailViewModel recruiterMeberViewModel)
        //{
        //    bool issuccess = false;
        //    if (recruiterMeberViewModel != null)
        //    {
        //        var recruiterMember = _mappingService.Map<RecruiterMemberDetailViewModel, RecruiterMembersDetail>(recruiterMeberViewModel);
        //        issuccess = _recruiterMeberService.UpesertRecruiterMeberDetail(recruiterMember);
        //        // return PartialView(@"~\Views\ManageUser\_AssignPanel.cshtml", PanelMemberViewModel);
        //    }
        //    return Json(issuccess, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult AddUser(int UserId, string RoleN)
        //{
        //    return View(SetUserModel(UserId, RoleN));
        //}

        //private UserViewModel SetUserModel(int UserId, string UserRoleName)
        //{
        //    UserViewModel userViewModel = new UserViewModel();
        //    try
        //    {
        //        userViewModel.UserId = UserId;

        //        if (userViewModel.UserId != 0)
        //        {
        //            var userList = _userService.GetUserDetails();
        //            var user = userList.FirstOrDefault(x => x.UserId == userViewModel.UserId);
        //            userViewModel = _mappingService.Map<UserBusinessModel, UserViewModel>(user);
        //        }
        //        else if (TempData["UserViewModel"] != null)
        //        {
        //            userViewModel = TempData.Peek("UserViewModel") as UserViewModel;
        //            TempData["UserViewModel"] = userViewModel;
        //        }
        //        //var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
        //        //userViewModel.PositionList = positionDetails.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        userViewModel.ErrorMessage = ex.Message;
        //    }

        //    return userViewModel;
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public async Task<ActionResult> SaveUser(UserViewModel userViewModel, HttpPostedFileBase files)
        //{
        //    string actionErrorName = "AddUser";
        //    string controllerName = "ManageUser";
        //    try
        //    {
        //        if (userViewModel.UserId != 0)
        //        {
        //            UpdateUserMethod(userViewModel, files);
        //        }
        //        else
        //        {
        //            userViewModel = await CreateUserMethod(userViewModel, files);

        //            if (!string.IsNullOrWhiteSpace(userViewModel.ErrorMessage)) { return RedirectToAction(actionErrorName, controllerName, new { UserId = userViewModel.UserId, RoleN = userViewModel.Role }); }

        //            var organizationUserDomainModel = _mappingService.Map<UserViewModel, UserBusinessModel>(userViewModel);
        //            organizationUserDomainModel.IsDeleted = false;
        //            userViewModel.UserId = _userService.Add(organizationUserDomainModel);
        //            TempData["Success"] = "User created successfully!";

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        userViewModel.ErrorMessage = ex.Message;
        //        TempData["UserViewModel"] = userViewModel;
        //        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
        //        return RedirectToAction(actionErrorName, controllerName, new { UserId = userViewModel.UserId, RoleN = userViewModel.Role });

        //    }
        //    ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
        //    return RedirectToAction("Index");
        //}

        /// <summary>
        /// update user 
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        //private void UpdateUserMethod(UserViewModel vuser, HttpPostedFileBase files)
        //{
        //    var user = _userService.GetUserById(vuser.UserId);
        //    if (files != null && vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
        //    {
        //        vuser.ResumePath = GetFilePath(files);
        //    }
        //    if (user != null)
        //    {
        //        vuser.Role = user.Role;
        //        var organizationUserDomainModel = _mappingService.Map<UserViewModel, UserBusinessModel>(vuser);
        //        organizationUserDomainModel.TestStatus = user.TestStatus;
        //        organizationUserDomainModel.IsDeleted = false;
        //        _userService.Update(organizationUserDomainModel);
        //        TempData["Success"] = "User updated successfully.";
        //    }

        //}
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="vuser"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        //private async Task<UserViewModel> CreateUserMethod(UserViewModel vuser, HttpPostedFileBase files)
        //{
        //    var user = new ApplicationUser { UserName = vuser.Email, Email = vuser.Email };
        //    if (vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
        //    {
        //        vuser.TestStatus = CandidateStatus.New.ToString();
        //        vuser.CandidateStatus = CandidateStatus.TestAssigned.ToString();
        //    }

        //    vuser.NewPassword = vuser.FirstName.ToUpper() + vuser.LastName.ToLower() + "@123456";
        //    vuser.ConfirmPassword = vuser.FirstName.ToUpper() + vuser.LastName.ToLower() + "@123456";
        //    var userResult = await UserManager.CreateAsync(user, vuser.NewPassword);
        //    if (userResult.Succeeded)
        //    {
        //        vuser.IdentityUserId = new Guid(user.Id);
        //        if (files != null)
        //        {
        //            vuser.ResumePath = GetFilePath(files);
        //        }
        //        var result = await UserManager.AddToRoleAsync(user.Id, vuser.Role);
        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
        //        }

        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", userResult.Errors.First());
        //        vuser.ErrorMessage = userResult.Errors.First();
        //        TempData["UserViewModel"] = vuser;
        //        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
        //    }
        //    return vuser;
        //}
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