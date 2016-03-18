using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectStatusPieChartViewModel
    {       
            public string project { get; set; }
            public double percentage { get; set; }
            public string color { get; set; }
            public string message { get; set; }

    }
}