using Silicus.Encourage.Services.Interface;
using Silicus.EncourageWithAzureAd.Web.Models;
using System;
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
        public EmailController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }


        // GET: Email
        public ActionResult Index()
        {
            EmailTemplateViewModel emailTemplateViewModel = new EmailTemplateViewModel();

            emailTemplateViewModel.ProcessesViewModel = new List<ProcessesViewModel>()
            {
                new ProcessesViewModel() { Id = 1, Name="Nomination" },
                new ProcessesViewModel() { Id = 2, Name="Review" }
            };

            return View(emailTemplateViewModel);
        }

        public ActionResult GetEmailTemplate(string processName)
        {
            var templateOf = processName + "Template";
            var emailTemplate = _emailTemplateService.GetEmailTemplate(templateOf);
            EmailTemplateEditorViewModel emailTemplateEditor = new EmailTemplateEditorViewModel();

            emailTemplateEditor.EmailTemplate = emailTemplate.Template;

            var allManagers = _emailTemplateService.GetAllManagers();

            foreach (var manager in allManagers)
            {
                var user = new UserViewModel();
                user.Email = manager.EmailAddress;
                user.Name = manager.DisplayName;
                user.UserId = manager.ID;

                emailTemplateEditor.Users.Add(user);
            }

            return PartialView("~/Views/Email/Shared/_emailTemplateEditor.cshtml", emailTemplateEditor);
        }

        [HttpPost]
        public string SendMailToManagers(List<string> managersList,string emailTemplate)
        {
            string emailBody = HttpUtility.HtmlDecode(emailTemplate);

            string subject = ConfigurationManager.AppSettings["MailNominationSubject"];

            var result = _emailTemplateService.SendEmail(managersList, emailBody, subject);

            return result;
        }
    }
}