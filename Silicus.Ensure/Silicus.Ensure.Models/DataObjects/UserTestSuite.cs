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

        public int MaxScore { get; set; }

        public int Score { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
