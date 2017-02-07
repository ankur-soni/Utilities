using System.ComponentModel.DataAnnotations;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class LoginAs
    {
        [Required]
        public string Username { get; set; }
    }
}