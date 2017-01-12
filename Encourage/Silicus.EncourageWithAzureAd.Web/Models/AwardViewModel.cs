namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class LockAwardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int FrequencyId { get; set; }
        public bool Status { get; set; }
        public string ConfigurationKey { get; set; }
    }
}