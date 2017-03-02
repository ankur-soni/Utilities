using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class ReviewerQuestionViewModel : TestDetailsViewModel
    {

        public string Comment { get; set; }

        [Display(Name ="Reviewer Marks")]
        public int? ReviwerMark { get; set; }
        public TestSummaryViewModel TestSummary { get; set; }
        public bool IsCorrect { get; set; }
        public List<string> CandidateAnswers { get; set; }
        public List<string> CorrectAnswers { get; set; }
    }
}