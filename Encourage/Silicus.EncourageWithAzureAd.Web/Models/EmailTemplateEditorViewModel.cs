using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class EmailTemplateEditorViewModel
    {
        public EmailTemplateEditorViewModel()
        {
            Users = new List<UserViewModel>();
        }

        public string EmailTemplate { get; set; }

        public List<UserViewModel> Users { get; set; }
    }
}