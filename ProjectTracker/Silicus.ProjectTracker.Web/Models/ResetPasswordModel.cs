using System.ComponentModel.DataAnnotations;

namespace Silicus.ProjectTracker.Web.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$",ErrorMessage = "Password must contain at least one special character.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserType { get; set; }
    }
}