using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            IsActive = true;
        }

        [ScaffoldColumn(false)]
        public int ProjectId { get; set; }

        public int ProjectStatusId { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} cannot exceed more than {1} characters.", MinimumLength = 1)]
        [Display(Name = "Project Name")]
   
        public string ProjectName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} cannot exceed more than {1} characters.", MinimumLength = 1)]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        //[Required]
        [StringLength(1000, ErrorMessage = "The {0} cannot exceed more than {1} characters.", MinimumLength = 1)]
        [Display(Name = "Summary Of Project Status")]
        public string ProjectSummary { get; set; }
           
        public int StatusId { get; set; }
      
        [Display(Name = "Start Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
 
        public DateTime StartDate { get; set; }

        [Display(Name = "Planned End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
      
        public DateTime PlannedEndDate { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<SelectListItem> Status { get; set; }

        public string UserName { get; set; }
    }
}
