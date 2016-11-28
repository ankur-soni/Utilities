using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        public int AnswerType { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public List<string> CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public string Tag { get; set; }
        public string Competency { get; set; }
        public int Duration { get; set; }
        public int Marks { get; set; }
        public bool IsPublishd { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public List<Tags> SkillTagsList { get; set; }
        public List<string> SkillTag { get; set; }

        private int _success = 0;
        private bool _edit = false;
        public int Success { get { return _success; } set { _success = value; } }
        public bool Edit { get { return _edit; } set { _edit = value; } }
    }
}