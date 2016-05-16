using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.Encourage.Web.Filters
{
    public class CustomeAuthorizeAttribute:AuthorizeAttribute
    {
        

        public string AllowedRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var dataContextFactory = new DataContextFactory();
            var _commonDbService = new CommonDbService(dataContextFactory);

            var adAuthenticationCookie = httpContext.Request.Cookies[".ADAuthCookie"];

            if (adAuthenticationCookie != null)
            {
                bool status = false;
                var userRole = string.Empty;
                string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);


                if (!string.IsNullOrEmpty(HttpContext.Current.Session["Role"] as string))
                {
                    userRole = HttpContext.Current.Session["Role"].ToString();
                }
                else
                {
                    var authCookie = httpContext.Request.Cookies[".ADAuthCookie"];
                    var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    var username = authenticationTicket.Name;

                    var user = Membership.GetUser(username);
                    string utility = WebConfigurationManager.AppSettings["ProductName"];

                   
                    var _authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
                    userRole = _authorizationService.GetRoleForUtility(user.Email, utility);
                }

                if (allowedRoles.Contains(userRole))
                    status = true;
                return status;
            }


            return false;
            
        }
    }
}