using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class TestSuite
    {
        [Key]
        public int TestSuiteId { get; set; }

        [Required(ErrorMessage = "Test-Suite Name is required!")]
        [StringLength(50, ErrorMessage = "Test-Suite name length should be less than or equal to 50 characters.")]
        [Display(Name = "TestSuite Name")]
        public string TestSuiteName { get; set; }

        [Required(ErrorMessage="Duration is required!")]
        public Int32 Duration { get; set; }

        [Required(ErrorMessage = "Position is required!")]
        public Int32 Position { get; set; }

        [Required(ErrorMessage = "Competency is required!")]
        public Int32 Competency { get; set; }

        [Required(ErrorMessage = "Primary Tag is required!")]
        [StringLength(500, ErrorMessage = "Tag length should be less than or equal to 500 characters.")]
        [Display(Name="Primary Skill Tags")]
        public List<Tags> PrimaryTags { get; set; }
        
        [StringLength(200, ErrorMessage = "Description length should be less than or equal to 50 characters.")]
        [Display(Name = "Secondary Skill Tags")]
        public List<Tags> SecondaryTag { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public List<Tags> TagList { get; set; }
    }
}
