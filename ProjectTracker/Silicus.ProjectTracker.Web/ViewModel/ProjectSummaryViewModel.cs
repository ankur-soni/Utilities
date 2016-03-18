using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectSummaryViewModel : BaseEntity
    {
        public int ProjectSummaryId { get; set; }

        public int ProjectId { get; set; }

        public string ReleaseNumber { get; set; }
        
        public int SprintId { get; set; }

        public int MileStoneId { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Feedback Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FeedBack { get; set; }

        public string Remarks { get; set; }

        public int WeekId { get; set; }
               
        public  IList<Sprints>Sprints  { get; set; }

        public string SprintName { get; set; }

        public IList<Sprints> Milestones { get; set; }

        public string MilestoneName { get; set; }

    }
}