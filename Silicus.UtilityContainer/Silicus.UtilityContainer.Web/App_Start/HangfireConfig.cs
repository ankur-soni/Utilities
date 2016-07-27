using Hangfire;
using HangFireBackgroundTasks.EventProcessors;
using System;
using Silicus.UtilityContainer.Models;

namespace Silicus.UtilityContainer.Web.App_Start
{
    public static class HangfireConfig
    {
        public static void StartBackgroundScheduling()
        {
            try
            {
                //RecurringJob.AddOrUpdate<EncourageEmailProcessor>(eventProcessor => eventProcessor.Process(EventType.SendNominationEmail), Cron.Minutely);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>(processor => processor.Process(EventType.LockNomination), "0 0 / 5 * ***");

                //RecurringJob.AddOrUpdate<SendNominationEventProcessor>(mailProcessor => mailProcessor.Process(), Cron.Minutely);  //CRON expression that Run once a month at midnight of the first day of the month 

            }
            catch (Exception ex)
            {
                throw ex;
            }
           // RecurringJob.AddOrUpdate<SendNominationEventProcessor>(mailProcessor => mailProcessor.Process(), "0 0 1 * *");  //CRON expression that Run once a month at midnight of the first day of the month 

            //RecurringJob.AddOrUpdate<SendNominationEventProcessor>(mailProcessor => mailProcessor.Process(),"40 7 23 6 *");
            //RecurringJob.AddOrUpdate<ReviewNominationEventProcessor>(mailProcessor => mailProcessor.Process(), "45 7 23 6 *");
            //
            //RecurringJob.AddOrUpdate<ReviewsLockedNotificationToAdmin>(mailProcessor => mailProcessor.Process(), "50 7 23 6 *");
            //RecurringJob.AddOrUpdate<SendNotificationAfterWinnerSelected>(mailProcessor => mailProcessor.Process(), "58 7 23 6 *");
        }
    }
}