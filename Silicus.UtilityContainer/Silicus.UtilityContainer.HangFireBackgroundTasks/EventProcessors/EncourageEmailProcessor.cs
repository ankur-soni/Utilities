using System;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.Models;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Models.Enumerations;

namespace HangFireBackgroundTasks.EventProcessors
{
    public class EncourageEmailProcessor : IEmailProcessor
    {
     //   EncourageEventProcessor eventProcessor = new EncourageEventProcessor();
        ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);

        public void Process()
        {
            throw new NotImplementedException();
        }

        public void Process(EventType eventType)
        {
            _logger.Log("EncourageEmailProcessor-Process-EventType-" + eventType);

            switch (eventType)
            {
                case EventType.SendNominationEmail:
                    SendNominationEmail();
                    break;
                case EventType.SendReviewNominationEmail:
                    SendReviewNominationEmail();
                    break;
                case EventType.SendAdminNominationEmail:
                    SendAdminNominationEmail();
                    break;
                default:
                    break;

            }
        }

        private void SendNominationEmail()
        {
            _logger.Log("EncourageEmailProcessor-SendNominationEmail");
            //eventProcessor.Process(EventType.UnLockNominations,EventProcess.UnLockEvent);
            var managerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(369);

            if (managerEmailAddresses.Count <= 0)
            {
                return;
            }

            var emailBodyPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Views/EmailBody/EmailToManagerBody.html");
            var subject = "Submit Your Nominations";
            //new EmailService().SendEmail(emailBodyPath, managerEmailAddresses, subject);
            new EmailService().SendEmail(emailBodyPath, new List<string>() { "asha.bhandare@silicus.com" }, subject);
        }

        private void SendReviewNominationEmail()
        {
           // eventProcessor.Process(EventType.UnLockReviews,EventProcess.UnLockEvent);
            var reviewerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(371);

            if (reviewerEmailAddresses.Count <= 0)
            {
                return;
            }
            var emailBodyPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Views/EmailBody/EmailBodyToReviewer.html");
            var subject = "Nominations Submitted For Your Review.";
            //new EmailService().SendEmail(emailBodyPath, reviewerEmailAddresses, subject);
            new EmailService().SendEmail(emailBodyPath, new List<string>() { "asha.bhandare@silicus.com" },subject);
        }

        private void SendAdminNominationEmail()
        {
            
            var adminEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(370);

            if(adminEmailAddresses.Count <= 0)
            {
                return;
            }

            var emailBodyPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Views/EmailBody/ReviewsLockedNotificationToAdmin.html");
            //var mappath = System.Web.HttpContext.Current.Server.MapPath("~/View/EmailBodyToReviewer.html");

            var subject = "Review Process Locked";
            // var emailBodyPath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\ReviewsLockedNotificationToAdmin.html";
            //new EmailService().SendEmail(emailBodyPath, adminEmailAddresses, subject);
            new EmailService().SendEmail(emailBodyPath, new List<string>() { "asha.bhandare@silicus.com" },subject);
        }
    }
}
