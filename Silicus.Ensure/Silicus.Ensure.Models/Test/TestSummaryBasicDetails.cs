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

    public class TestSuiteViewQuesBussinessModel
    {
        public int QuestionNumber { get; set; }
        public int Duration { get; set; }
        public string TestSuiteName { get; set; }
        public int Proficiency { get; set; }
        public int ObjectiveCount { get; set; }
        public int PracticalCount { get; set; }
        public int MaxScore { get; set; }
        public string ErrorMessage { get; set; }
        
    }
}
