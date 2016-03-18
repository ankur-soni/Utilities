using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface ISmtpClient
    {
        // Properties
        X509CertificateCollection ClientCertificates { get; }        

        ICredentialsByHost Credentials { get; set; }

        SmtpDeliveryMethod DeliveryMethod { get; set; }

        bool EnableSsl { get; set; }

        string Host { get; set; }

        string PickupDirectoryLocation { get; set; }

        int Port { get; set; }

        ServicePoint ServicePoint { get; }

        int Timeout { get; set; }

        bool UseDefaultCredentials { get; set; }

        // Methods
        void Send(MailMessage message);
       
        Task SendAsync(MailMessage message);
    }
}
