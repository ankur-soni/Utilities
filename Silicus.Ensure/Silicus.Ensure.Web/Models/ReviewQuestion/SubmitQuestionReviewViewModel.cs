using Silicus.Ensure.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.ReviewQuestion
{
    public class SubmitQuestionReviewViewModel
    {
        public int QuestionId { get; set; }
        public QuestionStatus Status { get; set; }
        public string Comment { get; set; }
        public int NextQuestionId { get; set; }
        public int TechnologyId { get; set; }
        public QuestionStatus QuestionStatusType { get; set; }

    }
}