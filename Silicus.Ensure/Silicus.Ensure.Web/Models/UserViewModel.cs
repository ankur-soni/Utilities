using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool isActive { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        //[Required]
        public string Position { get; set; }

        public string Experience { get; set; }
        // [Required]
        public string Department { get; set; }

        public string TestStatus { get; set; }

        public Guid IdentityUserId { get; set; }

        public string ResumePath { get; set; }

        public IList<Position> PositionList { get; set; }

        public string ErrorMessage { get; set; }
    }
}