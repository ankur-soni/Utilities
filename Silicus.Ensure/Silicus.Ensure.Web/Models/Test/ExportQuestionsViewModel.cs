using System.Collections.Generic;

namespace Silicus.Ensure.Web.Models.Test
{
    public class ExportQuestionsViewModel
    {
        public CandidateInfoViewModel CandidateInfo { get; set; }
        public List<TestDetailsViewModel> Practical { get; set; }
        public List<TestDetailsViewModel> Objective { get; set; }
    }
}