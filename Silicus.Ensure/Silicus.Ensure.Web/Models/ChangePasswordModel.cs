using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.*[A-Za-z0-9][!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password not too strong.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-type New Password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public int? UserId { get; set; }
    }
}