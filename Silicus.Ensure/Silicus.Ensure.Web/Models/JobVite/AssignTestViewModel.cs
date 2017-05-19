using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.JobVite
{
    public class AssignTestViewModel
    {
        public int RequisitionId { get; set; }
        public int TestSuiteId { get; set; }
        public List<string> CandidatesJson { get; set; }
    }
}