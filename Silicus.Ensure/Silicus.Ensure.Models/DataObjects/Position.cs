using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }

        [Required(ErrorMessage = "Position Name is required!")]        
        [Display(Name = "Position Name")]
        public string PositionName { get; set; }
    }
}
