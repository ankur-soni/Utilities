using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class TempPreviewTest
    {
        public int TempPreviewTestId { get; set; }

        public int? CandidateId { get; set; }

        public int? TestSuiteId { get; set; }

        public int ViewerId { get; set; }

        public string QuestionIds { get; set; }
    }
}
