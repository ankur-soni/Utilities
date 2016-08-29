using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.Models
{
    public class CodeType
    {
        public CodeType()
        {
            ExtensionSolutions = new List<ExtensionSolution>();
        }

        public int Id { get; set; }

        [Display(Name = "Language")]
        [Required(ErrorMessage = "Please Select the Language")]
        public string Name { get; set; }

        public virtual ICollection<ExtensionSolution> ExtensionSolutions { get; set; }

        public virtual ICollection<OtherCode> OtherCodes { get; set; }
    }
}
