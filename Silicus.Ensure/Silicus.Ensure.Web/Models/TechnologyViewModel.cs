using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class TechnologyViewModel
    {
        public int TechnologyId { get; set; }

        [Required(ErrorMessage = "Technology name is required.")]
        [StringLength(50, ErrorMessage = "Technology name length should be less than or equal to 50 characters.")]
        [Display(Name = "Technology name")]
        public string TechnologyName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, ErrorMessage = "Description length should be less than or equal to 50 characters.")]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }
}