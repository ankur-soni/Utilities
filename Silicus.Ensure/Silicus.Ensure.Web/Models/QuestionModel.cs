using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [DisplayName("Type")]
        public string QuestionType { get; set; }
        [DisplayName("Question")]
        [Required(ErrorMessage = "Question is required.")]
        [AllowHtml]
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
        public bool IsAnsOption1 { get; set; }
        public bool IsAnsOption2 { get; set; }
        public bool IsAnsOption3 { get; set; }
        public bool IsAnsOption4 { get; set; }
        public bool IsAnsOption5 { get; set; }
        public bool IsAnsOption6 { get; set; }
        public bool IsAnsOption7 { get; set; }
        public bool IsAnsOption8 { get; set; }

        public List<string> CorrectAnswer { get; set; }
        [AllowHtml]
        public string Answer { get; set; }
        public string Tags { get; set; }
        [Required(ErrorMessage = "Technology is required.")]
        [Display(Name = "Technology")]
        public int TechnologyId { get; set; }
        [Required(ErrorMessage = "Proficiency is required.")]
        public string ProficiencyLevel { get; set; }
        [Required(ErrorMessage = "Duration in min is required.")]
        [Range(minimum: 1, maximum: 360, ErrorMessage = "Duration must be between 1 to 360.")]
        public int? Duration { get; set; }
        [Required(ErrorMessage = "Marks are required.")]
        [Range(minimum: 1, maximum: 100, ErrorMessage = "Duration must be between 1 to 100.")]
        public int? Marks { get; set; }
        public bool IsPublishd { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set;}

        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }

        public List<Tags> SkillTagsList { get; set; }
        [Required(ErrorMessage = "Skill tag is required.")]
        public List<string> SkillTag { get; set; }
        public int Success { get; set; }
        public bool Edit { get; set; }
        public QuestionStatus Status { get; set; }
        public string StatusName { get; set; }
        public Technology Technology { get; set; }
        public string TechnologyName { get; set; }
        public int? NextQuestionId { get; set; }
        public string ReviewerComment { get; set; }
        public string QuestionTypeString { get; set; }
        public string ProficiencyLevelString { get; set; }
        public string TagsString { get; set; }
        public QuestionStatus? ChangeStatusTo { get; set; }

        public int NoOfTimesIncludedInTest { get; set; }
        public int NoOfTimesCorrectlyAnswered { get; set; }
        public int NoOfTimesIncorrectlyAnswered { get; set; }
        
    }

    public enum ProficiencyLevel
    {
        Beginner,
        Intermediate,
        Expert
    }
}