using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class TestViewModel
    {
        public int TotalQuestionCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int IncorrectAnswersCount { get; set; }
        public int MarksObtained { get; set; }
        public int MaximumMarks { get; set; }
    }
}