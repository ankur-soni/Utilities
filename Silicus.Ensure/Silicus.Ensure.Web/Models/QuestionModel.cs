using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        public string AnswerType { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public List<string> CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public List<string> SkillTag { get; set; }
        public string Tag { get; set; }
        public string Competency { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        public List<Tags> Tags { get; set; }

        public int Success { get; set; }
        public bool Edit { get; set; }
    }

    public class Skills
    {
        public string Skill { get; set; }
        public string Value { get; set; }
    }
}