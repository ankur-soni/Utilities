using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using Silicus.FrameWorx.Logger;

namespace Silicus.EncourageWithAzureAd.Web
{
    public class EmailNotificationOfWinnerService : IEmailNotificationOfWinner
    {
        private IAwardService _awardService;
        private readonly ILogger _logger;

        public EmailNotificationOfWinnerService(IAwardService awardService,ILogger logger)
        {
            _awardService = awardService;
            _logger = logger;
        }
        public void Process()
        {
            _logger.Log("EmailNotificationOfWinnerService-Process");

            try
            {
                _logger.Log("EmailNotificationOfWinnerService-Process   try");
                var allWinners = _awardService.GetWinnerData();
                // var htmlPagePath = @"C:\Users\IKadam.SILICUS\Source\Repos\Utilities3\Encourage\Silicus.EncourageWithAzureAd.Web\Views\SendMailAfterWinnerSelected.html";
                var emailBodyPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Views/SendMailAfterWinnerSelected.html");
                foreach (WinnerData winnerData in allWinners)
                {
                    IDictionary<string, string> mergeFields = new Dictionary<string, string>()
                {
                    {"ENCOURAGE_WINNER_NAME", winnerData.Name},
                    {"ENCOURAGE_WINNER_PROJECT_NAME", winnerData.ProjectName},
                    {"ENCOURAGE_WINNER_AWARD_NAME", winnerData.AwardName},
                    {"ENCOURAGE_WINNER_AWARDPERIOD", winnerData.AwardPeriod},
                    {"ENCOURAGE_WINNER_MANAGER_NAME", winnerData.ManagerName},
                };

                    var regex = new Regex(String.Join("|", mergeFields.Keys));
                    var mailBody = regex.Replace(File.ReadAllText(emailBodyPath), m => mergeFields[m.Value]);
                    var winnerManagerEmailAddresses = _awardService.GetEmailAddressOfManager(winnerData.ManagerName);
                    var winnerName = winnerData.Name;
                    var awardName = winnerData.AwardName;
                    var awardPeriod = winnerData.AwardPeriod;
                    var subject = " " + winnerName + " : " + awardName + " - " + awardPeriod;

                    var fromAddress = new MailAddress(ConfigurationManager.AppSettings["UserName"], "Silicus Rewards and Recognition Team");
                    //  const string fromPassword = "Indra@123";
                    //  var fromAddress = new MailAddress(ConfigurationManager.AppSettings["UserName"], "Silicus Rewards and Recognition Team");
                    //const string fromPassword = ConfigurationManager.AppSettings["Password"];
                    string body = string.Empty;

                    body = mailBody;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"])
                    };

                    using (var message = new MailMessage() { Subject = subject, Body = body })
                    {
                        message.From = fromAddress;

                        foreach (string email in winnerManagerEmailAddresses)
                        {
                            message.To.Add("asha.bhandare@silicus.com");
                        }

                        message.IsBodyHtml = true;
                        smtp.Send(message);
                        _logger.Log("Email sent.");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Log( string.Format("EmailNotificationOfWinnerService-Process - Exception {0}",ex.Message));
            }
            
        }
    }
}