using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Awards = new List<AwardViewModel>();
            NominationList = new List<NominationListViewModel>();
        }
        public List<string> userRoles { get; set; }
        public List<AwardViewModel> Awards { get; set; }
        public List<NominationListViewModel> NominationList { get; set; }
    }

    public class AwardViewModel
    {
        public int AwardId { get; set; }
        public string AwardTitle { get; set; }
        public string AwardCode { get; set; }
    }
}