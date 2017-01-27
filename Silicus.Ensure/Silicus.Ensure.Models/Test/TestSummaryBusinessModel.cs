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
        public int TotalObtainedMarks { get; set; }
        public decimal Percentage { get; set; }
    }
}
