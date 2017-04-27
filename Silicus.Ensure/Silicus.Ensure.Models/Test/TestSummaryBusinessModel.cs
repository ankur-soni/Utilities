using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Test
{
    public class TestSummaryBusinessModel
    {
        public TestSummaryBasicDetails Practical { get; set; }
        public TestSummaryBasicDetails Objective { get; set; }
        public int TotalMaximumMarks { get; set; }
        public decimal TotalObtainedMarks { get; set; }
        public decimal Percentage { get; set; }
        public int TimeAllotted { get; set; }
        public int TimeTaken { get; set; }
    }
}
