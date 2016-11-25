using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class TestSuiteCandidateModel
    {
        public int UserTestSuiteId { get; set; }
        public int TestSuiteId { get; set; }
        public int UserId { get; set; }
        public int ObjectiveCount { get; set; }
        public int PracticalCount { get; set; }
        public int Duration { get; set; }
        public int TotalCount { get; set; }
        public int DurationInMin { get; set; }
    }
}