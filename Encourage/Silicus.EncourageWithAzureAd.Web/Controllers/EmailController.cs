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
    }
}