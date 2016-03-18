using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectEditViewModel
    {       
        public int ProjectId { get; set; }

        [Display(Name = "Project Name *")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Code *")]
        public string ProjectCode { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Project Type *")]
        public ProjectType? ProjectType { get; set; }

        [Display(Name = "Engagement Type *")]
        public EngagementType? EngagementType { get; set; }

        [Display(Name = "Status *")]
        public Status Status { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Expected End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpectedEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ActualEndDate { get; set; }
        
        [Display(Name = "Engagement Manager")]
        public int? EngagementManagerId { get; set; }

        [Display(Name = "Project Manager *")]
        public int? ProjectManagerId { get; set; }

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }

        public int[] skillSetId { get; set; }
        public virtual ICollection<ProjectSkillSetDetailsViewModel> SkillSets { get; set; }

        public int[] EmployeeIds { get; set; }
        public virtual ICollection<ProjectEmployeeDetailsViewModel> Employees { get; set; }
    }
}