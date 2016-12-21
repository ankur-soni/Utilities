using System;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.Models;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Models.Enumerations;
using System.Reflection;
using System.Configuration;

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

        public void Process(EventType eventType,string awardName)
        {
            _logger.Log("EncourageEmailProcessor-Process-EventType-" + eventType);

            switch (eventType)
            {
                case EventType.SendNominationEmail:
                    SendNominationEmail(awardName, System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["ManagersEmailTemlate"]));
                    break;
                case EventType.SendReviewNominationEmail:
                    SendReviewNominationEmail(awardName, System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["ReviewersEmailTemplate"]));
                    break;
                case EventType.SendAdminNominationEmail:
                    SendAdminNominationEmail(awardName, System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["AdminsEmailtemplate"]));
                    break;
                default:
                    break;

            }
        }

        private string getEmailBody(string awardName,string templatePath)
        {
            string emailTemplate = "";
            using (StreamReader reader = new StreamReader(templatePath))
            {
                emailTemplate = reader.ReadToEnd();
                emailTemplate = emailTemplate.Replace("{awardname}", awardName);
            }

            return emailTemplate;
        }
        private void SendNominationEmail(string awardName, string templatePath)
        {
            _logger.Log("EncourageEmailProcessor-SendNominationEmail");
            var managerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(369);

            if (managerEmailAddresses.Count <= 0)
            {
                return;
            }

            var emailBody = getEmailBody(awardName,templatePath);
            var subject = "Submit Your Nominations";
            //new EmailService().SendEmail(emailBodyPath, managerEmailAddresses, subject);
            new EmailService().SendEmail(emailBody, new List<string>() { "asha.bhandare@silicus.com" }, subject);
        }

        private void SendReviewNominationEmail(string awardName, string templatePath)
        {
            var reviewerEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(371);

            if (reviewerEmailAddresses.Count <= 0)
            {
                return;
            }

            var emailBody = getEmailBody(awardName, templatePath);
            var subject = "Nominations Submitted For Your Review.";
            //new EmailService().SendEmail(emailBodyPath, reviewerEmailAddresses, subject);
            new EmailService().SendEmail(emailBody, new List<string>() { "asha.bhandare@silicus.com" },subject);
        }

        private void SendAdminNominationEmail(string awardName, string templatePath)
        {
            
            var adminEmailAddresses = GetEmailAddress.GetEmailAddressForRoles(370);

            if(adminEmailAddresses.Count <= 0)
            {
                return;
            }

            var emailBody = getEmailBody(awardName, templatePath);

            var subject = "Review Process Locked";
            //new EmailService().SendEmail(emailBodyPath, adminEmailAddresses, subject);
            new EmailService().SendEmail(emailBody, new List<string>() { "asha.bhandare@silicus.com" },subject);
        }
    }
}
