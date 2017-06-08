using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class UserWinningHistoryViewModel
    {
        public int AwardId { get; set; }
        public string AwardMonth { get; set; }
        public string AwardYear { get; set; }
        public string ManagerComment { get; set; }
        public decimal AverageScore { get; set; }
        public string AdminComment { get; set; }
        public int NominationId { get; set; }
    }
}