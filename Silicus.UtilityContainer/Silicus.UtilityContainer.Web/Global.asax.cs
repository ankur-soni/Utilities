using Hangfire;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Web.App_Start;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Silicus.UtilityContainer.Services;
using Silicus.FrameWorx.Logger;

namespace Silicus.UtilityContainer.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LightInjectWebCommon.CreateContainer();

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
            _logger.Log("Application_error-" + exception);
            Server.ClearError();
            Response.Redirect("/Home/Error");
        }
        public static List<SuperUser> GetAllSuperUsers()
        {
            var superUserService = new SuperUserService(new CommonDataBaseContext("DefaultConnection"));
            var superUsers = superUserService.GetAllSuperUsers();
            return superUsers;
        }
        //public static void KeepApplicationRunning()
        //{
        // new MvcApplication().Application_Start();
        //}
    }
}
