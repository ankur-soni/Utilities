namespace Silicus.Finder.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string EmailAddress, string subject, string body);

        void SendEmailAsync(string EmailAddress, string subject, string body);
              
    }
}