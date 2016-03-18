using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class Week
    {
        public int WeekId { get; set; }

        public int WeekNumber { get; set; }

        public int Year { get; set; }

        public string Text { get; set; }
       
    }
}