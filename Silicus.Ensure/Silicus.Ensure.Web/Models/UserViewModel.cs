using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        public bool isActive { get; set; }

        public string Role { get; set; }

        [Required]
        [StringLength(100)]
        [System.Web.Mvc.Remote("IsDuplicateEmail", "User", AdditionalFields = "UserId", ErrorMessage = "Email already name exist !")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required!")]
        public string Gender { get; set; }

        [Display(Name = "Postion")]
        [Required(ErrorMessage = "Postion is required!")]
        public string Position { get; set; }

        [StringLength(2)]
        [Display(Name = "Experience")]
        [Required(ErrorMessage = "Experience is required!")]
        [RegularExpression(@"^[0-9]{2}$", ErrorMessage = "Experience is not a valid")]
        public string Experience { get; set; }

        [Display(Name = "Current Employer")]
        [Required(ErrorMessage = "Current employer is required!")]
        public string CurrentEmployer { get; set; }

        [StringLength(10)]
        [Display(Name = "Primary Mobile Number")]
        [Required(ErrorMessage = "Primary mobile number is required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Primary mobile number is not a valid phone number")]
        public string PrimaryMobileNumber { get; set; }

        [StringLength(10)]
        [Display(Name = "Secondary Mobile Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Secondary mobile number is not a valid phone number")]
        [Required(ErrorMessage = "Secondary mobile number is required!")]
        public string SecondaryMobileNumber { get; set; }

        public string Department { get; set; }

        public string TestStatus { get; set; }

        public int TestSuiteId { get; set; }

        public Guid IdentityUserId { get; set; }

        public string ResumePath { get; set; }

        public IList<Position> PositionList { get; set; }

        public string ErrorMessage { get; set; }

        public string PanelName { get; set; }

        public bool IsAdmin { get; set; }


        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}