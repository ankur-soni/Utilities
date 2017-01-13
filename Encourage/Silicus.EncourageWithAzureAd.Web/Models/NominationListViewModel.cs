using System.ComponentModel;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class NominationListViewModel
    {
        public int Id { get; set; }
        [DisplayName("Winner")]
        public string DisplayName { get; set; }
        [DisplayName("Nominated Date")]
        public string NominationTime { get; set; }
        [DisplayName("Award")]
        public string AwardName { get; set; }
        public string Intials { get; set; }
        public int ManagerId { get; set; }
        public bool IsLocked { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool? IsDrafted { get; set; }
        [DisplayName("Feedback")]
        public string AwardComment { get; set; }
        public string AwardFrequencyCode { get; set; }

    }
}