namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ReviewFeedbackListViewModel
    {
        public int NominationId { get; set; }
        public string DisplayName { get; set; }
        public string NominationTime { get; set; }
        public string AwardName { get; set; }
        public string Intials { get; set; }
        public decimal Credits { get; set; }
        public bool IsShortlisted { get; set; }
        public bool IsWinner { get; set; }
        public bool IsHistorical { get; set; }
        public bool IsAwardLocked { get; set; }
        public int NumberOfReviews { get; set; }
        public decimal AverageCredits { get; set; }
        public int NominatedMonth { get; set; }
        public string AwardFrequencyCode { get; set; }
    }
}