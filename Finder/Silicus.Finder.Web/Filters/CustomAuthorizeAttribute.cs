
using Silicus.Finder.ModelMappingService;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

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
            var adAuthenticationCookie = httpContext.Request.Cookies[".ADAuthCookie"];

            if (adAuthenticationCookie != null)
            {
                bool status = false;
                var userRole = new List<string>();
                string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

               
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["Role"] as string))
                {
                    userRole.Add(HttpContext.Current.Session["Role"].ToString());
                }
                else
                {
                    var authCookie = httpContext.Request.Cookies[".ADAuthCookie"];
                    var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    var username = authenticationTicket.Name;

                    var user = Membership.GetUser(username);
                    string utility = WebConfigurationManager.AppSettings["ProductName"];

                    var _commonMapper = new CommonMapper();
                    var _authorizationService = new Authorization(_commonMapper.GetCommonDataBAseContext());
                    userRole = _authorizationService.GetRoleForUtility(user.Email, utility);
                }
                foreach (var role in userRole)
                {
                    if (allowedRoles.Contains(role))
                        status = true;
                }
                
                return status;
            }


            return false;
            
        }
    }
}