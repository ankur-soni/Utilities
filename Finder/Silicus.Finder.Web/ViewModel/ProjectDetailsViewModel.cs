using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectDetailsViewModel
    {
        [Display(Name = "Project Id")]
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        public string Description { get; set; }

        [Display(Name = "Project Type")]
        public ProjectType ProjectType { get; set; }

        [Display(Name = "Engagement Type")]
        public EngagementType EngagementType { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Expected End Date")]
        public DateTime? ExpectedEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        public DateTime? ActualEndDate { get; set; }

        [Display(Name = "Engagement Manager")]
        public int? EngagementManagerId { get; set; }

        [Display(Name = "Project Manager")]
        public int? ProjectManagerId { get; set; }

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }

        public virtual ICollection<ProjectSkillSetDetailsViewModel> SkillSets { get; set; }

        public virtual ICollection<ProjectEmployeeDetailsViewModel> Employees { get; set; }
    }
}