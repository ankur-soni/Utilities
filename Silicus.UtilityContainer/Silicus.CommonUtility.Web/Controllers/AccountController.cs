using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Silicus.UtilityContainer.Web.Models;
using System.Web.Security;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Security;
using Silicus.UtilityContainer.Services.Interfaces;
using System.Configuration;

namespace Silicus.UtilityContainer.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAuthentication _userAuthentication;    
        private IUserService _userService;

        public AccountController(IAuthentication userAuthentication, IUserService userService)
        {
            _userAuthentication = userAuthentication;
            _userService = userService;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (_userAuthentication.ValidateUser(model.UserName, model.Password))
            {
                var cookie = FormsAuthentication.GetAuthCookie(model.UserName, model.RememberMe);
                var authenticationTicket = FormsAuthentication.Decrypt(cookie.Value);

                var newAuthenticationTicket = new FormsAuthenticationTicket(
                              authenticationTicket.Version,
                              model.UserName,
                             DateTime.Now,
                              new DateTime(2016, 3, 30),
                              true,
                              userData: model.Password);

                var encryptedTicket = FormsAuthentication.Encrypt(newAuthenticationTicket);
                cookie.Value = encryptedTicket;

                Response.Cookies.Add(cookie);
                Session["CurrentUser"] = model.UserName;

                var ADUser = Membership.GetUser(model.UserName);
                var checkFirstLogin = _userService.CheckForFirstLoginByEmail(ADUser.Email);
               
                if(checkFirstLogin)
                {
                    var newUser = _userService.FindUserByEmail(ADUser.Email);
                    _userService.AddRoleToUserForAllUtility(newUser);
                }

                if (returnUrl != null)
                {
                    return Redirect(ConfigurationManager.AppSettings["Finder"] + "?returnUrl=" + returnUrl);
                }
                return this.RedirectToAction("Index", "Home");
            }
            this.ModelState.AddModelError(string.Empty, "The user name or password is incorrect.");

            return this.View(model);
        }

        //POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var cookies = HttpContext.Request.Cookies.AllKeys;
            foreach (var cookie in cookies)
            {
                HttpContext.Response.Cookies[cookie].Expires = DateTime.Now;
            }      
            return RedirectToAction("Login", "Account");
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}