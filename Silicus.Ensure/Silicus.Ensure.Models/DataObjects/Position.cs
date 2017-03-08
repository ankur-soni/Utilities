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
        [Required(ErrorMessage = "Position name is required.")]
        [Remote("IsDuplicatePositionName", "Positions", ErrorMessage = "Position name already exists.")]
        [Display(Name = "Position name")]
        public string PositionName { get; set; }
        public bool IsDeleted { get; set; }

        public Position()
        {
            IsDeleted = false;
        }
    }
}
