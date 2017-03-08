using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class QuestionDetailsViewModel
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int? UserTestSuiteId { get; set; }
        public int? UserTestDetailId { get; set; }
        public string Answer { get; set; }
        public int QuestionType { get; set; }
        public int? Marks { get; set; }
        public string Comment { get; set; }
    }
}