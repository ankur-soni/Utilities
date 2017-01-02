using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class TestSuiteViewModel
    {
        public int TestSuiteId { get; set; }

        [Required(ErrorMessage = "Test-Suite Name is required!")]
        [StringLength(50, ErrorMessage = "Test-Suite name length should be less than or equal to 50 characters.")]
        [RegularExpression(@"^\d*[a-zA-Z][a-zA-Z0-9]*$",
            ErrorMessage = "Name should not contain any special character and should contain atleast one alphabet")]
        [Display(Name = "Test Suite Name")]
        public string TestSuiteName { get; set; }

        [Required(ErrorMessage = "Duration is required!")]
        [Display(Name = "Duration(Min)")]
        [Range(0, 360, ErrorMessage = "Enter number between 1-360")]
        public Int32 Duration { get; set; }

        [Required(ErrorMessage = "Position is required!")]
        [Display(Name = "Position")]
        public Int32 Position { get; set; }

        [Required(ErrorMessage = "Competency is required!")]
        [Display(Name = "Overall Proficiency")]
        public Int32 Competency { get; set; }

        [Display(Name = "Tags")]
        public List<string> PrimaryTagIds { get; set; }

        public List<string> SecondaryTagIds { get; set; }

        public string PrimaryTags { get; set; }

        public string SecondaryTags { get; set; }

        public IList<Tags> TagList { get; set; }

        public Boolean IsCopy { get; set; }

        public string PositionName { get; set; }

        [Display(Name = "Primary Tag")]
        public string PrimaryTagNames { get; set; }

        public string Weights { get; set; }

        public string Proficiency { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        public int? FromExperience { get; set; }

        public int? ToExperience { get; set; }

        [Range(1, 100, ErrorMessage = "Enter numbers from 1-100")]
        [Required(ErrorMessage = "Optional question is required!")]
        [Display(Name = "Optional question")]
        public int OptionalQuestion { get; set; }

        [Range(1, 100, ErrorMessage = "Enter numbers from 1-100")]
        [Required(ErrorMessage = "Practical question is required!")]
        [Display(Name = "Practical question")]
        public int PracticalQuestion { get; set; }

        public int Userid { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        //[Display(Name = "Experience Range")]
        //public List<string> ExperienceRangeId { get; set; }

        //public string ExperienceRange { get; set; }

        public IList<Position> PositionList { get; set; }

        public IList<TestSuiteTagViewModel> Tags { get; set; }

        public bool UserInRole { get; set; }
    }

    public class TestSuiteTagViewModel
    {
        public string TagName { get; set; }

        public int TagId { get; set; }

        public int Weightage { get; set; }

        public int Proficiency { get; set; }

        public int Minutes { get; set; }
    }
}