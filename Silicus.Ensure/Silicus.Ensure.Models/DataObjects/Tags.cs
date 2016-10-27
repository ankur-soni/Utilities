using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Tags
    {
        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = "Tag Name is required!")]
        public string TagName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
