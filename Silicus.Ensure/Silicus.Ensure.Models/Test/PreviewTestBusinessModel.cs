using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Test
{
    public class PreviewTestBusinessModel
    {
        public TestSuite TestSuite { get; set; }
        public int ViewerId { get; set; }
        public int? CandidateId { get; set; }
        public string QuestionIds { get; set; }
    }
}
