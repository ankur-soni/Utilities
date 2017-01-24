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
        EmailTemplate GetEmailTemplate(string templateName);
        List<User> GetAllManagers();
        string SendEmail(List<string> ToEmailAddresses, string body, string emailSubject);
        string SaveEmailTemplate(string templateName, string updatedTemplate);
    }
}
