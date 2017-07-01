using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silicus.Ensure.Models;
using System.IO;
using RazorEngine;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.Constants;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Silicus.Ensure.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IPanelService _panelService;
        private readonly ITagsService _tagService;
       // private readonly IPositionService _positionService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        private readonly IEmailService _emailService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IRoleService _roleService;
        public CommonController(IPanelService panelService, ITagsService tagService, Silicus.UtilityContainer.Services.Interfaces.IUserService containerUserService, IEmailService emailService, Silicus.UtilityContainer.Services.Interfaces.IRoleService roleService)
        {
            _panelService = panelService;
            _tagService = tagService;
            //_positionService = positionService;
            _containerUserService = containerUserService;
            _emailService = emailService;
            _roleService = roleService;
        }

        public ActionResult GetPanelDetails()
        {
            var utilityId = GetUtilityId();
            var roleDetails = _roleService.GetRoleByRoleName("Panel");
            var panellist = _containerUserService.GetAllUsersByRoleInUtility(utilityId, roleDetails.ID).OrderByDescending(model => model.DisplayName).ToList(); 
            
            return Json(panellist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllTagDetails()
        {
            var taglist = _tagService.GetTagsDetails();
            if (taglist.Any())
            {
                taglist = taglist.OrderByDescending(model => model.TagId);
            }
            return Json(taglist, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetAllPositionDetails()
        //{
        //    var positionlist = _positionService.GetPositionDetails().Where(y => y.IsDeleted != true).OrderByDescending(model => model.PositionId);
        //    return Json(positionlist, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        public void SendMailByRoleName(string subject, string templateName,List<string> roleName,string candidateName=null,string candidateStatus=null,string RecruiterName=null)
        {
            string extension = ".cshtml";
            List<EmailModel> emailList = new List<EmailModel>();
            var utilityId = GetUtilityId();
            foreach (string role in roleName)
            {
                if (!string.IsNullOrWhiteSpace(role))
                {
                    var roleDetails = _roleService.GetRoleByRoleName(role);
                    if (roleDetails != null)
                    {
                        List<Silicus.UtilityContainer.Models.DataObjects.User> users = _containerUserService.GetAllUsersByRoleInUtility(utilityId, roleDetails.ID);
                        foreach (var user in users)
                        {
                            if (user != null)
                            {
                                var viewModel = new EmailModel
                                {
                                    Name = user.DisplayName,
                                    Email = user.EmailAddress,
                                    CandidateName = candidateName,
                                    CandidateStatus = candidateStatus,
                                    RecruiterName = RecruiterName
                                };
                                emailList.Add(viewModel);
                            }
                        }
                    }
                }
            }

            if (emailList.Count > 0)
            {
                var template = System.IO.File.ReadAllText(Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplates/" + templateName));
                foreach (var emails in emailList)
                {
                    if (!string.IsNullOrWhiteSpace(emails.Email))
                    {                     
                        var body = RazorEngine.Razor.Run(templateName.Substring(0, templateName.Length - extension.Length), emails);
                        _emailService.SendEmailInBackgroundThread(emails.Email, subject, body);

                    }
                }
            }

           
        }

        private int GetUtilityId()
        {
            var utilityProductId = WebConfigurationManager.AppSettings["ProductId"];
            if (string.IsNullOrWhiteSpace(utilityProductId))
            {
                throw new ArgumentNullException();
            }

            return Convert.ToInt32(utilityProductId);
        }

    }
}