using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class Dashboard
    {
        public Dashboard()
        {
            Awards = new List<DashboardAwardsAndNominations>();
        }
        public List<string> userRoles { get; set; }

        public List<DashboardAwardsAndNominations> Awards { get; set; }
    }

    public class DashboardAwardsAndNominations
    {
        public int AwardId { get; set; }

        public string AwardTitle { get; set; }

        public List<NominationListViewModel> NominationList { get; set; }
    }
}