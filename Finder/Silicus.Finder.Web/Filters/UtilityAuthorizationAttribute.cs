using Silicus.Finder.Entities;
//using Silicus.Finder.IdentityWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;


namespace Silicus.Finder.Web.Filters
{
    public class UtilityAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var request = HttpContext.Current.Request;
            var authCookie = request.Cookies[".ADAuthCookie"];
            var DirectLoginInFinderCookie = request.Cookies["DirectLoginInFinderCookie"];

            if (DirectLoginInFinderCookie!=null)
            {
                return true;
            }
         
            if (authCookie== null )
            {
                return false;
            }
            else if (authCookie.Value != null)
            {
                var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var username = authenticationTicket.Name;
                var password = authenticationTicket.UserData;
                if (Membership.ValidateUser(username, password))
                {
                    var userFromAd = Membership.GetUser(username);
                    //var userManager = new UserManager();
                    //var membershipId = userManager.CreateUserIfNotExistfromActiveDirectory(userFromAd.UserName, userFromAd.Email, password);
                   // userManager.AssignRoleToUser(membershipId, "Admin");
                    return true;
                }

            }
            
           return false;
        }

    }
}