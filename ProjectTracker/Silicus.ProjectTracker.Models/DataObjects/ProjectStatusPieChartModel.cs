using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.ProjectTracker.Web.Models
{
    public class ProjectStatusPieChartModel
    {
        public string project { get; set; }
        public double percentage { get; set; }
        public string color { get; set; }
        public string message { get; set; }
    }
}

