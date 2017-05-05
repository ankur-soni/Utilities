using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.ReviewQuestion
{
    public class TabSelectionViewModel
    {
        public int TechnologyId { get; set; }
        public int? QuestionId { get; set; }
        public bool IsReadyForReview { get; set; }
        public bool IsApproved { get; set; }
        public bool IsOnHold { get; set; }
        public bool IsRejected { get; set; }
        public int ReadyForReviewCount { get; set; }
        public int OnHoldCount { get; set; }
        public int RejectedCount { get; set; }
    }
}