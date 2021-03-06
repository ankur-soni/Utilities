﻿using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;

        private readonly UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;

        public EmailTemplateService(IDataContextFactory dataContextFactory, ICommonDbService commonDbService, ILogger logger, ICustomDateService customdateService)
        {
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _commonDataBaseContext = commonDbService.GetCommonDataBaseContext();
        }

        public EmailTemplate GetEmailTemplate(int templateId)
        {
            return _encourageDatabaseContext.Query<EmailTemplate>().FirstOrDefault(t => t.Id == templateId);
        }

        public List<EmailTemplate> GetAllTemplates()
        {
            return _encourageDatabaseContext.Query<EmailTemplate>().ToList();
        }

        public List<User> GetAllManagers(string templateName)
        {
            var managers = new List<User>();
            List<int> orDefault = new List<int>();
            switch (templateName)
            {
                case "Nomination":
                    orDefault.Add(_commonDataBaseContext.Query<Role>().FirstOrDefault(r => r.Name == "Manager" && r.IsActive).ID);
                    break;
                case "Review":
                    orDefault.Add(_commonDataBaseContext.Query<Role>().FirstOrDefault(r => r.Name == "Reviewer" && r.IsActive).ID);
                    break;
                default:
                    orDefault.Add(_commonDataBaseContext.Query<Role>().FirstOrDefault(r => r.Name == "Manager" && r.IsActive).ID);
                    orDefault.Add(_commonDataBaseContext.Query<Role>().FirstOrDefault(r => r.Name == "Reviewer" && r.IsActive).ID);
                    break;
            }
            
            if (orDefault.Count > 0)
            {
                var utility = _commonDataBaseContext.Query<Utility>().FirstOrDefault(r => r.Name == "Encourage");
                if (utility != null)
                {
                    var utilityId = utility.Id;
                    var listOfMangerIds = _commonDataBaseContext.Query<UtilityUserRoles>().Where(u => u.UtilityId == utilityId && orDefault.Contains(u.RoleId) &&  u.IsActive).Select(m => m.UserId).ToList();
                    managers = _commonDataBaseContext.Query<User>().Where(u => listOfMangerIds.Contains(u.ID)).ToList();
                }
            }

            return managers;
        }

        public string SendEmail(List<string> ToEmailAddresses,string body, string emailSubject)
        {
            ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
            _logger.Log("EmailService-SendEmail");

            var smtp = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = "mail.global.frontbridge.com",
                Port = 25,
                EnableSsl = false,
                Timeout = 600000
            };

            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["MailUserName"], "Silicus Rewards and Recognition Team");

            using (var message = new MailMessage() { Subject = emailSubject, Body = body })
            {
                message.From = fromAddress;

                foreach (string email in ToEmailAddresses)
                {
                    message.Bcc.Add(email);
                }
                message.IsBodyHtml = true;
                try
                {
                    _logger.Log("EmailService-SendEmail-try");
                     smtp.Send(message);
                    return "Success";
                }
                catch (Exception ex)
                {
                    _logger.Log("EmailService-SendEmail-" + ex.Message);
                    return "Error";
                }
            }
        }

        public string UpdateEmailTemplate(int templateId, string updatedTemplate)
        {
            var toBeUpdatedTemplate =  _encourageDatabaseContext.Query<EmailTemplate>().FirstOrDefault(t => t.Id == templateId);
            if (toBeUpdatedTemplate != null)
            {
                toBeUpdatedTemplate.Template = updatedTemplate;
                var returnedValue = _encourageDatabaseContext.Update(toBeUpdatedTemplate);
                if (returnedValue == 0)
                {
                    return "Error";
                }
            }
            return "Success";
        }

        public EmailTemplate SaveEmailTemplate(string templateName, string template)
        {
            EmailTemplate newTemplate = new EmailTemplate();
            newTemplate.TemplateName = templateName;
            newTemplate.Template = template;

            return _encourageDatabaseContext.Add(newTemplate);
        }
    }
}
