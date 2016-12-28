using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class EmailTemplateViewModel
    {
        public EmailTemplateViewModel()
        {
            ProcessesViewModel = new List<ProcessesViewModel>();
        }

        public List<ProcessesViewModel> ProcessesViewModel { get; set; }
    }
}