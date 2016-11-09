using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public int QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        [Required]
        public int AnswerType { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public string SkillTag { get; set; }
        [Required]
        public int Competency { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public bool IsPublishd { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }
        [Required]
        public int ModifiedBy { get; set; }
    }
}
