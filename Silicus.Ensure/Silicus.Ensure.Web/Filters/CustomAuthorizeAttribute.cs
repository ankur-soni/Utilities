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
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Entities.Identity;

namespace Silicus.Ensure.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            bool authorize = false;

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                if (allowedroles.Contains(RoleName.Candidate.ToString()) && HttpContext.Current.User.IsInRole("Candidate"))
                {
                    authorize = true;
                }
                else
                {
                    var userRoles = MvcApplication.getCurrentUserRoles();
                    if (userRoles.Count > 0)
                    {
                        authorize = allowedroles.Intersect(userRoles).Any();
                    }
                    else
                        authorize = false;

                }
                if (!authorize)
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Error/Unauthorized.cshtml"
                    };
                }
            }
            base.OnAuthorization(filterContext);
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