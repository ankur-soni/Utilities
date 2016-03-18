using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class Project : BaseEntity
    {
        public Project()
        {
            IsActive = true;
        }

        [ScaffoldColumn(false)]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} cannot exceed more than {1} characters.", MinimumLength = 1)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} cannot exceed more than {1} characters.", MinimumLength = 1)]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Planned End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlannedEndDate { get; set; }
                                
        public bool IsActive { get; set; }

        public virtual ICollection<ProjectSummary> ProjectSummaries { get; set; }

        public virtual ICollection<ProjectMapping> ProjectMapping { get; set; }

        public virtual ICollection<ProjectStatus> ProjectStatus { get; set; }

        public virtual ICollection<ProjectResourceUtilization> ProjectResources { get; set; }
    }
}