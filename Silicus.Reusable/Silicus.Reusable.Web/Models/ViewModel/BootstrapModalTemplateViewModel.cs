namespace Silicus.Reusable.Web.Models.ViewModel
{
    public class BootstrapModalTemplateViewModel
    {

        public string Id { get; set; }

        public string Title { get; set; }

        public string CloseButtonText { get; set; }

        public string CloseButtonId { get; set; }

        public string SaveButtonText { get; set; }

        public string SaveButtonId { get; set; }

        public string SaveButtonOnlickEvent { get; set; }

        public bool? SaveButtonVisible { get; set; }

        public bool? CloseButtonVisible { get; set; }

        public bool? DismissModalOnCloseClick { get; set; }

        public bool? DismissModalOnSaveClick { get; set; }

        public string ContentHtml { get; set; }
    }
}