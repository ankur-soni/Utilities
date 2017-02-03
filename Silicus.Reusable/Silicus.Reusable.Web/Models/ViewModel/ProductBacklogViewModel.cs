using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Reusable.Web.Models.ViewModel
{
    public class ProductBacklogViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Area { get; set; }
        [Display(Name ="Status")]
        [Required]
        public string State { get; set; }
        [Display(Name = "Assignee")]
        public string AssigneeDisplayName { get; set; }
        public string AssigneeEmail { get; set; }
        [Display(Name = "Time Allocated")]
        public Double? TimeAllocated { get; set; }
        [Display(Name = "Time Spent")]
        public Double? TimeSpent { get; set; }
        [Display(Name = "Time Remaining")]
        public Double? TimeRemaining
        {
            get
            {
                return TimeAllocated - TimeSpent;
            }
        }
        public bool IsTaskAssignedToUser { get; set; }
        [Display(Name = "Assigned By")]
        public string AssignedBy { get; set; }
        [Required]
        [Display(Name = "Created On")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Last Updated")]
        public DateTime? ChangedDate { get; set; }
    }
}