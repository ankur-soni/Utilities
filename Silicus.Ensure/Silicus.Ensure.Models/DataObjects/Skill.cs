using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Skill Name is required!")]
        public string SkillName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
