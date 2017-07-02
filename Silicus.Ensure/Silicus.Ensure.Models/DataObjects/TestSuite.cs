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

        [Required(ErrorMessage = "Test suite name is required.")]
        [StringLength(50, ErrorMessage = "Test suite name length should be less than or equal to 50 characters.")]
        [Display(Name = "Test suite name")]
        public string TestSuiteName { get; set; }

        [Required(ErrorMessage="Duration is required.")]
        public Int32 Duration { get; set; }        

        //[Required(ErrorMessage = "Position is required.")]
        //public Int32? Position { get; set; }

        [Required(ErrorMessage = "Competency is required.")]
        public Int32 Competency { get; set; }

        public string PrimaryTags { get; set; }

        public string Weights { get; set; }

        public string Proficiency { get; set; }

        public int Status { get; set; }

        public string ProjectName { get; set; }

        public int FromExperience { get; set; }

        public int ToExperience { get; set; }

        public int OptionalQuestion { get; set; }

        public int PracticalQuestion { get; set; }

        [Display(Name = "Special Instruction")]
        public string SpecialInstruction { get; set; }
                
        public bool IsDeleted { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Is External")]
        public bool? IsExternal { get; set; }
    }
}
