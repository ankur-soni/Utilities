using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectComplaintViewModel : BaseEntity
    {
        public int ComplaintId { get; set; }

        public int ProjectId { get; set; }

        public int WeekId { get; set; }

        public int StatusId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, MinimumLength = 3)]
        public string PlannedAction { get; set; }

        public string Remarks { get; set; }

        [UIHint("ComplaintStatusEditor")]
        public StatusList ComplaintStatus { get; set; }

        public string StatusName { get; set; }
       
    }
}