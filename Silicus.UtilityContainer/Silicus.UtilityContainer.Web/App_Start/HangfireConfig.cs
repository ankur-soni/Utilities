using Hangfire;
using Silicus.UtilityContainer.HangFireBackgroundTasks.EventProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.UtilityContainer.Web.App_Start
{
    public static class HangfireConfig
    {
        public static void StartBackgroundScheduling()
        {
            //RecurringJob.AddOrUpdate<SendNominationEventProcessor>(mailProcessor => mailProcessor.Process(), "0 0 1 * *");  //CRON expression that Run once a month at midnight of the first day of the month 

            RecurringJob.AddOrUpdate<SendNominationEventProcessor>(mailProcessor => mailProcessor.Process(), "40 7 23 6 *");
            RecurringJob.AddOrUpdate<ReviewNominationEventProcessor>(mailProcessor => mailProcessor.Process(), "45 7 23 6 *");
            RecurringJob.AddOrUpdate<ReviewsLockedNotificationToReviewers>(mailProcessor => mailProcessor.Process(), "50 7 23 6 *");
            RecurringJob.AddOrUpdate<ReviewsLockedNotificationToAdmin>(mailProcessor => mailProcessor.Process(), "50 7 23 6 *");
            RecurringJob.AddOrUpdate<SendNotificationAfterWinnerSelected>(mailProcessor => mailProcessor.Process(), "58 7 23 6 *");

        }
    }
}