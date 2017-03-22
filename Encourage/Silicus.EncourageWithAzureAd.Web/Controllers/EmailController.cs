using System;
using Silicus.Encourage.Services.Interface;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ILogger _logger;
        private readonly INominationService _nominationService;
        private readonly IAwardService _awardService;
        private readonly ICustomDateService _customDateService;
        public EmailController(IEmailTemplateService emailTemplateService,ILogger logger, INominationService nominationService, IAwardService awardService, ICustomDateService customDateService)
        {
            _emailTemplateService = emailTemplateService;
            _logger = logger;
            _nominationService = nominationService;
            _awardService = awardService;
            _customDateService = customDateService;
        }


        // GET: Email
        public ActionResult Index()
        {
            var awards = _awardService.GetAllAwards().Select( x => new AwardViewModel()
            {
                AwardId =  x.Id,
                AwardCode = x.Code,
                AwardTitle = x.Name
            }
            ).ToList();

            var emailTemplateViewModel = new EmailTemplateViewModel() { Awards = awards };
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
            return PartialView("~/Views/Email/Shared/_emailTemplateEditor.cshtml", emailTemplateEditor);
        }

        [HttpPost]
        public string SendMailToManagers(List<string> managersList,string emailTemplate, string subject)
        {
            _logger.Log("Post - SendMailToManagers - start");
            var emailBody = HttpUtility.HtmlDecode(emailTemplate);
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

        public string GetAwardPeriod(int awardId)
        {
            var awardPeriod = _customDateService.GetCustomDate(awardId);
           var month = @System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(awardPeriod.Month);
            var stringtoReturn = month + "-" + awardPeriod.Year;
            return stringtoReturn;
        }
    }
}