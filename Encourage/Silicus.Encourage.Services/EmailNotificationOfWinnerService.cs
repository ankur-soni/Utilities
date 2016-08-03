﻿using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web
{
    public class EmailNotificationOfWinnerService: IEmailNotificationOfWinner
    {
        private IAwardService _awardService;
        public EmailNotificationOfWinnerService(IAwardService awardService)
        {
            _awardService = awardService;
        }
        public void Process()
        {
            var allWinners =  _awardService.GetWinnerData().FirstOrDefault(); //getting only first user
            //var htmlPagePath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\SendMailAfterWinnerSelected.html";
            //var htmlPagePath = @"C:\Users\aajadhav\Source\Repos\UtilitiesDeployedOnAzure\Silicus.UtilityContainer\HangFireBackgroundTasks\View\SendMailAfterWinnerSelected.html";
            var htmlPagePath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Encourage\Silicus.EncourageWithAzureAd.Web\Views\SendMailAfterWinnerSelected.html";
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

            var allUsersEmailAddresses = _awardService.GetEmailAddressesOfAllUsers();
            var winnerName = allWinners.Name;
            var awardName = allWinners.AwardName;
            var awardPeriod = allWinners.AwardPeriod;
            var subject = " " + winnerName + " : " + awardName + " - " + awardPeriod;

            var fromAddress = new MailAddress("indrajit.kadam@silicus.com", "Silicus Rewards and Recognition Team");
            const string fromPassword = "Indra@123";
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

                foreach (string email in new List<string>() { "indrajit.kadam@silicus.com", "gajanan.annamwar@silicus.com" })
                {
                    message.To.Add(email);
                }

                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }
    }
}