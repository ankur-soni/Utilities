using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class TestSummaryViewModel
    {
        public TestViewModel Practical { get; set; }
        public TestViewModel Objective { get; set; }
        public int TotalMaximumMarks { get; set; }
        public int TotalObtainedMarks { get; set; }
        public int Percentage { get; set; }
    }
}