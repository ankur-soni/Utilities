using Silicus.Ensure.Models.CustomValidations;
using System;
using System.Collections;
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

        [Display(Name = "No. Of Objective Questions")]
        public string ObjectiveQuestionsCount { get; set; }

        [Display(Name = "No. Of Practical Questions")]
        public string PracticalQuestionsCount { get; set; }

        [Required(ErrorMessage = "Position is required!")]
        public Int32 Position { get; set; }

        [Required(ErrorMessage = "Competency is required!")]
        public Int32 Competency { get; set; }

        public string PrimaryTags { get; set; }

        public string Weights { get; set; }

        public string Proficiency { get; set; }

        public int Status { get; set; }
        
        public bool IsDeleted { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}
