using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal MarksObtained { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int MaximumMarks { get; set; }
    }
}
