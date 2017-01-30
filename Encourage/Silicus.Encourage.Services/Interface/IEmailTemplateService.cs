using Silicus.Encourage.Models;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface IEmailTemplateService
    {
        EmailTemplate GetEmailTemplate(int templateId);
        List<EmailTemplate> GetAllTemplates();
        List<User> GetAllManagers();
        string SendEmail(List<string> ToEmailAddresses, string body, string emailSubject);
        string UpdateEmailTemplate(int templateId, string updatedTemplate);
        EmailTemplate SaveEmailTemplate(string templateName, string template);
    }
}
