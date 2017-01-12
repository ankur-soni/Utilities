using System.Collections.Generic;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class EmailTemplateViewModel
    {
        public EmailTemplateViewModel()
        {
            ProcessesViewModel = new List<ProcessesViewModel>();
            EmailTemplateEditor = new EmailTemplateEditorViewModel();
        }

        public List<ProcessesViewModel> ProcessesViewModel { get; set; }

        public EmailTemplateEditorViewModel EmailTemplateEditor { get; set; }
    }
}