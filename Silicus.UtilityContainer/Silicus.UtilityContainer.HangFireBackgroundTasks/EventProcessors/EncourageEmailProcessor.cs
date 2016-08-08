using System;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.Models;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System.Collections.Generic;

namespace HangFireBackgroundTasks.EventProcessors
{
    public class EncourageEmailProcessor : IEmailProcessor
    {
        public void Process()
        {
            throw new NotImplementedException();
        }

        public void Process(EventType eventType)
        {
            switch(eventType)
            {
                case EventType.SendNominationEmail:
                    //SendNominationEmail();
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
            var managerEmailAddresses = new List<string>(); //GetEmailAddress.GetEmailAddressForRoles(364);
            managerEmailAddresses.Add("Indrajit.kadam@silicus.com");
            var subject = "Submit Your Nominations";
            var emailBodyPath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\EmailToManagerBody.html";
            //var emailBodyPath = @"C:\Projects\Silicus-Online\Utilities\Utilities\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\EmailToManagerBody.html";
            //var emailBodyPath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\EmailToManagerBody.html";
            new EmailService().SendEmail(emailBodyPath, managerEmailAddresses, subject);
        }

        private void SendReviewNominationEmail()
        {
            var reviewerEmailAddresses =  GetEmailAddress.GetEmailAddressForRoles(367);
            //reviewerEmailAddresses.Add("indrajit.kadam@silicus.com");
            var subject = "Nominations Submitted For Your Review.";
            var emailBodyPath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\EmailBodyToReviewer.html";
            new EmailService().SendEmail(emailBodyPath, reviewerEmailAddresses, subject);
        }
        private void SendAdminNominationEmail()
        {
            var adminEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(366);
            var subject = "Review Process Locked";
            //var emailBodyPath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\ReviewsLockedNotificationToAdmin.html";
            var emailBodyPath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\ReviewsLockedNotificationToAdmin.html";
            new EmailService().SendEmail(emailBodyPath, adminEmailAddresses, subject);
        }
    }
}
