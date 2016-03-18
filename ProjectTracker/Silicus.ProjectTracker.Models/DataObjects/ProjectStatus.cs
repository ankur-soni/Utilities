using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectStatus : BaseEntity
    {
        [Key]
        public int ProjectStatusId { get; set; }

        //[Required]
        [StringLength(1000, ErrorMessage = "The {0} cannot exceed more than {1} characters.", MinimumLength = 1)]
        [Display(Name = "Summary Of Project Status")]
        public string ProjectSummary { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public int WeekId { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }                
    }
}