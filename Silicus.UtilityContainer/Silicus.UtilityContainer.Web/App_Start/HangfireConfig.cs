using Hangfire;
using HangFireBackgroundTasks.EventProcessors;
using System;
using Silicus.UtilityContainer.Models;
using System.Configuration;
using Silicus.FrameWorx.Logger;
using System.Threading;
using Silicus.UtilityContainer.Models.Enumerations;
using HangFireBackgroundTasks.Enums;

namespace Silicus.UtilityContainer.Web.App_Start
{
    public static class HangfireConfig
    {
        public static void StartBackgroundScheduling()
        {
            ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
            _logger.Log("HangfirConfig-StartBackgroundScheduling");
            try
            {
                _logger.Log("HangfirConfig-StartBackgroundScheduling-try");

                //Adding job for monthly awards
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("UnLockNomination", processor => processor.Process(EventType.UnLockNominations, EventProcess.UnLockEvent, FrequencyCode.MON),  ConfigurationManager.AppSettings["CronExpressionForNominationMailMonthly"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockNomination", processor => processor.Process(EventType.LockNomination, EventProcess.LockEvent, FrequencyCode.MON), () => ConfigurationManager.AppSettings["CronExpressionForNominationLockMail"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockReview", processor => processor.Process(EventType.LockReview, EventProcess.LockEvent, FrequencyCode.MON), () => ConfigurationManager.AppSettings["CronExpressionForReviewLockMail"]);

                //Adding job for yearly awards
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("UnLockNomination", processor => processor.Process(EventType.UnLockNominations, EventProcess.UnLockEvent, FrequencyCode.YEAR), ConfigurationManager.AppSettings["CronExpressionForNominationMail"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockNomination", processor => processor.Process(EventType.LockNomination, EventProcess.LockEvent, FrequencyCode.YEAR), () => ConfigurationManager.AppSettings["CronExpressionForNominationLockMail"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockReview", processor => processor.Process(EventType.LockReview, EventProcess.LockEvent, FrequencyCode.YEAR), () => ConfigurationManager.AppSettings["CronExpressionForReviewLockMail"]);
                _logger.Log("HangfirConfig-StartBackgroundScheduling-done");

            }
            catch (Exception ex)
            {
                _logger.Log("HangfirConfig-StartBackgroundScheduling-catch-"+ex.Message);
                throw ex;
            }
        }
    }
}