using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.Services
{
   public class EmailService
    {
        public void SendEmail(string emailViewPath, List<string> ToEmailAddresses,string emailSubject)
        {
            var fromAddress = new MailAddress("Indrajit.kadam@silicus.com", "Silicus Rewards and Recognition Team");
           // const string fromPassword = "Godfather.1515";
            string subject = emailSubject;
            string body = string.Empty;
                
            using ( StreamReader reader = new StreamReader(emailViewPath))
            {
                body = reader.ReadToEnd();
            }

            var smtp = new SmtpClient
            {
                Host = "outlook.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = false,
                //Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                Credentials = new NetworkCredential("Indrajit.kadam@silicus.com", "Indra@123")
            };

            using (var message = new MailMessage() { Subject = subject, Body = body })
            {
                message.From = fromAddress;

                //foreach(string email in ToEmailAddresses)
                //{
                //    message.To.Add(email);
                //}

                foreach (string email in new List<string>() { "Indrajit.kadam@silicus.com"})
                {
                    message.To.Add(email);
                }

                message.IsBodyHtml = true;
                try
                {
                    smtp.Send(message);
                }
                catch(Exception ex)
                {
                    var errorMessage = ex.Message;
                }
                
            }
        }
    }
}
