using Silicus.Encourage.Services.Interface;
using Silicus.EncourageWithAzureAd.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
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
            return null;
        }
    }
}