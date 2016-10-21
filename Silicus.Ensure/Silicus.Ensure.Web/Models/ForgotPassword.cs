using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "We need your email address to send you a link to reset your password!")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage= "Not a valid email address")]
        public string Email { get; set; }
    }
}