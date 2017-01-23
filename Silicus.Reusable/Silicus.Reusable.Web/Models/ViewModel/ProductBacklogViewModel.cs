using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Reusable.Web.Models.ViewModel
{
    public class ProductBacklogViewModel
    {
        public int Id { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        [Display(Name = "Area")]
        public string AreaPath { get; set; }
        public string State { get; set; }
        public string AssigneeDisplayName { get; set; }

        public string AssigneeEmail { get; set; }
      
        [Display(Name = "Time Allocated")]
        public Double TimeAllocated { get; set; }
        [Display(Name = "Time Spent")]
        public Double TimeSpent { get; set; }

        public bool IsTaskAssignedToUser { get; set; }

        public string AssignedBy { get; set; }
    }
}