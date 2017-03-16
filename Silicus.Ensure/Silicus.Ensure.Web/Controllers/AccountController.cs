using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Silicus.Ensure.Models.Constants;
using System.Collections.Generic;
using System;
using Silicus.FrameWorx.Logger;

namespace Silicus.Ensure.Web.Controllers
{
    public class AccountController : Controller
    {
       
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string userName)
        {

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "CandidateAccount");
            }

            if (string.IsNullOrWhiteSpace(userName) && HttpContext.User != null && !string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                userName = HttpContext.User.Identity.Name;
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return View();
            }

            IList<string> userRoles = new List<string>();
            userRoles = MvcApplication.getCurrentUserRoles();

            if (userRoles.Count > 0)
            {
                return RedirectToLocal(returnUrl, userRoles[0]);
            }
            else
            {
                throw new Exception();
            }

            return View();
        }



        private ActionResult RedirectToLocal(string returnUrl, string role)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            RoleName rol;
            Enum.TryParse(role, out rol);
            switch (rol)
            {
                case RoleName.Candidate:
                    return RedirectToAction("Welcome", "Candidate");
                    break;
                case RoleName.Panel:
                    return RedirectToAction("Candidates", "Admin");
                    break;
                case RoleName.Admin:
                    return RedirectToAction("Candidates", "Admin");
                    break;
                case RoleName.Recruiter:
                    return RedirectToAction("Candidates", "Admin");
                    break;
                default:
                    return View();
                    break;
            }
        }






        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("Candidates", "Admin", routeValues: null, protocol: Request.Url.Scheme);
            string[] cookies = HttpContext.Request.Cookies.AllKeys;
            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            return RedirectToAction("LogOff", "CandidateAccount");
        }

    }

}
