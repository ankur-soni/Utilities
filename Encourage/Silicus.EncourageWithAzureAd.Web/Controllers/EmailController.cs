using Silicus.Encourage.Services.Interface;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ILogger _logger;
        private readonly INominationService _nominationService;
        public EmailController(IEmailTemplateService emailTemplateService,ILogger logger, INominationService nominationService)
        {
            _emailTemplateService = emailTemplateService;
            _logger = logger;
            _nominationService = nominationService;
        }


        // GET: Email
        public ActionResult Index()
        {
            EmailTemplateViewModel emailTemplateViewModel = new EmailTemplateViewModel();
            var emailTemplates = _emailTemplateService.GetAllTemplates();

            foreach (var template in emailTemplates)
            {
                emailTemplateViewModel.ProcessesViewModel.Add(new ProcessesViewModel() { Id = template.Id, Name = template.TemplateName });
            }

            return View(emailTemplateViewModel);
        }

        public ActionResult GetEmailTemplate(int processId)
        {
            var emailTemplate = _emailTemplateService.GetEmailTemplate(processId);
            var emailTemplateEditor = new EmailTemplateEditorViewModel {EmailTemplate = emailTemplate.Template};
            var allEmployees = _nominationService.GetAllResources();

            allEmployees.ForEach(x => emailTemplateEditor.Users.Add(new UserViewModel()
            {
                UserId = x.ID,
                Email = x.EmailAddress,
                Name = x.DisplayName
            })
            );
            //var allManagers = _emailTemplateService.GetAllManagers(emailTemplate.TemplateName);

            //foreach (var manager in allManagers)
            //{
            //    var user = new UserViewModel();
            //    user.Email = manager.EmailAddress;
            //    user.Name = manager.DisplayName;
            //    user.UserId = manager.ID;

            //    emailTemplateEditor.Users.Add(user);
            //}

            return PartialView("~/Views/Email/Shared/_emailTemplateEditor.cshtml", emailTemplateEditor);
        }

        [HttpPost]
        public string SendMailToManagers(List<string> managersList,string emailTemplate, string subject)
        {
            _logger.Log("Post - SendMailToManagers - start");
            string emailBody = HttpUtility.HtmlDecode(emailTemplate);

          //  string subject = ConfigurationManager.AppSettings["MailNominationSubject"];

            var result = _emailTemplateService.SendEmail(managersList, emailBody, subject);
            _logger.Log("Post - SendMailToManagers - end - result -"+result);
            return result;
        }

        public string UpdateMailTemplate(int processId, string emailTemplate)
        {
            string emailBody = HttpUtility.HtmlDecode(emailTemplate);

            var result = _emailTemplateService.UpdateEmailTemplate(processId, emailBody);

            return result;
        }

        public string SaveMailTemplate(string processName, string emailTemplate)
        {
            var templateOf = processName;
            string emailBody = HttpUtility.HtmlDecode(emailTemplate);

            var result = _emailTemplateService.SaveEmailTemplate(templateOf, emailBody);
            string newTemplateName = result != null ? result.TemplateName : "";
            return newTemplateName;
        }
    }
}