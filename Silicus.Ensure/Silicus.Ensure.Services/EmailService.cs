using System;
using System.Net.Mail;
using System.Web;
using Silicus.FrameWorx.Utility;
using Silicus.Ensure.Services.Interfaces;
using System.Threading;

namespace Silicus.Ensure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpClient _smtpClient;

        public EmailService(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;           
        }

        private delegate void SendEmailDelegate(System.Net.Mail.MailMessage m);

        public void SendEmail(string emailId, string subject, string body)
        {
            var msg = PrepareMessage(emailId, subject, body);
            _smtpClient.Send(msg);
        }       

        public void SendEmailAsync(string emailId, string subject, string body)
        {
            var msg = PrepareMessage(emailId, subject, body);
            //SendEmailDelegate sd = new SendEmailDelegate(_smtpClient.Send);
            //AsyncCallback cb = new AsyncCallback(SendEmailResponse);
            //sd.BeginInvoke(msg, cb, sd); 
            _smtpClient.Send(msg);
        }        

        private static void SendEmailResponse(IAsyncResult ar)
        {
            SendEmailDelegate sd = (SendEmailDelegate)ar.AsyncState;
            sd.EndInvoke(ar);
        }

        private static MailMessage PrepareMessage(string emailId, string subject, string body)
        {
            Guard.ArgumentNotNullOrEmpty(emailId, "emailId");
            Guard.ArgumentNotNullOrEmpty(subject, "subject");
            Guard.ArgumentNotNullOrEmpty(body, "body");

            var msg = new MailMessage();
            msg.To.Add(new MailAddress(emailId));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            //msg.Body = body;
			msg.Body = HttpUtility.HtmlDecode(body);
            return msg;
        }


        public void SendEmailInBackgroundThread(string emailId, string subject, string body)
        {
            var msg = PrepareMessage(emailId, subject, body);
            Thread bgThread = new Thread(new ParameterizedThreadStart(_smtpClient.SendMail));
            bgThread.IsBackground = true;
            bgThread.Start(msg);
        }
    }
}
