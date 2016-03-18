using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.Models
{
    public class ChangePasswordModel
    {


        [Required(ErrorMessage = "Please Enter Current Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [RegularExpression(@"^.*(?=.*[A-Za-z0-9][!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password not too strong.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.Web.Mvc.Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public int? UserId { get; set; }
    }
}