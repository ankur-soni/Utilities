using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class UserTestDetails
    {
        [Key]
        public int TestDetailId { get; set; }

        public int UserTestSuiteId { get; set; }

        public int QuestionId { get; set; }

        public int Score { get; set; }
    }
}
