using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Silicus.ProjectTracker.Web.Filters;


namespace Silicus.ProjectTracker.Web
{
    [ExcludeFromCodeCoverage]
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new RdbiAuthorizationAttribute());
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomActionFilter());

        }
    }
}