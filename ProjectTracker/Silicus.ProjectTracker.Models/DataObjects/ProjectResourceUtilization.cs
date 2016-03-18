using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectResourceUtilization : BaseEntity
    {
        public ProjectResourceUtilization()
        {
            IsActive = true;
        }

        [Key]
        public int ProjectResourceId { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        
        [Required]
        public int WeekId { get; set; }
              
        [Required(ErrorMessage = "The Role Name is required.")]
        [StringLength(100, MinimumLength = 3)]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "The Resource Name is required.")]
        [StringLength(100, MinimumLength = 3)]
        public string ResourceName { get; set; }

        [Required]
        public int AvailableEfforts { get; set; }

        [Required]
        public int ConsumedEfforts { get; set; }

        public string Status { get; set; }
        
        public bool IsActive { get; set; }
               
    }
}
