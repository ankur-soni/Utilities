using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Silicus.EncourageWithAzureAd.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public string getCurrentUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public static List<string> getCurrentUserRoles()
        {
            var authorizationService = new Authorization(new Silicus.UtilityContainer.Entities.CommonDataBaseContext("DefaultConnection"));

            string utility = WebConfigurationManager.AppSettings["ProductName"];

            var commonRoles = authorizationService.GetRoleForUtility(new MvcApplication().getCurrentUserName(), utility);
            return commonRoles;
        }


    }
}
