using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.UtilityContainer.Web.Filters
{
    public class CustomeAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies[".ADAuthCookie"];
            if (cookie != null)
            {
                var autheticationTicket = FormsAuthentication.Decrypt(cookie.Value);

                if (Membership.ValidateUser(autheticationTicket.Name, autheticationTicket.UserData))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}