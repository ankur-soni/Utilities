using Silicus.FrameworxProject.DAL.Interfaces;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Web;
using System.Net.Mail;
using System.IO;

namespace Silicus.FrameworxProject.Services
{
    public class ExtensionCodeService : IExtensionCodeService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IFrameworxProjectDatabaseContext _FrameworxProjectDatabaseContext;


        public ExtensionCodeService(Silicus.FrameworxProject.DAL.Interfaces.IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _FrameworxProjectDatabaseContext = _dataContextFactory.CreateFrameworxProjectDbContext();
        }

        public List<Frameworx> GetAllFrameworx()
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().ToList();
        }

        public List<ExtensionSolution> GetAllApprovedExtensionSolution()
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(a => a.ReviewFlag == true).ToList();
        }

        public List<ExtensionSolution> GetAllExtensionSolution()
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().ToList();
        }

        public List<ExtensionSolution> GetAllReviewExtensionSolution(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(a => a.ReviewFlag == false && a.reviewerid == id).ToList();
        }

        public List<ExtensionSolution> GetMyAllExtensionSolution(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(a => a.userid == id).ToList();
        }

        public void AddExtensionSolution(ExtensionSolution extensionSolution)
        {
            _FrameworxProjectDatabaseContext.Add<ExtensionSolution>(extensionSolution);
        }

        public void EditExtensionSolution(ExtensionSolution extensionSolution)
        {
            _FrameworxProjectDatabaseContext.Update<ExtensionSolution>(extensionSolution);
        }

        public void AddOtherCode(OtherCode otherCode)
        {
            _FrameworxProjectDatabaseContext.Add<OtherCode>(otherCode);
        }

        public void EditOtherCode(OtherCode otherCode)
        {
            _FrameworxProjectDatabaseContext.Update<OtherCode>(otherCode);
        }

        public List<FrameworxCategory> GetAllCategories()
        {
            return _FrameworxProjectDatabaseContext.Query<FrameworxCategory>().ToList();//Poulate Business Model h
        }

        public List<CodeType> GetAllCodeTypes()
        {
            return _FrameworxProjectDatabaseContext.Query<CodeType>().ToList();//Poulate Business Model h
        }

        public List<OtherCode> GetAllOtherCodeList()
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().ToList();
        }

        public List<OtherCode> GetAllApprovedOtherCodeList()
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().Where(a => a.ReviewFlag == true).ToList();
        }

        public List<OtherCode> GetMyAllOtherCodeList(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().Where(a => a.userid == id).ToList();
        }

        public List<OtherCode> GetAllReviewOtherCodeList(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().Where(a => a.ReviewFlag == false && a.reviewerid == id).ToList();
        }

        public ExtensionSolution GetExtensionMethodById(int ExtensionId)
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(exn => exn.Id == ExtensionId).First();

        }

        public OtherCode GetOtherCodeMethodById(int ExtensionId)
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().Where(exn => exn.Id == ExtensionId).First();

        }

        public void ExtensionFrequentSearchedCountUpdate(ExtensionSolution extensionSolution)
        {
            _FrameworxProjectDatabaseContext.Update<ExtensionSolution>(extensionSolution);
        }

        public void OtherCodeFrequentSearchedCountUpdate(OtherCode otherCode)
        {
            _FrameworxProjectDatabaseContext.Update<OtherCode>(otherCode);
        }

        public void SendEmail(string userName, string ToEmailAddresses, string emailSubject,string codeType,string link)
        {
            var fromAddress = new MailAddress("devendra.birthare@silicus.com", "Silicus Rewards and Recognition Team");
            const string fromPassword = "pinky@123";
            string subject = emailSubject;
            string body = string.Empty;

            body = "<p>Dear Review Committee Member,</p><p>Congratulations for the great efforts put in by you and your team.</p><p>We request you to please login to Frameworx system using below URL and submit your review to the "+ codeType + ". Submitted by "+ userName + ".</p>"+ link+" <p>Keep up the great work.Thank You !</p><p>Best regards,</p><p>Silicus Team </p><p>This is an auto - generated email.</p>";
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
                message.To.Add(ToEmailAddresses);
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Send(message);
            }
        }

        public async Task<bool> asyncSendEmail(string userName, string ToEmailAddresses, string emailSubject, string codeType, string link)
        {
            var fromAddress = new MailAddress("devendra.birthare@silicus.com", "Silicus Rewards and Recognition Team");
            const string fromPassword = "pinky@123";
            string subject = emailSubject;
            string body = string.Empty;

            body = "<p>Dear Review Committee Member,</p><p>Congratulations for the great efforts put in by you and your team.</p><p>We request you to please login to Frameworx system using below URL and submit your review to the " + codeType + ". Submitted by " + userName + ".</p>" + link + " <p>Keep up the great work.Thank You !</p><p>Best regards,</p><p>Silicus Team </p><p>This is an auto - generated email.</p>";
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
                message.To.Add(ToEmailAddresses);
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Send(message);
                await Task.Yield();
                return true;
            }
        }
    }
}
