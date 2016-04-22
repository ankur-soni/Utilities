using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Silicus.FrameWorx.Logger;
using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.Filters;
using Silicus.Finder.Web.Models;
using Silicus.Finder.IdentityWrapper;
using Silicus.Finder.IdentityWrapper.Models;
using System.Web.Security;
using System.Security.Principal;

namespace Silicus.Finder.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ICookieHelper _cookieHelper;
        private readonly ILogger _logger;
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEmailService _emailService;

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

        public AccountController(ICookieHelper cookieHelper, ILogger logger,
            IDataContextFactory dataContextFactory, IEmailService emailService)
        {
            _cookieHelper = cookieHelper;
            _logger = logger;
            _dataContextFactory = dataContextFactory;
            _emailService = emailService;
        }

        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Login(string returnUrl)
        {
            using (var context = _dataContextFactory.Create(ConnectionType.Ip))
            {
                // Hitting database just to let EF create it if it does not
                // exist based on initializer.
                context.Query<Organization>().Count();
            }
            var cookieName = FormsAuthentication.FormsCookieName;

            var authCookie = Request.Cookies[".ADAuthCookie"];

            if (authCookie == null)
            {
                // cookie to check if user logins directly in finder
                HttpCookie DirectLoginInFinderCookie = new HttpCookie("DirectLoginInFinderCookie");
                DirectLoginInFinderCookie.Value = "abcd";
                Response.Cookies.Add(DirectLoginInFinderCookie);
                return Redirect("http://localhost:52250/?returnUrl=http://localhost:53393/" + returnUrl);
            }

            if (authCookie.Value != null)
            {
                var model = new LoginModel();
                var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var username = authenticationTicket.Name;
                var password = authenticationTicket.UserData;
              
                if (!Membership.ValidateUser(username, password))
                {
                    ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                    return View(model);
                }

                if (ModelState.IsValid)
                {

                    var userFromAd = Membership.GetUser(username);
                    var userManager = new UserManager();
                    var membershipId = userManager.CreateUserIfNotExistfromActiveDirectory(userFromAd.UserName, userFromAd.Email, password);
                    userManager.AssignRoleToUser(membershipId, "User");

                    var loginResult = await SignInManager.PasswordSignInAsync(username, password, false, shouldLockout: true);
                    switch (loginResult)
                    {
                        case SignInStatus.Success:
                            var user = await UserManager.FindByNameAsync(username);
                            var isAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");

                            //GetLoggedUserRole(user);
                            return RedirectToLocal(returnUrl, username, isAdmin);
                        case SignInStatus.LockedOut:
                            ModelState.AddModelError("", "Your account has been locked. Please contact system Administrator");
                            return View(model);
                        case SignInStatus.RequiresVerification:
                            ModelState.AddModelError("", "Account verification is pending. Please verify your account.");
                            return View(model);
                        case SignInStatus.Failure:
                            var userName = UserManager.FindByName(model.UserName);
                            if (userName != null)
                            {
                                string message = "The password does not matches with your username, please enter the correct one.";
                                var loginUser = await UserManager.FindByNameAsync(model.UserName);

                                string role = UserManager.GetRoles(loginUser.Id).FirstOrDefault();

                                if (loginUser.AccessFailedCount == 3 && role != "Admin")
                                {
                                    UserManager.SetLockoutEnabled(loginUser.Id, true);
                                    UserManager.SetLockoutEndDate(loginUser.Id, DateTimeOffset.MaxValue);
                                    message = "Your account has been locked. Please contact system Administrator";
                                }
                                ModelState.AddModelError("", message);
                                return View(model);
                            }
                            ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                            return View(model);
                        default:
                            ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                            return View(model);
                    }
                }

                // If we got this far, something failed, redisplay form

                return View(model);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        //{

        //    //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }


        //    if (Membership.ValidateUser(model.UserName, model.Password))
        //    {

        //        var cookie = FormsAuthentication.GetAuthCookie(model.UserName, model.RememberMe);

        //        //cookie to check if user logins directly in finder
        //        HttpCookie DirectLoginInFinderCookie = new HttpCookie("DirectLoginInFinderCookie");
        //        DirectLoginInFinderCookie.Value ="abcd";
        //        Response.Cookies.Add(DirectLoginInFinderCookie);

        //        // Gets an authentication ticket with the appropriate default and configured values.  
        //        var ticket = FormsAuthentication.Decrypt(cookie.Value);
        //        var newTicket = new FormsAuthenticationTicket(
        //                      ticket.Version,
        //                      model.UserName,
        //                     DateTime.Now,
        //                      new DateTime(2016, 3, 30),
        //                      true, userData: model.Password);

        //        var encryptedTicket = FormsAuthentication.Encrypt(newTicket);
        //        cookie.Value = encryptedTicket;
        //        Response.Cookies.Add(cookie);
        //        //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
        //        Session.Add("currentUser", model.UserName);

        //        return this.RedirectToAction("Dashboard", "Dashboard");
        //    }

        //    this.ModelState.AddModelError(string.Empty, "The user name or password is incorrect.");

        //    return this.View(model);

        //}



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Audit]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
           _logger.Log(string.Format("Login request received for user : {0}", model.UserName),
                LogCategory.Information, GetUserIdentifiableString(model.UserName));
            if (!Membership.ValidateUser(model.UserName, model.Password))
            {
                ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                return View(model);
            }
            if (ModelState.IsValid)
            {

                var userFromAd = Membership.GetUser(model.UserName);
                var userManager = new UserManager();
                var membershipId = userManager.CreateUserIfNotExistfromActiveDirectory(userFromAd.UserName, userFromAd.Email, model.Password);
                userManager.AssignRoleToUser(membershipId, "Admin");
                var loginResult = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
                switch (loginResult)
                {


                    case SignInStatus.Success:
                        var user = await UserManager.FindByNameAsync(model.UserName);
                        var isAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");

                        //GetLoggedUserRole(user);
                        return RedirectToLocal(returnUrl, model.UserName, isAdmin);
                    case SignInStatus.LockedOut:
                        ModelState.AddModelError("", "Your account has been locked. Please contact system Administrator");
                        return View(model);
                    case SignInStatus.RequiresVerification:
                        ModelState.AddModelError("", "Account verification is pending. Please verify your account.");
                        return View(model);
                    case SignInStatus.Failure:
                        var userName = UserManager.FindByName(model.UserName);
                        if (userName != null)
                        {
                            string message = "The password does not matches with your username, please enter the correct one.";
                            var loginUser = await UserManager.FindByNameAsync(model.UserName);

                            string role = UserManager.GetRoles(loginUser.Id).FirstOrDefault();

                            if (loginUser.AccessFailedCount == 3 && role != "Admin")
                            {
                                UserManager.SetLockoutEnabled(loginUser.Id, true);
                                UserManager.SetLockoutEndDate(loginUser.Id, DateTimeOffset.MaxValue);
                                message = "Your account has been locked. Please contact system Administrator";
                            }
                            ModelState.AddModelError("", message);
                            return View(model);
                        }
                        ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                        return View(model);
                    default:
                        ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                        return View(model);
                }
            }

            // If we got this far, something failed, redisplay form

            return View(model);
        }

        [AllowAnonymous]
        [Audit]
        public async Task<ActionResult> LogOff()
        {
            var userName = "Unknown";

            if (HttpContext.User != null && !string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                userName = HttpContext.User.Identity.Name;
            }

            _logger.Log(string.Format("LogOff request received for user : {0}", userName),
                LogCategory.Information, GetUserIdentifiableString(userName));
            var stat = Request.IsAuthenticated;

            return await LogUserOut();
        }

        //public string GetLoggedUserRole(ApplicationUser loginUser)
        //{
        //    string role = UserManager.GetRoles(loginUser.Id).FirstOrDefault();
        //    Session["LoggedUserRole"] = role;
        //    return role;
        //}

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            Session.Abandon();
            _cookieHelper.ClearAllCookies();
            AuthenticationManager.SignOut();

            return View("ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [Audit]
        public async Task<ActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            _logger.Log(string.Format("ForgotPassword request received for user : {0}", forgotPassword.Email),
                    LogCategory.Information, GetUserIdentifiableString(forgotPassword.Email));

            ViewBag.IsEmailSent = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var identityUser = UserManager.FindByEmail(forgotPassword.Email);

                    if (identityUser != null)
                    {
                        var code = await UserManager.GeneratePasswordResetTokenAsync(identityUser.Id);
                        SendForgotPasswordMail(identityUser.Email, identityUser.UserName, code);
                        ViewBag.IsEmailSent = true;

                        _logger.Log(string.Format("ForgotPassword request processed successfully for user : {0}", forgotPassword.Email),
                            LogCategory.Information, GetUserIdentifiableString(forgotPassword.Email));
                    }
                    else
                    {
                        ModelState.AddModelError("", "The email address provided is not registered.");
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsEmailSent = false;
                _logger.Log(ex);
                ModelState.AddModelError("", "Sorry! Cannot send email right now. Please try again later.");
            }

            return View("Forgot_Password", forgotPassword);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string username, string reset)
        {
            try
            {
                Session.Abandon();
                _cookieHelper.ClearAllCookies();
                AuthenticationManager.SignOut();
                if ((reset != null) && (username != null))
                {
                    var currentUser = await UserManager.FindByNameAsync(username);
                    if (currentUser != null)
                    {
                        var model = new ResetPasswordModel() { Email = username, UserType = "Existing", ResetToken = reset };
                        return View("ResetPassword", model);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: Account/SetupPassword exception - {0}", ex);
                throw;
            }

            return RedirectToAction("BadRequest", "Error");
        }

        [HttpPost, ActionName("ResetPassword")]
        [AllowAnonymous]
        [Audit]
        public async Task<ActionResult> ResetPasswordConfirmed(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.Log(string.Format("ResetPassword request received for user : {0}", model.Email),
                LogCategory.Information, GetUserIdentifiableString(model.Email));

            try
            {
                var currentUser = await UserManager.FindByEmailAsync(model.Email);
                if (currentUser != null)
                {
                    var resultlockedout = await UserManager.IsLockedOutAsync(currentUser.Id);
                    if (resultlockedout)
                    {
                        var result = await UserManager.SetLockoutEnabledAsync(currentUser.Id, false);
                        if (result.Succeeded)
                        {
                            await UserManager.ResetAccessFailedCountAsync(currentUser.Id);
                        }
                    }

                    var token = model.ResetToken.Replace(" ", "+");
                    var resultReset = await UserManager.ResetPasswordAsync(currentUser.Id, token, model.Password);
                    if (!resultReset.Succeeded)
                    {
                        foreach (var error in resultReset.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        return View("ResetPassword", model);
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }

            return RedirectPermanent("Login/?userName=" + model.Email);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordModel model)
        {

            if (ModelState.IsValid)
            {


                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

                foreach (var error in result.Errors)
                {
                    if (error.StartsWith("Incorrect"))
                    {

                        ModelState.AddModelError("", "Incorrect Current Password");
                    }
                }




                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }


                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);

                }


                var errorQuery = from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage;

                return Json(new { Result = "Failed", Message = errorQuery.ToList() }, JsonRequestBehavior.AllowGet);

            }



            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            // return errorList;



            return Json(new { Result = "Failed", Message = errorList }, JsonRequestBehavior.AllowGet);
        }


        private ActionResult RedirectToLocal(string returnUrl, string userName = "", bool isAdmin = false)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        private void SendForgotPasswordMail(string email, string userName, object key)
        {
            try
            {
                string urlEncodedUserName = System.Web.HttpUtility.UrlEncode(userName); // url encoded
                string subject = ConfigurationManager.AppSettings["ProductNameLong"] + ": " + ConfigurationManager.AppSettings["SmtpMailSubjectForgotPassword"];
                string baseUrl = ConfigurationManager.AppSettings["SmtpMailbaseUrl"];

                string link = baseUrl + "/Account/ResetPassword/?username=" + urlEncodedUserName +
                    "&reset=" + key.ToString();

                string body = "<html>" +
                       "<body>" +
                       "<table style='width: 590px; border: none;'><tr><td><table style='border: 1px solid :#75A8B9; align: left; width: 1000px; font-family: arial; font-size: 14px; height: auto;border-spacing: 0;'>" +
                       "<tr style='width: 590px; height: 44px; border-bottom: 1px solid  #75A8B9;'>" +
                       "<td style=' background-color:#75A8B9; height: 44px; width: 650px; border-bottom: 5px solid :#75A8B9; margin: 0 auto;'>" + "<p style='font-size: 19px; margin-left: 20px; color: #fff; font-weight: bold; padding: 0; width: 100%;'>" +
                       "Password Reset" +
                       "</p>" + "</td>" +

                       "</tr>" +
                       "<tr>" +
                       "<td colspan='2' style='width: 590px; width: auto; border: 4px solid #D1D3D4; border-top: none; padding: 30px; margin-top: 4px;'>" +
                       "<p style='font-size: 14px; color: #000; margin-top: 20px!important;'>" +
                       "Please click the following link to reset your password." +
                       "</p>" +
                       "<ul>" +
                       "<li style='list-style-type:disc; width: 470px;'>" +
                       "Click to set your Password: " +
                       "<a href='" + link + "' target='blank' style='color: #00698E; text-decoration: underline; width: 450px; -ms-word-wrap:break-word; word-wrap:break-word;'>" +
                       link +
                       "</a>" +
                       "</span> " +
                       "</li>" +
                       "<li style='list-style-type:disc; width: 470px;'>" +
                       "Username: <span style='color: #000; text-decoration: none!important; font-family: arial; font-size: 14px;'>" + userName +
                       "</span></li>" +
                       "</ul>" +
                       "<p style='font-size: 14px; color: #000;'>" +
                       "If you did not request a password reset you do not need to take any action. If you have any questions please contact administrator." +
                       "<p style='text-align:right;margin-bottom: -15px; margin-right: -10px'></p></td></tr>" +
                       "</table><table style='border: none; width: 590px; border-spacing: 0; align: left; margin: 0; padding: 0;'></table>" +
                       "</td></tr></table></body>" +
                       "</html>";

                _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private static string GenerateEncodedKeyNew(string username, string guid)
        {
            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + "new" + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
            return HashParams;
        }

        private async Task<ActionResult> LogUserOut()
        {
            Session.Abandon();
            _cookieHelper.ClearAllCookies();

            try
            {
                var identityUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                identityUser.LastActivityDate = DateTime.UtcNow;
                await UserManager.UpdateAsync(identityUser);

                AuthenticationManager.SignOut();
            }
            catch
            {
                AuthenticationManager.SignOut();
            }
            var stat = Request.IsAuthenticated;
            return RedirectToAction("Login", "Account");
        }

        private string GetUserIdentifiableString(string userName)
        {
            return Session.SessionID + "-" + userName;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Login");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private const string XsrfKey = "XsrfId";

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
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
