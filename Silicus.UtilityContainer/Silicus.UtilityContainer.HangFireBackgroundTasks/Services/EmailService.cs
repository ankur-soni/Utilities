using Silicus.FrameWorx.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
            _logger.Log("EmailService-SendEmail");

            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["UserName"], "Silicus Rewards and Recognition Team");
            string subject = emailSubject;
            string body = string.Empty;

            //using ( StreamReader reader = new StreamReader(emailViewPath))
            //{
            //    body = reader.ReadToEnd();
            //}
            body = emailViewPath;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"])
            };

            using (var message = new MailMessage() { Subject = subject, Body = body })
            {
                message.From = fromAddress;

                foreach (string email in ToEmailAddresses)
                {
                    // message.To.Add("Asha.Bhandare@silicus.com");
                    message.To.Add(email);
                }
                message.IsBodyHtml = true;
                try
                {
                    _logger.Log("EmailService-SendEmail-try");
                    smtp.Send(message);
                }
                catch(Exception ex)
                {
                    _logger.Log("EmailService-SendEmail-"+ex.Message);
                    var errorMessage = ex.Message;
                }
                
            }
        }
    }
}
