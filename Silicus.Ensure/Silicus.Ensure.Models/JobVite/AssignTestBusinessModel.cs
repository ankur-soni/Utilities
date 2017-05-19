using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.JobVite
{
    public class AssignTestBusinessModel
    {
        public int RequisitionId { get; set; }
        public int TestSuiteId { get; set; }
        public List<string> CandidatesJson { get; set; }
    }
}
