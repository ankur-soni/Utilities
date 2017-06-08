using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class EmployeeTestDetails
    {
        [Key]
        public int TestDetailId { get; set; }

        public int QuestionId { get; set; }

        public string Answer { get; set; }

        public int? Mark { get; set; }

        public int? MarkGivenBy { get; set; }

        public string MarkGivenByName { get; set; }

        public bool IsViewedOnly { get; set; }

        public DateTime? MarkGivenDate { get; set; }

        public string ReviwerComment { get; set; }

        public virtual EmployeeTestSuite EmployeeTestSuite { get; set; }
    }
}
