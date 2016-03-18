using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Silicus.Finder.Services.Interfaces;

namespace Silicus.Finder.Services
{
    /// <summary>
    /// This class is written to warp SmtpClient class to make it mockable.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SmtpClientWrapper : ISmtpClient, IDisposable
    {
        private readonly SmtpClient smtpClient;
        private bool disposed = false;        

        public SmtpClientWrapper()
        {
            smtpClient = new SmtpClient();
        }

        public X509CertificateCollection ClientCertificates
        {
            get
            {
                return smtpClient.ClientCertificates;
            }
        }

        public ICredentialsByHost Credentials
        {
            get
            {
                return smtpClient.Credentials;
            }

            set
            {
                smtpClient.Credentials = value;
            }
        }

        public SmtpDeliveryMethod DeliveryMethod
        {
            get
            {
                return smtpClient.DeliveryMethod;
            }

            set
            {
                smtpClient.DeliveryMethod = value;
            }
        }

        public bool EnableSsl
        {
            get
            {
                return smtpClient.EnableSsl;
            }

            set
            {
                smtpClient.EnableSsl = value;
            }
        }

        public string Host
        {
            get
            {
                return smtpClient.Host;
            }

            set
            {
                smtpClient.Host = value;
            }
        }

        public string PickupDirectoryLocation
        {
            get
            {
                return smtpClient.PickupDirectoryLocation;
            }

            set
            {
                smtpClient.PickupDirectoryLocation = value;
            }
        }

        public int Port
        {
            get
            {
                return smtpClient.Port;
            }

            set
            {
                smtpClient.Port = value;
            }
        }

        public ServicePoint ServicePoint
        {
            get
            {
                return smtpClient.ServicePoint;
            }
        }

        public int Timeout
        {
            get
            {
                return smtpClient.Timeout;
            }

            set
            {
                smtpClient.Timeout = value;
            }
        }

        public bool UseDefaultCredentials
        {
            get
            {
                return smtpClient.UseDefaultCredentials;
            }

            set
            {
                smtpClient.UseDefaultCredentials = value;
            }
        }

        public void Send(MailMessage message)
        {
            smtpClient.Send(message);
        }

        public Task SendAsync(MailMessage message)
        {
            return smtpClient.SendMailAsync(message);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.smtpClient != null)
                {
                    this.smtpClient.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
