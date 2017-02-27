using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Test
{
   public class TestDetailsBusinessModel
    {
       public int QuestionId { get; set; }
        public int? PreviousQuestionId { get; set; }
        public int? NextQuestionId { get; set; }
        public int TestDetailId { get; set; }
        public int QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        public int AnswerType { get; set; }
        public int OptionCount { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public int Marks { get; set; }
        public int? ReviwerMark { get; set; }
        public string Comment { get; set; }
        public int DisplayQuestionNumber { get; set; }
        public bool IsViewedOnly { get; set; }
        public bool IsAnswered { get; set; }
    }
}
