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

namespace Silicus.Ensure.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IPanelService _panelService;
        private readonly ITagsService _tagService;
        private readonly IPositionService _positionService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        private readonly IEmailService _emailService;
        private readonly Silicus.UtilityContainer.Services.Interfaces.IRoleService _roleService;
        public CommonController(IPanelService panelService, ITagsService tagService, IPositionService positionService, Silicus.UtilityContainer.Services.Interfaces.IUserService containerUserService, IEmailService emailService, Silicus.UtilityContainer.Services.Interfaces.IRoleService roleService)
        {
            _panelService = panelService;
            _tagService = tagService;
            _positionService = positionService;
            _containerUserService = containerUserService;
            _emailService = emailService;
            _roleService = roleService;
        }

        public ActionResult GetPanelDetails()
        {
            var panellist = _panelService.GetPanelDetails();
            if (panellist.Any())
            {
                panellist = panellist.OrderByDescending(model => model.PanelId);
            }
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

        public ActionResult GetAllPositionDetails()
        {
            var positionlist = _positionService.GetPositionDetails().Where(y => y.IsDeleted != true).OrderByDescending(model => model.PositionId);
            return Json(positionlist, JsonRequestBehavior.AllowGet);
        }



     public void SendMailByRoleName(string subject, string templateName,List<string> roleName,string candidateName=null,string candidateStatus=null)
        {
            string retVal = "failed";
            List<EmailModel> emailList = new List<EmailModel>();
            foreach (string role in roleName)
            {
                if (!string.IsNullOrWhiteSpace(role))
                {
                    var roleDetails = _roleService.GetRoleByRoleName(role);
                    List<Silicus.UtilityContainer.Models.DataObjects.User> users = _containerUserService.GetAllUsersByRoleInUtility(1, roleDetails.ID);
                    foreach (var user in users)
                    {
                        if (user != null)
                        {
                            var viewModel = new EmailModel
                            {
                                Name = user.DisplayName,
                                Email = user.EmailAddress,
                                CandidateName = candidateName,
                                CandidateStatus=candidateStatus
                            };
                            emailList.Add(viewModel);
                        }
                    }
                }
            }           
           

           var template = System.IO.File.ReadAllText(Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplates/"+templateName));
            foreach (var emails in emailList)
            {
                if (!string.IsNullOrWhiteSpace(emails.Email))
                {
                    var body = RazorEngine.Razor.Parse(template, emails);

                    _emailService.SendEmailInBackgroundThread(emails.Email, subject, body);
                    retVal = "succeeded";
                }
            }

           
        }

    }
}