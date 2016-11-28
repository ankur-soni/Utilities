using Silicus.FrameworxProject.Services;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Silicus.FrameworxProject.Web.Filters
{
    public class CustomeAuthorizeAttribute : AuthorizeAttribute
    {


        public string AllowedRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var dataContextFactory = new DataContextFactory();
            var _commonDbService = new CommonDbService(dataContextFactory);

            var userRoles = new List<string>();
            string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string utility = WebConfigurationManager.AppSettings["ProductName"];

            var _authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
            userRoles = _authorizationService.GetRoleForUtility(HttpContext.Current.User.Identity.Name, utility);

            foreach (var allowedRole in allowedRoles)
            {
                if (!userRoles.Contains(allowedRole))
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/CustomErrorMessage.cshtml"
                    };
                }

            }


        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var dataContextFactory = new DataContextFactory();
            var _commonDbService = new CommonDbService(dataContextFactory);
            bool status = false;
            var userRoles = new List<string>();
            string[] allowedRoles = AllowedRole.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string utility = WebConfigurationManager.AppSettings["ProductName"];
            var _authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
            userRoles = _authorizationService.GetRoleForUtility(httpContext.User.Identity.Name, utility);

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