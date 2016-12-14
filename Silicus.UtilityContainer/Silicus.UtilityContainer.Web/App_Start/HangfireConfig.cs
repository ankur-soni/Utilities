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
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("UnLockMonthlyNomination", processor => processor.Process(EventType.UnLockNominations, EventProcess.UnLockEvent, FrequencyCode.MON),  ConfigurationManager.AppSettings["CronExpressionForNominationMailMonthly"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockMonthlyNomination", processor => processor.Process(EventType.LockNomination, EventProcess.LockEvent, FrequencyCode.MON), () => ConfigurationManager.AppSettings["CronExpressionForNominationLockMailMonthly"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockMonthlyReview", processor => processor.Process(EventType.LockReview, EventProcess.LockEvent, FrequencyCode.MON), () => ConfigurationManager.AppSettings["CronExpressionForReviewLockMailMonthly"]);

                //Adding job for yearly awards
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("UnLockYearlyNomination", processor => processor.Process(EventType.UnLockNominations, EventProcess.UnLockEvent, FrequencyCode.YEAR), ConfigurationManager.AppSettings["CronExpressionForNominationMailYearly"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockYearlyNomination", processor => processor.Process(EventType.LockNomination, EventProcess.LockEvent, FrequencyCode.YEAR), () => ConfigurationManager.AppSettings["CronExpressionForNominationLockMailYearly"]);
                RecurringJob.AddOrUpdate<EncourageEventProcessor>("LockYearlyReview", processor => processor.Process(EventType.LockReview, EventProcess.LockEvent, FrequencyCode.YEAR), () => ConfigurationManager.AppSettings["CronExpressionForReviewLockMailYearly"]);
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