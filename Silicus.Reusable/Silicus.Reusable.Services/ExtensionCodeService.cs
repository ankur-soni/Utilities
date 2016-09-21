using Silicus.FrameworxProject.DAL.Interfaces;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Mail;

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
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(a=>a.ReviewFlag==true).ToList();
        }

        public List<ExtensionSolution> GetAllExtensionSolution()
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().ToList();
        }

        public List<ExtensionSolution> GetAllReviewExtensionSolution(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(a => a.ReviewFlag == false && a.reviewerid==id).ToList();
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

        public string EmailSendToReviewer(EmailFormModel model)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("birthare06@gmail.com");
            mail.To.Add(model.ToEmail);
            mail.Subject = "One Frameworx Method Review Request";
            mail.Body =model.Message;

            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("birthare06@gmail.com", "devdev@123");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return "Email sent";
        }
    }
}
