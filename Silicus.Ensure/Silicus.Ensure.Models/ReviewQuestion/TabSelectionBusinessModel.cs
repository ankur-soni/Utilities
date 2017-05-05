using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.ReviewQuestion
{
   public class TabSelectionBusinessModel
    {
        public int TechnologyId { get; set; }
        public int ReadyForReviewCount { get; set; }
        public int OnHoldCount { get; set; }
        public int RejectedCount { get; set; }
        public int UserId { get; set; }
    }
}
