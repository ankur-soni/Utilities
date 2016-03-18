namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string emailId, string subject, string body);

        void SendEmailAsync(string emailId, string subject, string body);
              
    }
}