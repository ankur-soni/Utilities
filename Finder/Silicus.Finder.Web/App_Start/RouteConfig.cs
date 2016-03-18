using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Routing;

namespace Silicus.Finder.Web
{
    [ExcludeFromCodeCoverage]
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
      name: "twoParametersView",
      url: "{controller}/{action}/{empId}/{projectId}",
      defaults: new
      {
          controller = "Projects",
          action = "RemoveProjectEmployee",

      });

            routes.MapRoute(
             name: "DashboardMapping",
             url: "Dashboard/{action}",
             defaults: new { controller = "Dashboard", action = "Dashboard"}
            
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
             defaults: new {controller = "Account", action = "Login", id = UrlParameter.Optional}
                //defaults: new { controller = "Employee", action = "Details", id = UrlParameter.Optional }
                );



          
        }
    }
}