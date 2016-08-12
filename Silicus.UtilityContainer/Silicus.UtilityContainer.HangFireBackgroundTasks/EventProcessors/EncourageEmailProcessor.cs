using System;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.Models;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System.Collections.Generic;
using System.Web;
using System.IO;

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
            //Role Id - 369 - Manager
            var managerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(369);

            if (managerEmailAddresses.Count <= 0)
            {
                return;
            }

            var emailBodyPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Views/EmailBody/EmailToManagerBody.html");
            var subject = "Submit Your Nominations";
            //var emailBodyPath =   @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\EmailToManagerBody.html";
            //var emailBodyPath =     @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.Web\Views\EmailBody\EmailToManagerBody.html";
            new EmailService().SendEmail(emailBodyPath, managerEmailAddresses, subject);
        }

        private void SendReviewNominationEmail()
        {
           
            var reviewerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(371);

            if (reviewerEmailAddresses.Count <= 0)
            {
                return;
            }
            var emailBodyPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Views/EmailBody/EmailBodyToReviewer.html");
            var subject = "Nominations Submitted For Your Review.";
           // var emailBodyPath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\EmailBodyToReviewer.html";
            new EmailService().SendEmail(emailBodyPath, reviewerEmailAddresses, subject);
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
            new EmailService().SendEmail(emailBodyPath, adminEmailAddresses, subject);
        }
    }
}
