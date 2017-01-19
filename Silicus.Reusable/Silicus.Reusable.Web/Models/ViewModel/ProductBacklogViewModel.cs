using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Reusable.Web.Models.ViewModel
{
    public class ProductBacklogViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        [Display(Name = "Area")]
        public string AreaPath { get; set; }
        public string State { get; set; }
        public string Assignee { get; set; }
        [Display(Name = "Time Allocated")]
        public string TimeAllocatedString
        {
            get
            {
                return String.Format("{0}:{1}", ((int)TimeAllocated).ToString().PadLeft(2, '0'), ((int)(TimeAllocated * 60) % 60).ToString().PadLeft(2, '0'));
            }
        }
        [Display(Name = "Time Spent")]
        public string TimeSpentString
        {
            get
            {
                return String.Format("{0}:{1}", ((int)TimeSpent).ToString().PadLeft(2, '0'), ((int)(TimeSpent * 60) % 60).ToString().PadLeft(2, '0'));
            }
        }

        public Double TimeAllocated { get; set; }

        public Double TimeSpent { get; set; }

        public bool IsTaskAssignedToUser { get; set; }

        public string AssignedBy { get; set; }
    }
}