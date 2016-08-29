using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silicus.FrameworxProject.Models
{
    public class OtherCode
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Method Name")]
        [Display(Name = "Method Name")]
        public string MethodName { get; set; }

        [Display(Name = "Your Code")]
        [Required(ErrorMessage = "Please Enter the Code")]
        [AllowHtml]
        public string Code { get; set; }

        public string Description { get; set; }

        [Display(Name = "Example Usage")]
        public string ExampleUsage { get; set; }

        //   public List<string> Tags { get; set; }
        [Required(ErrorMessage = "Please Enter the Language Type")]
        public int CodeTypeId { get; set; }

        public DateTime CreationDate { get; set; }

        public int FrequentSearchedCount { get; set; }

        public virtual CodeType CodeType { get; set; }
    }
}
