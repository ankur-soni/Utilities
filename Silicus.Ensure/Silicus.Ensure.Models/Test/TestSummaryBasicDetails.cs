using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Test
{
   public class TestSummaryBasicDetails
    {
        public int TotalQuestionCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int IncorrectAnswersCount { get; set; }
        public int MarksObtained { get; set; }
        public int MaximumMarks { get; set; }
    }
}
