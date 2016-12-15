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
        [Display(Name = "Test Suite Name")]
        public string TestSuiteName { get; set; }

        [Required(ErrorMessage = "Duration is required!")]
        [Display(Name = "Duration(Min)")]
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

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        public int Userid { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        [Display(Name = "Experience Range")]
        public List<string> ExperienceRangeId { get; set; }

        public string ExperienceRange { get; set; }

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