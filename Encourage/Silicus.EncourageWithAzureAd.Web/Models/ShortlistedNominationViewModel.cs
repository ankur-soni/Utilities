using System.Collections.Generic;

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