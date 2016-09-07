using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Reusable.Web.Models.ViewModel
{
    public class ExtensionCodeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Method Name")]
        public string MethodName { get; set; }

        [Display(Name = "Your Code")]
        [Required(ErrorMessage = "Please Enter the Code")]
        [AllowHtml]
        public string Code { get; set; }

        public string Description { get; set; }

        [Display(Name = "Example Usage")]
        public string ExampleUsage { get; set; }

        public string Language { get; set; }

        public DateTime CreationDate { get; set; }
        public int FrequentSearchedCount { get; set; }

    }
}