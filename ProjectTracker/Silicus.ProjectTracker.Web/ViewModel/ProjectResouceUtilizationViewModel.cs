
using System;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using System.ComponentModel.DataAnnotations;
using Silicus.ProjectTracker.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectResourceUtilizationViewModel : BaseEntity
    {
        public int ProjectResourceId { get; set; }

        public int ProjectId { get; set; }

        public int WeekId { get; set; }

        [Required(ErrorMessage = "The Role Name is required.")]
        [StringLength(100, MinimumLength = 3)]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "The Role Name is required.")]
        [StringLength(100, MinimumLength = 3)]
        public string ResourceName { get; set; }

        [Required]
        public int AvailableEfforts { get; set; }

        [Required]
        public int ConsumedEfforts { get; set; }

        public string Status { get; set; }
    }
}