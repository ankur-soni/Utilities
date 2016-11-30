using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class TestSuiteQuestionModel
    {
        public int QuestionNumber { get; set; }
        public int UserTestDetailId { get; set; }
        public int Id { get; set; }
        public int QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        public int OptionCount { get; set; }
        public int AnswerType { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string Answer { get; set; }
        public int Marks { get; set; }
        
        public bool _IsFirst = false;
        public bool _IsLast = false;
        public bool IsFirst
        {
            get { return _IsFirst; }
            set { _IsFirst = value; }
        }
        public bool IsLast
        {
            get { return _IsLast; }
            set { _IsLast = value; }
        }
    }
}