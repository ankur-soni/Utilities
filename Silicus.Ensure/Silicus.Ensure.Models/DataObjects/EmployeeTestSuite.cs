using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Silicus.Ensure.Models.DataObjects
{
    public class EmployeeTestSuite
    {
        [Key]
        public int EmployeeTestSuiteId { get; set; }

        public int TestSuiteId { get; set; }

        public int EmployeeId { get; set; }

        public string CandidateID { get; set; }

        public int ObjectiveCount { get; set; }

        public int? ReviewerId { get; set; }

        public DateTime? ReviewDate { get; set; }

        public int PracticalCount { get; set; }

        public int MaxScore { get; set; }

        public int EvaluatedMark { get; set; }

        public string FeedBack { get; set; }

        public int Duration { get; set; }

        public int RemainingTime { get; set; }

        public int ExtraCount { get; set; }

        public bool IsActive { get; set; }

        public int StatusId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? AttemptDate { get; set; }

        public virtual ICollection<EmployeeTestDetails> EmployeeTestDetails { get; set; }
    }
}
