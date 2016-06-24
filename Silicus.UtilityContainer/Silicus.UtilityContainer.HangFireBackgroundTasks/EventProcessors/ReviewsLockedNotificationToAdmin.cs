using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.EventProcessors
{
    public class ReviewsLockedNotificationToAdmin:IEventProcessor
    {
        public void Process()
        {
            var adminEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(366);
            var subject = "Review Process Locked";
            var emailBodyPath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\ReviewsLockedNotificationToAdmin.html";
            new EmailService().SendEmail(emailBodyPath, adminEmailAddresses, subject);
        }
    }
}
