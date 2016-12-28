using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Silicus.FrameWorx.Logger;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Filters;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.Constants;
using Microsoft.Owin.Security.Cookies;

namespace Silicus.Ensure.Web.Controllers
{
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl, string userName)
        {
            using (var context = _dataContextFactory.Create(ConnectionType.Ip))
            {
                // Hitting database just to let EF create it if it does not
                // exist based on initializer.
                context.Query<Organization>().Count();
            }

            if (!Request.IsAuthenticated)
            {
                _logger.Log(string.Format("Request not authenticated, showing login form."), LogCategory.Information);

                return View();
            }

            var model = new LoginModel();
            if (string.IsNullOrWhiteSpace(userName) && HttpContext.User != null && !string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                userName = HttpContext.User.Identity.Name;
            }

            _logger.Log(string.Format("Request authenticated for user : {0}", userName),
                LogCategory.Information, GetUserIdentifiableString(userName));

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.Log("UserName found null for user, showing login form.", LogCategory.Warning);

                return View();
            }

            ViewBag.UserName = userName;
            ViewBag.ReturnUrl = returnUrl;

            model.UserName = userName;
            model.Password = string.Empty;

            var identityUser = await UserManager.FindByNameAsync(model.UserName);
            var isAdmin = await UserManager.IsInRoleAsync(identityUser.Id, "Admin");
            if (identityUser == null)
            {
                _logger.Log(string.Format("MembershipUser is found null for user : {0}", userName),
                    LogCategory.Warning, GetUserIdentifiableString(userName));
                return View();
            }

            // Setting a cookie value for notification status.
            _cookieHelper.SetCookie("_notification", "false", new TimeSpan(8, 0, 0));

            var userRoles = await UserManager.GetRolesAsync(identityUser.Id);
            if (userRoles.Count > 0)
            {
                _logger.Log(string.Format("Redirecting to {1} URL for user : {0}", userName, returnUrl),
                    LogCategory.Verbose, GetUserIdentifiableString(userName));

                return RedirectToLocal(returnUrl, userName, isAdmin);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Audit]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            _logger.Log(string.Format("Login request received for user : {0}", model.UserName),
                LogCategory.Information, GetUserIdentifiableString(model.UserName));

            if (ModelState.IsValid)
            {
                _logger.Log("UserName: "+ model.UserName + "Password: " +  model.Password);
                var loginResult = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
                _logger.Log("LoginResult: "+loginResult);
                switch (loginResult)
                {
                    case SignInStatus.Success:
                        _logger.Log("Login-Post-Switch-Sucess");
                        var user = await UserManager.FindByNameAsync(model.UserName);
                        _logger.Log("User: "+user.Email);
                        var isAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");
                        _logger.Log(user.Email + " isAdmin: "+isAdmin);
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

            return await LogUserOut();
        }

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
                        //var newcode = code.Replace("+", "N"); // " ", "+");

                        var newcode = HttpUtility.UrlEncode(code);
                        SendForgotPasswordMail(identityUser.Email, identityUser.UserName, newcode);
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

            return View("ForgotPassword", forgotPassword);
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

                    model.ResetToken = HttpUtility.UrlDecode(model.ResetToken);
                    model.ResetToken = model.ResetToken.Replace(" ", "+");

                    var resultReset = await UserManager.ResetPasswordAsync(currentUser.Id, model.ResetToken, model.Password);
                    if (!resultReset.Succeeded)
                    {
                        foreach (var error in resultReset.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }

                        return View("ResetPassword", model);
                    }

                    SendResetPasswordMail(currentUser.Email, currentUser.UserName, model.Password);
                    ViewBag.IsEmailSent = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }

            return RedirectPermanent("Login/?userName=" + model.Email);
        }

        public ActionResult ChangePassword()
        {
            if (Request.IsAjaxRequest() == true)
            {
                return PartialView();
            }

            return PartialView("ChangePassword");
        }

        [HttpPost]
        [Audit]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            _logger.Log(string.Format("ChangePassword called for user Id - {0}", model.UserId), LogCategory.Information);

            string errorMsg = "";

            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;

                // ChangePassword will throw an exception rather than return
                // false in certain failure scenarios. Hence try catch...
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    changePasswordSucceeded = true;
                    errorMsg = "Password Changed Successfully.";
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                    }
                }
                else
                {
                    errorMsg = result.Errors.FirstOrDefault();
                    changePasswordSucceeded = false;
                }

                return Json(new { Validated = changePasswordSucceeded, Message = errorMsg }, JsonRequestBehavior.AllowGet);
            }

            return View();
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

        private void SendForgotPasswordMail(string email, string userName, object key)
        {
            try
            {
                string urlEncodedUserName = System.Web.HttpUtility.UrlEncode(userName); // url encoded
                string subject = ConfigurationManager.AppSettings["ProductNameLong"] + ": " + ConfigurationManager.AppSettings["SmtpMailSubjectForgotPassword"];
                string baseUrl = ConfigurationManager.AppSettings["SmtpMailbaseUrl"];

                string link = baseUrl + "/Account/ResetPassword/?username=" + urlEncodedUserName + "&reset=" + key.ToString(); //GenerateEncodedKey(userName, key.ToString());

                string body = "<html>" +
                       "<body>" +
                       "<table style='width: 590px; border: none;'><tr><td><table style='border: 1px solid #5C666F; align: left; width: 590px; font-family: arial; font-size: 14px; height: auto;border-spacing: 0;'>" +
                       "<tr style='width: 590px; height: 44px; border-bottom: 1px solid #5C666F;'>" +
                       "<td style=' background-color: #00263D; height: 44px; width: 195px; border-bottom: 5px solid #55A51C; margin: 0 auto;'><img src='" + ConfigurationManager.AppSettings["WebsiteURL"] + "/Images/rigdig_logo_email.png' style='padding: 3px; margin: 0 auto;' /></td>" +
                       "<td style='background-color: #5C666F; width: 395px; height: 44px; border-bottom: 5px solid #A3A9AC;vertical-align: middle;'>" +
                       "<p style='font-size: 19px; margin-left: 20px; color: #fff; font-weight: bold; padding: 0; width: 100%;'>" +
                       "Password Reset" +
                       "</p>" +
                       "</td>" +
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
                       "Click here!!!" +
                       "</a>" +
                       "</span> " +
                       "</li>" +
                       "<li style='list-style-type:disc; width: 470px;'>" +
                       "Username: <span style='color: #000; text-decoration: none!important; font-family: arial; font-size: 14px;'>" + userName +
                       "</span></li>" +
                       "</ul>" +
                       "<p style='font-size: 14px; color: #000;'>" +
                       "If you did not request a password reset you do not need to take any action. If you have any questions please contact your " + ConfigurationManager.AppSettings["ProductNameLong"] + " administrator." +
                       "</p>" +
                       "<p style='font-size: 14px; color: #000; margin-bottom: 10px !important;'>" +
                       "<span style='font-weight: bold;'>" + ConfigurationManager.AppSettings["ProductNameLong"] + "</span>" +
                       " 1509 Orchard Lake Drive | Charlotte, NC 28270" +
                       "<br />" +
                       "<span style='color: #000; text-decoration: none!important; font-family: arial; font-size: 14px;'>www.RigDigBI.com</span> | " +
                       "<span style='color: #000;text-decoration: none !important; font-family: arial; font-size: 14px;'>" + ConfigurationManager.AppSettings["SmtpMailSupportAddress"] +
                       "</span></p>" +
                       "<p style='text-align:right;margin-bottom: -15px; margin-right: -10px'><img src='" + ConfigurationManager.AppSettings["WebsiteURL"] + "/Images/RandallReillyProduct.png' style='padding: 2px;' /></p></td></tr>" +
                       "</table><table style='border: none; width: 590px; border-spacing: 0; align: left; margin: 0; padding: 0;'><tr><td colspan='2' style='border-top: 5px solid #00263D;'>&nbsp;</td></tr></table>" +
                       "</td></tr></table></body>" +
                       "</html>";

                _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private static string GenerateEncodedKey(string username, string guid)
        {
            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
            return HashParams;
        }

        private static string GenerateEncodedKeyNew(string username, string guid)
        {
            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + "new" + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
            return HashParams;
        }

        private void SendResetPasswordMail(string email, string userName, string Password)
        {
            try
            {
                string urlEncodedUserName = System.Web.HttpUtility.UrlEncode(userName); // url encoded
                string subject = "Reset Password";

                string body = "<html>" +
                       "<body>" +
                      "<table style='width: 590px; border: none;'><tr><td><table style='border: 1px solid #5C666F; align: left; width: 590px; font-family: arial; font-size: 14px; height: auto;border-spacing: 0;'>" +
                       "<tr style='width: 590px; height: 44px; border-bottom: 1px solid #5C666F;'>" +
                       "<td style=' background-color: #00263D; height: 44px; width: 195px; border-bottom: 5px solid #55A51C; margin: 0 auto;'></td>" +
                       "<td style='background-color: #5C666F; width: 395px; height: 44px; border-bottom: 5px solid #A3A9AC;vertical-align: middle;'>" +
                       "<p style='font-size: 19px; margin-left: 20px; color: #fff; font-weight: bold; padding: 0; width: 100%;'>" +
                       "Password Reset" +
                       "</p>" +
                       "</td>" +
                       "</tr>" +
                       "<tr>" +
                       "<td colspan='2' style='width: 590px; width: auto; border: 4px solid #D1D3D4; border-top: none; padding: 30px; margin-top: 4px;'>" +
                       "<p style='font-size: 14px; color: #000; margin-top: 20px!important;'>" +
                       "Your password has been reset successfully. Below is your new password to access the application. You will be prompted to enter a new password on your initial login." +
                       "</p>" +
                       "<ul>" +
                       "<li style='list-style-type:disc; width: 470px;'>" +
                       "User Name: " +
                       "<a href='" + userName + "' target='blank' style='color: #00698E; text-decoration: underline; width: 450px; -ms-word-wrap:break-word; word-wrap:break-word;'>" + userName +
                       "</a>" +
                       "</span> " +
                       "</li>" +
                       "<li style='list-style-type:disc; width: 470px;'>" +
                       "Reset Password: <span style='color: #000; text-decoration: none!important; font-family: arial; font-size: 14px;'>" + Password +
                       "</span></li>" +
                       "</ul>" +
                       "<p style='font-size: 14px; color: #000;'>" +
                       "This is an auto-generated email, please do not reply." +
                       "</p>" +
                       "<p style='font-size: 14px; color: #000; margin-bottom: 10px !important;'>" +
                       "<span style='font-weight: bold;'>" + "Regards," +
                       "<br />" +
                       "<span style='color: #000; text-decoration: none!important; font-family: arial; font-size: 14px;'>Ensure IT Support " +
                       "<span style='color: #000;text-decoration: none !important; font-family: arial; font-size: 14px;'>" +
                       "</span></p>" +
                        "<p style='text-align:right;margin-bottom: -15px; margin-right: -10px'></p></td></tr>" +
                       "</table><table style='border: none; width: 590px; border-spacing: 0; align: left; margin: 0; padding: 0;'><tr><td colspan='2' style='border-top: 5px solid #00263D;'>&nbsp;</td></tr></table>" +
                       "</td></tr></table></body>" +
                       "</html>";

                _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
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

        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/Account/SignIn" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);
            string[] cookies = HttpContext.Request.Cookies.AllKeys;
            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
