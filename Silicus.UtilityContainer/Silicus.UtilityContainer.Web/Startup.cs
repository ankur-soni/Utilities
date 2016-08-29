using Owin;
using Hangfire;
using Silicus.UtilityContainer.Web.App_Start;
using System.Configuration;


namespace Silicus.UtilityContainer.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            ConfigureAuth(app);
            var connectionString = ConfigurationManager.ConnectionStrings["HangfireDbConnection"].ConnectionString;
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
            //GlobalConfiguration.Configuration.UseSqlServerStorage(@"Data Source=10.4.1.190\SQLExpress1;User Id=SilUtilSqlUser;Password=Pa55w0rd");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            HangfireConfig.StartBackgroundScheduling();
        }
    }
}
