using Silicus.Ensure.Models.Constants;
using System;
using System.Collections.Generic;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Question
    {
        public int Id { get; set; }
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
        public string Tags { get; set; }
        public int ProficiencyLevel { get; set; }
        public int TechnologyId { get; set; }
        public int Duration { get; set; }
        public int Marks { get; set; }
        public bool IsPublishd { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public QuestionStatus Status { get; set; }
        public virtual ICollection<QuestionStatusDetails> QuestionStatusDetails { get; set; }
        public virtual Technology Technology { get; set; }
    }
}
