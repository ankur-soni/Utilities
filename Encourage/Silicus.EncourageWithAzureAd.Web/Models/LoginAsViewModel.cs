using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class LoginAsViewModel
    {
        [Required]
        public string Username { get; set; }
        public SelectList UsersWithMultipleRoles { get; set; }
    }
}