using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectComplaint : BaseEntity
    {
        public ProjectComplaint()
        {
            IsActive = true;
        }

        public int ComplaintId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [Required]
        [ForeignKey("Week")]
        public int WeekId { get; set; }

        public int StatusId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Planned Action is required.")]
        [StringLength(500, MinimumLength = 3)]
        public string PlannedAction { get; set; }

        public string Remarks { get; set; }
      
        public virtual Project Project { get; set; }

        public bool IsActive { get; set; }

        public virtual Week Week { get; set; }

        [UIHint("ComplaintStatusEditor")]
        public StatusList Category { get; set; }
    }
}
