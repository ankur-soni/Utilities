using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Silicus.EncourageWithAzureAd.Web.Models;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    public class AccountController : Controller
    {
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
            var protocol = Request.Url != null ? Request.Url.Scheme : null;
            string callbackUrl = Url.Action("SignOutCallback", "Account", null, protocol);
            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties {RedirectUri = callbackUrl},
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
            
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult LogoutAs()
        {
            var rootUserName = User.GetClaimValue("RootUserName");
            User.AddUpdateClaim("RootUserName", string.Empty);

            var userLastName = string.Empty;
                var  userFirstName = string.Empty;
                var nameParts = rootUserName.Split('.', '@');
            if (nameParts.Length > 0)
            {
                userFirstName = nameParts[0];
                userLastName = nameParts[1];
            }
            User.AddUpdateClaim(ClaimTypes.Name, rootUserName);
            User.AddUpdateClaim(ClaimTypes.Upn, rootUserName);
            User.AddUpdateClaim(ClaimTypes.Surname, userLastName);
            User.AddUpdateClaim(ClaimTypes.GivenName, userFirstName);
            return RedirectToAction("LoginAs", "Home");
        }

    }
}
