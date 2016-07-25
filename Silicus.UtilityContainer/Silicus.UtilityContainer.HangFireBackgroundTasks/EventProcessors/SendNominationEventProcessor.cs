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
    public class SendNominationEventProcessor : IEventProcessor
    {
        public void Process()
        {
            var managerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(364);
            var subject = "Submit Your Nominations";
            var emailBodyPath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Silicus.UtilityContainer\Silicus.UtilityContainer.HangFireBackgroundTasks\View\EmailToManagerBody.html";
            //var emailBodyPath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\EmailToManagerBody.html";
            new EmailService().SendEmail(emailBodyPath, managerEmailAddresses, subject);
        }
    }
}

