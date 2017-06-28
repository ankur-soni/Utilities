
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Web.Models.Test;
namespace Silicus.Ensure.Web.Models
{
    public class TestSuiteCandidateModel
    {
        public int UserTestSuiteId { get; set; }
        public int TestSuiteId { get; set; }
        public int UserId { get; set; }
        public int ObjectiveCount { get; set; }
        public int PracticalCount { get; set; }
        public int Duration { get; set; }
        public int RemainingTime { get; set; }
        public int TotalQuestionCount { get; set; }
        public int DurationInMin { get; set; }
        public int ExtraCount { get; set; }
        public string SpecialInstruction { get; set; }
        public QuestionNavigationViewModel NavigationDetails { get; set; }
        public CandidateInfoViewModel CandidateInfo { get; set; }
        public TestSummaryViewModel TestSummary { get; set; }
        public CandidateStatus CandidateStatus { get; set; }
        public string ProfilePhotoFilePath { get; set; }
    }

    public class TestSuiteEmployeeModel
    {
        public int EmployeeTestSuiteId { get; set; }
        public int TestSuiteId { get; set; }
        public int EmployeeId { get; set; }
        public string CandidateId { get; set; }
        public int ObjectiveCount { get; set; }
        public int PracticalCount { get; set; }
        public int Duration { get; set; }
        public int RemainingTime { get; set; }
        public int TotalQuestionCount { get; set; }
        public int DurationInMin { get; set; }
        public int ExtraCount { get; set; }
        public string SpecialInstruction { get; set; }
        public QuestionNavigationViewModel NavigationDetails { get; set; }
       // public CandidateInfoViewModel CandidateInfo { get; set; }
        public TestSummaryViewModel TestSummary { get; set; }
        public CandidateStatus TestStatus { get; set; }
        public string ProfilePhotoFilePath { get; set; }
    }
}//