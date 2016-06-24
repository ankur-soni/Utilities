using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.EventProcessors
{
   public class ReviewsLockedNotificationToReviewers : IEventProcessor
    {
        public void Process()
        {
            var reviewerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(367);
            var subject = "Review Process Locked";
            var emailBodyPath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\ReviewsLockedNotificationToReviewer.html";
            new EmailService().SendEmail(emailBodyPath, reviewerEmailAddresses, subject);
        }
    }
}
