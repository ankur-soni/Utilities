using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectCreateViewModel
    {
        public ProjectCreateViewModel()
        {
            Employees = new List<Employee>();
            SkillSets = new List<SkillSet>();
        }
        
        public int ProjectId { get; set; }

        [Display(Name = "Project Name *")]
        [Required(ErrorMessage = "Project Name can't be blank")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Code *")]
        [Required(ErrorMessage = "Project Code can't be blank")]
        public string ProjectCode { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Project Type *")]
        [Range(0, int.MaxValue, ErrorMessage = "Select a Project Type")]
        public ProjectType? ProjectType { get; set; }

        [Display(Name = "Engagement Type*")]
        [Range(0, int.MaxValue, ErrorMessage = "Select a Engagement Type")]
        public EngagementType? EngagementType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Select Status")]
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
        [Range(0, int.MaxValue, ErrorMessage = "Select project manager")]
        public int? ProjectManagerId { get; set; }

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }

        public int[] skillSetId { get; set; }
        public virtual ICollection<SkillSet> SkillSets { get; set; }
                
        public int[] EmployeeIds { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}