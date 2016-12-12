using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class TestSuiteViewQuesModel
    {
        public int QuestionNumber { get; set; }     
        public int Duration { get; set; }
        public string TestSuiteName { get; set; }
        public int Proficiency { get; set; }
        public int ObjectiveCount { get; set; }
        public int PracticalCount { get; set; }
        public int MaxScore { get; set; }
        public string ErrorMessage{ get; set; }

        public virtual List<TestSuiteQuestion> TestSuiteQuestion { get; set; }
    }

    public class TestSuiteQuestion
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public int QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        public int OptionCount { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string Answer { get; set; }
        public string CorrectAnswer { get; set; }
        public int Marks { get; set; }
    }
}