using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Finder.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string AllowedRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool status = false;
            string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var userRole = HttpContext.Current.Session["Role"].ToString();
            if (allowedRoles.Contains(userRole))
                status = true;
            
            return status;
        }
    }
}