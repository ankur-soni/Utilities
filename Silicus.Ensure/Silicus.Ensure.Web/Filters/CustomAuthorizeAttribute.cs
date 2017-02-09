using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using Silicus.UtilityContainer.Entities;
using System.Configuration;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool authorize = false;

            if (allowedroles.Contains(RoleName.Candidate.ToString()))
            {
                var commondbContext = new SilicusIpDataContext(ConfigurationManager.ConnectionStrings["SilicusIpDataContext"].ConnectionString);
                var users = commondbContext.Query<User>().ToList();
                var loggedInUser = HttpContext.Current.User.Identity.Name; 
                authorize = users.Find(x => x.Email.ToLower() == loggedInUser.ToLower()) != null;
            }
            else
            {
                var userRoles = MvcApplication.getCurrentUserRoles();
                if(userRoles.Count>0)
                {
                    authorize = allowedroles.Intersect(userRoles).Any();
                }
            }
           
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error/Unauthorized.cshtml"
            };
        }
    }
}