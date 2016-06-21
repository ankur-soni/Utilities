using Hangfire;
using Silicus.EncourageWithAzureAd.Web.Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.App_Start
{
    public class HangfireConfig
    {
        public static void StartBackgroundScheduling()
        {
            RecurringJob.AddOrUpdate<Mailer>(mailer=>mailer.sendMail(), "0 0 1 * *");  //CRON expression that Run once a month at midnight of the first day of the month 
        }
    }
}