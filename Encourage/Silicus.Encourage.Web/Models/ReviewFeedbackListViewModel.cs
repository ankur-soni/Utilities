using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
{
    public class ReviewFeedbackListViewModel
    {
        public int NominationId { get; set; }
        public string DisplayName { get; set; }
        public string NominationTime { get; set; }
        public string AwardName { get; set; }
        public string Intials { get; set; }
        public int Credits { get; set; }
        public bool IsShortlisted { get; set; }
        public bool IsWinner { get; set; }
    }
}