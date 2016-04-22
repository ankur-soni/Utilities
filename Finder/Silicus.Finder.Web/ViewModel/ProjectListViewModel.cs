using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectListViewModel
    {
        [Key]
        [Display(Name = "Code/ID")]
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Type")]
        public string ProjectType { get; set; }

        [Display(Name = "Engagement Type")]
        public EngagementType EngagementType { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Expected End Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedEndDate { get; set; }

        [Display(Name = "Engagement Manager")]
        public int? EngagementManagerId { get; set; }

        [Display(Name = "Project Manager")]
        public int? ProjectManagerId { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}