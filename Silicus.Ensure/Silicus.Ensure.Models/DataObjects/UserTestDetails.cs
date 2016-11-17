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

        public string Answer { get; set; }

        public int? Mark { get; set; }

        public int? MarkGivenBy { get; set; }

        public DateTime? MarkGivenDate { get; set; }

        public virtual UserTestSuite UserTestSuite { get; set; }
    }
}
