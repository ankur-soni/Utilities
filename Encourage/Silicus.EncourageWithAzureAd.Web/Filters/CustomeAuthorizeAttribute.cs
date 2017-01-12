using Silicus.Encourage.Services;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Silicus.Encourage.Web.Filters
{
    public class CustomeAuthorizeAttribute : AuthorizeAttribute
    {


        public string AllowedRole { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var dataContextFactory = new DataContextFactory();
            var _commonDbService = new CommonDbService(dataContextFactory);
            
            string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string utility = WebConfigurationManager.AppSettings["ProductName"];

            var _authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
            var userRoles = _authorizationService.GetRoleForUtility(HttpContext.Current.User.Identity.Name, utility);

            foreach (var allowedRole in allowedRoles)
            {
                if (!userRoles.Contains(allowedRole))
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/Error.cshtml"
                    };
                }

            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var dataContextFactory = new DataContextFactory();
            var _commonDbService = new CommonDbService(dataContextFactory);
            bool status = false;
            string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string utility = WebConfigurationManager.AppSettings["ProductName"];
            var _authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
            var userRoles = _authorizationService.GetRoleForUtility(httpContext.User.Identity.Name, utility);

            foreach (var allowedRole in allowedRoles)
            {
                if (userRoles.Contains(allowedRole))
                {
                    return true;
                }
                else
                {
                    status = false;
                }

            }

            return status;

        }
    }
}