using System;
using System.ComponentModel.DataAnnotations;
namespace Silicus.Ensure.Models.DataObjects
{
    public class UserTestSuite
    {
        [Key]
        public int UserTestSuiteId { get; set; }

        public int TestSuiteId { get; set; }

        public int UserId { get; set; }

        public int ObjectiveCount { get; set; }

        public int PracticalCount { get; set; }

        public int MaxScore { get; set; }

        public int Score { get; set; }

        public string FeedBack { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }
    }
}