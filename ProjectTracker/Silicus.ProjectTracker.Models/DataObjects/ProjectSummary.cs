using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectSummary : BaseEntity
    {
        public ProjectSummary()
        {
            IsActive = true;
        }

        [ScaffoldColumn(false)]
        public int ProjectSummaryId { get; set; }

        [ForeignKey("Project")]
        [Required]
        public int ProjectId { get; set; }

        public int SprintId { get; set; }

        public int MileStoneId { get; set; }
              
        public string ReleaseNumber { get; set; }
        
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Feedback Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FeedBack { get; set; }

        public string Remarks { get; set; }

        [Required]
        [ForeignKey("Week")]
        public int WeekId { get; set; }

        public bool IsActive { get; set; }

        public virtual Project Project { get; set; }

        public virtual Week Week { get; set; }
        
    }
}