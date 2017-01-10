using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Panel
    {

        [Key]
        public int PanelId { get; set; } 

        [StringLength(50)]
        [Required(ErrorMessage = "Panel is required!")]
        [Remote("IsDuplicatePanelName", "Panel", ErrorMessage = "Panel is already name exist !")]
        [Display(Name = "Panel name")]
        public string PanelName { get; set; } 
        public bool IsDeleted { get; set; }

        public Panel()
        {
            IsDeleted = false;
        }
    }
}
