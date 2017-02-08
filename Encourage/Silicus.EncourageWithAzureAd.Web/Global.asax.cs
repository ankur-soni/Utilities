using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
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
            AttributeRoutingConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
            _logger.Log("Application_error-"+exception);
            Server.ClearError();
            Response.Redirect("/Home/Error");
        }

        public string GetCurrentUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public static List<string> GetCurrentUserRoles()
        {
            var authorizationService = new Authorization(new Silicus.UtilityContainer.Entities.CommonDataBaseContext("DefaultConnection"));
            string utility = WebConfigurationManager.AppSettings["ProductName"];
            var commonRoles = authorizationService.GetRoleForUtility(new MvcApplication().GetCurrentUserName(), utility);
            return commonRoles;
        }

        public static List<string> GetDevelopersName()
        {
            var authorizationService = new Authorization(new Silicus.UtilityContainer.Entities.CommonDataBaseContext("DefaultConnection"));
            var developers = authorizationService.GetNameOfContributors();
            return developers;
        }


    }
}
