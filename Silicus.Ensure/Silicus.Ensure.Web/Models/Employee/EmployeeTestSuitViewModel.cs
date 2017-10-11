using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Web.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Employee
{
    public class EmployeeTestSuitViewModel
    {
        public int EmployeeTestSuiteId { get; set; }
        public int TestSuiteId { get; set; }
        public string TestSuitName { get; set; }
        public int EmployeeId { get; set; }
        public int ObjectiveCount { get; set; }
        public int PracticalCount { get; set; }
        public int Duration { get; set; }
        public int RemainingTime { get; set; }
        public int? MarksObtained { get; set; }
        public int TotalQuestionCount { get; set; }
        public int DurationInMin { get; set; }
        public DateTime? AttemptDate { get; set; }

        public int ExtraCount { get; set; }
        public string SpecialInstruction { get; set; }
        public QuestionNavigationViewModel NavigationDetails { get; set; }
        public CandidateInfoViewModel CandidateInfo { get; set; }
        public TestSummaryViewModel TestSummary { get; set; }
        public CandidateStatus StatusId { get; set; }
    }


    public class EmployeeTestResultViewModel
    {
        public int UserId { get; set; }

        public int EmployeeTestSuiteId { get; set; }

        public string EmployeeId { get; set; }

        public string EmpName { get; set; }

        public int MaxScore { get; set; }

        public decimal? MarksObtained { get; set; }

        public int TestSuitId { get; set; }

        public string TestSuitName { get; set; }

        public string AssignedReviewerName { get; set; }
        public string ActualReviewerName { get; set; }

        public DateTime? ReviewDate { get; set; }
        public DateTime? AttemptDate { get; set; }

        public int StatusId { get; set; }
    }
}