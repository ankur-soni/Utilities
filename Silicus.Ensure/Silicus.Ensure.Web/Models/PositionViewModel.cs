using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Models
{
    public class PositionViewModel
    {
        public int PositionId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Position Name is required!")]
        [Display(Name = "Position Name")]
        public string PositionName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
