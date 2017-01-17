using System.Collections.Generic;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Awards = new List<AwardViewModel>();
            NominationList = new List<NominationListViewModel>();
        }
        public List<string> UserRoles { get; set; }
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