using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Position Name is required!")]
        [Remote("IsDuplicatePositionName", "Admin", ErrorMessage = "Position name exist !")]
        [Display(Name = "Position Name")]
        public string PositionName { get; set; }
    }
}
