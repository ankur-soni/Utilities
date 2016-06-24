using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Hangfire
{
    public class Mailer
    {
        public void sendMail()
        {
            var fromAddress = new MailAddress("abhi.jadhav1515@gmail.com", "From Name");
            var toAddress = new MailAddress("abhishek.jadhav@silicus.com", "To Name");
            const string fromPassword = "7385791619";
            const string subject = "Submit Your Nominations";
            const string body = "Dear Review Committee Member,\n Dear Manager,Congratulations for the great efforts put in by you and your team.\nIt's the time to get your top performing subordinates recognized for their great work. The nominations for STAR OF THE MONTH award category are open now.\n" +
                                 "We request you to please login to Encourage system using below URL and submit the names of your subordinates for getting considered for the mentioned award category.\n" +
                                 "http://encourage.silicus.com/nominate" +
                                 "\nKeep up the great work. Thank You !\n" +
                                 "\nBest regards,\n" +
                                 "\nSilicus Rewards and Recognition Team\n" +
                                 "This is an auto-generated mail.";


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

    }
}