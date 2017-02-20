using System.Threading.Tasks;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string emailId, string subject, string body);     

        void SendEmailAsync(string emailId, string subject, string body);

        void SendEmailInBackgroundThread(string emailId, string subject, string body);
    }
}