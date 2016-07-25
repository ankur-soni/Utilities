using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using Silicus.UtilityContainer.HangFireBackgroundTasks.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.EventProcessors
{
    public class SendNotificationAfterWinnerSelected : IEventProcessor
    {
        public void Process()
        {
            var allWinners = new EncourageDataService().GetWinnerData().FirstOrDefault(); //getting only first user
            //var htmlPagePath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\SendMailAfterWinnerSelected.html";
            var htmlPagePath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\SendMailAfterWinnerSelected.html";

            IDictionary<string, string> map = new Dictionary<string, string>()
        {
           {"ENCOURAGE_WINNER_NAME",allWinners.Name},
           {"ENCOURAGE_WINNER_PROJECT_NAME",allWinners.ProjectName},
           {"ENCOURAGE_WINNER_AWARD_NAME",allWinners.AwardName},
           {"ENCOURAGE_WINNER_AWARDPERIOD",allWinners.AwardPeriod},
           {"ENCOURAGE_WINNER_MANAGER_NAME",allWinners.ManagerName},
        };

            var regex = new Regex(String.Join("|", map.Keys));
            var newStr = regex.Replace(File.ReadAllText(htmlPagePath), m => map[m.Value]);

            var allUsersEmailAddresses = GetEmailAddress.GetEmailAddressesOfAllUsers();
            var winnerName = allWinners.Name;
            var awardName = allWinners.AwardName;
            var awardPeriod = allWinners.AwardPeriod;
            var subject = " " + winnerName + " : " + awardName + " - " + awardPeriod;

            var fromAddress = new MailAddress("abhishek.jadhav@silicus.com", "Silicus Rewards and Recognition Team");
            const string fromPassword = "Godfather.1515";
            string body = string.Empty;

            body = newStr;
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage() { Subject = subject, Body = body })
            {
                message.From = fromAddress;

                //foreach(string email in ToEmailAddresses)
                //{
                //    message.To.Add(email);
                //}

                foreach (string email in new List<string>() { "abhishek.jadhav@silicus.com", "asha.bhandare@silicus.com" })
                {
                    message.To.Add(email);
                }

                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }


    }
}

