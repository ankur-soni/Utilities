using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ShortlistedNominationViewModel
    {
        public ShortlistedNominationViewModel()
        {
            ReviewFeedbackList = new List<ReviewFeedbackListViewModel>();
            Awards = new List<LockAwardViewModel>();
        }

        public List<ReviewFeedbackListViewModel> ReviewFeedbackList { get; set; }
        public List<LockAwardViewModel> Awards { get; set; }
    }
}