using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Silicus.FrameworxProject.Models
{
    public class Frameworx
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Html Description")]
        [AllowHtml]
        public string HtmlDescription { get; set; }

        [Display(Name = "Demo Link")]
        public string DemoLink { get; set; }

        [Display(Name = "Source Code Link")]
        public string SourceCodeLink { get; set; }

        
        public int CategoryId { get; set; }

        public int OwnerId { get; set; }

        public virtual FrameworxCategory Category { get; set; }

        public virtual ICollection<FrameworxLike> Likes { get; set; }


    }
}
