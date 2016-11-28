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
        [Display(Name = "TestSuite Name")]
        public string TestSuiteName { get; set; }

        [Required(ErrorMessage = "Duration is required!")]
        [Display(Name = "Duration(Min)")]
        public Int32 Duration { get; set; }

        [Required(ErrorMessage = "Position is required!")]
        public Int32 Position { get; set; }

        [Required(ErrorMessage = "Competency is required!")]
        public Int32 Competency { get; set; }

        [Display(Name="Tags")]
        public List<string> PrimaryTagIds { get; set; }

        public List<string> SecondaryTagIds { get; set; }

        public string PrimaryTags { get; set; }

        public string SecondaryTags { get; set; }

        public IList<Tags> TagList { get; set; }

        public Boolean IsCopy { get; set; }

        public string PositionName { get; set; }

        [Display(Name = "Primary Tag")]
        public string PrimaryTagNames { get; set; }

        [Display(Name = "No. Of Objective Questions(Max)")]
        public string ObjectiveQuestionsCount { get; set; }

        [Display(Name = "No. Of Practical Questions")]
        public string PracticalQuestionsCount { get; set; }

        public string Weights { get; set; }

        public int Userid { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public IList<Position> PositionList { get; set; }

        public IList<TestSuiteTagViewModel> Tags { get; set; }
    }

    public class TestSuiteTagViewModel
    { 
        public string TagName { get; set; }

        public int TagId { get; set; }

        public string Weightage { get; set; }

        public int Minutes { get; set; }
    }
}