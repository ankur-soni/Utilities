using System.Collections.Generic;

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