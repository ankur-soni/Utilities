using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Models;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class CandidateAccountController : Controller
    {
        private readonly ICookieHelper _cookieHelper;
        private readonly ILogger _logger;
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEmailService _emailService;
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

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public CandidateAccountController(ICookieHelper cookieHelper, ILogger logger,
            IDataContextFactory dataContextFactory, IEmailService emailService, IRolesService rolesService)
        {
            _cookieHelper = cookieHelper;
            _logger = logger;
            _dataContextFactory = dataContextFactory;
            _emailService = emailService;
            _rolesService = rolesService;
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Welcome", "Candidate");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
                switch (loginResult)
                {
                    case SignInStatus.Success:
                        _logger.Log("Login-Post-Switch-Sucess");
                        var user = await UserManager.FindByNameAsync(model.UserName);
                        _logger.Log("User: " + user.Email);
                        var isAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");
                        _logger.Log(user.Email + " isAdmin: " + isAdmin);
                        var isCandidate = await UserManager.IsInRoleAsync(user.Id, "Candidate");
                        _logger.Log(user.Email + " isCandidate: " + isCandidate);

                        var isPanel = await UserManager.IsInRoleAsync(user.Id, RoleName.Panel.ToString());
                        _logger.Log(user.Email + " isPanel: " + isPanel);

                        return RedirectToLocal(returnUrl, model.UserName, isAdmin, isCandidate, isPanel);
                    case SignInStatus.LockedOut:
                        ModelState.AddModelError("", "User account is locked out. Please contact administrator.");
                        return View(model);
                    case SignInStatus.RequiresVerification:
                        ModelState.AddModelError("", "Account verification is pending. Please verify your account.");
                        return View(model);
                    case SignInStatus.Failure:
                    default:
                        {
                            _logger.Log(string.Format("Login request failed for user : {0}", model.UserName),
                                LogCategory.Information, GetUserIdentifiableString(model.UserName));

                            var user1 = await UserManager.FindByNameAsync(model.UserName);

                            _logger.Log(string.Format("User Id is: {0}", user1.Id),
                                LogCategory.Information, GetUserIdentifiableString(model.UserName));

                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                        }
                }
            }

            // If we got this far, something failed, redisplay form            
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl, string userName = "", bool isAdmin = false, bool isCandidate = false, bool isPanel = false)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (isCandidate)
                return RedirectToAction("Welcome", "Candidate");

            if (isAdmin)
                return RedirectToAction("Candidates", "Admin");

            if (isPanel)
                return RedirectToAction("Candidates", "Admin");

            return RedirectToAction("Dashboard", "User");
        }
        private string GetUserIdentifiableString(string userName)
        {
            return Session.SessionID + "-" + userName;
        }

        [AllowAnonymous]
        public async Task<ActionResult> LogOff()
        {
            var userName = "Unknown";

            if (HttpContext.User != null && !string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                userName = HttpContext.User.Identity.Name;
            }

            _logger.Log(string.Format("LogOff request received for user : {0}", userName),
                LogCategory.Information, GetUserIdentifiableString(userName));

            return await LogUserOut();
        }

        private async Task<ActionResult> LogUserOut()
        {
            Session.Abandon();
            _cookieHelper.ClearAllCookies();
            AuthenticationManager.SignOut();

            return RedirectToAction("Login");
        }

    }
}