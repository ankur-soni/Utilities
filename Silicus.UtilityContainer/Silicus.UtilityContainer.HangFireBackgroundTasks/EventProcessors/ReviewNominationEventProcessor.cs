using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.EventProcessors
{
    public class ReviewNominationEventProcessor //: IEventProcessor
    {
        public void Process()
        {
            var reviewerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(367);
            var subject = "Nominations Submitted For Your Review.";
            var emailBodyPath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\EmailBodyToReviewer.html";
            new EmailService().SendEmail(emailBodyPath, reviewerEmailAddresses, subject);
        }
    }
}
