using Silicus.Ensure.Models.Test;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models.Test
{
    public class TestSummaryViewModel
    {
        public TestSummaryBasicDetails Practical { get; set; }
        public TestSummaryBasicDetails Objective { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int TotalMaximumMarks { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal TotalObtainedMarks { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Percentage { get; set; }
        public int TimeAllotted { get; set; }
        public int TimeTaken { get; set; }
    }
}