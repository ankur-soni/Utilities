using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class ReviewerQuestionViewModel:TestSuiteQuestionModel
    {
        public string CorrectAnswer { get; set; }
        public string Comment { get; set; }
        public int Marks { get; set; }
    }
}