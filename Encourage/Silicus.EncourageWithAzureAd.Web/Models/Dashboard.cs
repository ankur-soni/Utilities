using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class Dashboard
    {
        public List<string> userRoles { get; set; }
        public List<NominationListViewModel> NominationList { get; set; }
    }
}