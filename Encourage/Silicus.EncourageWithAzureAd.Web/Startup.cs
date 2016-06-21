using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using Silicus.EncourageWithAzureAd.Web.App_Start;

namespace Silicus.EncourageWithAzureAd.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //GlobalConfiguration.Configuration.UseSqlServerStorage(@"Data Source=SILICUS505\SQLEXPRESS;Integrated Security=True;");
            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
            //HangfireConfig.StartBackgroundScheduling();
        }
    }
}
