using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Silicus.UtilityContainer.Web.Startup))]
namespace Silicus.UtilityContainer.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
