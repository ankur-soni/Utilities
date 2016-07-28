using System;
using System.Threading.Tasks;
using Microsoft.Owin;
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

            //GlobalConfiguration.Configuration.UseSqlServerStorage(@"Server = tcp:utilitycontainerdbserver.database.windows.net,1433; Data Source = utilitycontainerdbserver.database.windows.net; Persist Security Info = False; User ID = SilUtilSqlUser; Password = Pa55w0rd; Pooling = False; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");


            //Hangfire Configuration
            var connectionString = ConfigurationManager.ConnectionStrings["HangfireDbConnection"].ConnectionString;
            GlobalConfiguration.Configuration.UseSqlServerStorage(@"Data Source=10.4.1.190\SQLExpress1;User Id=SilUtilSqlUser;Password=Pa55w0rd");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            HangfireConfig.StartBackgroundScheduling();
        }
    }
}
