using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string JobViteId { get; set; }

        [StringLength(50)]
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [CompareAttribute("NewPassword", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        [Display(Name = "Requisition id")]
        [Required(ErrorMessage = "Requisition id is required.")]
        public string RequisitionId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100)]
        //[System.Web.Mvc.Remote("IsDuplicateEmail", "User", AdditionalFields = "UserId", ErrorMessage = "Email already exists.")]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$",
     ErrorMessage = "Please enter correct email.")]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Current location")]
        [Required(ErrorMessage = "Current location is required.")]
        public string CurrentLocation { get; set; }

        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Date of birth is required.")]
        public string DOB { get; set; }

        [StringLength(10)]
        [Display(Name = "Contact number")]
        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Contact number is not a valid phone number.")]
        public string ContactNumber { get; set; }
        
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        public string CandidateStatus { get; set; }

        public string TestStatus { get; set; }

        public Guid IdentityUserId { get; set; }

        // public IList<Position> PositionList { get; set; }

        public string Position { get; set; }

        public string ErrorMessage { get; set; }

        public string PanelName { get; set; }

        public string RecruiterName { get; set; }



      

    }
}