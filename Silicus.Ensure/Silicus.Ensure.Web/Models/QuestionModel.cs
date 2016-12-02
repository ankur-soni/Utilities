using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;

namespace Silicus.Ensure.Web.Models
{
    public class QuestionModel
    {
        public QuestionModel()
        {
            Edit = false;
            Success = 0;
        }
        public int Id { get; set; }
        public string QuestionType { get; set; }
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
        public List<string> CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public string Tag { get; set; }
        public string ProficiencyLevel { get; set; }
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
       
        public int Success { get; set; }
        public bool Edit { get; set; }
    }
}