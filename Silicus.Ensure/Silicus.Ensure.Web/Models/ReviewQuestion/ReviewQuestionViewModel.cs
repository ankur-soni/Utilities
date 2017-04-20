using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.ReviewQuestion
{
    public class ReviewQuestionViewModel
    {
        public QuestionModel QuestionDetails { get; set; }
        public int? NextQuestionId { get; set; }
        public int TechnologyId { get; set; }
    }
}