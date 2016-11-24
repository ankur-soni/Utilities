using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class SubmittedTestViewModel
    {
        public int UserId { get; set; }

        public string TestStatus { get; set; }

        public int UserTestSuiteId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Display(Name = "Total Makrs")]
        public int TotalMakrs { get; set; }

        [Display(Name = "Test Suit Name")]
        public string TestSuitName { get; set; }

        [Display(Name = "Postion")]
        public string Postion { get; set; }

        public int ObjectiveQuestionResult { get; set; }

        public int ObjectiveQuestionMarks { get; set; }

        public int TotalMarksObtained { get; set; }

        public string EvaluatedFeedBack { get; set; }

        public List<ObjectiveQuestionList> objectiveQuestionList { get; set; }

        public List<PracticalQuestionList> practicalQuestionList { get; set; }

    }

    public class ObjectiveQuestionList
    {
        public string QuestionDescription { get; set; }

        public string SubmittedAnswer { get; set; }

        public string CorrectAnswer { get; set; }

        public string Result { get; set; }

    }

    public class PracticalQuestionList
    {
        public int QuestionId { get; set; }

        public string QuestionDescription { get; set; }

        public string SubmittedAnswer { get; set; }

        public int Weightage { get; set; }

        public int? EvaluatedMark { get; set; }
    }
}