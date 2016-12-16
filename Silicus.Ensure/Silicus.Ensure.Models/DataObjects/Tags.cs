using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Tags
    {
        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = "Tag Name is required!")]
        [StringLength(50,ErrorMessage="Tag length should be less than or equal to 50 characters.")]
        [Display(Name="Tag Name")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Space are not allowd")]
        [Remote("IsDuplicateTagName", "Tag", ErrorMessage = "Tag name already exist !")]
        public string TagName { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(200, ErrorMessage = "Description length should be less than or equal to 50 characters.")]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }
    }
}
