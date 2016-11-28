using System;
using System.Collections.Generic;
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

        public int EvaluatedMark { get; set; }

        public string FeedBack { get; set; }

        public int Duration { get; set; }

        public int ExtraCount { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<UserTestDetails> UserTestDetails { get; set; }
    }
}
