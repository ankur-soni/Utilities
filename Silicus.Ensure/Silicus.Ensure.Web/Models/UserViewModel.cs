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
        public int UserId { get; set; }

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
        [System.Web.Mvc.Remote("IsDuplicateEmail", "User", AdditionalFields = "UserId", ErrorMessage = "Email already exists.")]
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

        [StringLength(50)]
        [Display(Name = "Client name")]
        [Required(ErrorMessage = "Client name is required.")]
        public string ClientName { get; set; }

        [StringLength(50)]
        [Display(Name = "Technology")]
        [Required(ErrorMessage = "Technology is required.")]
        public string Technology { get; set; }

        [Required(ErrorMessage = "Total experience years are required.")]
        public int? TotalExperienceInYear { get; set; }

        [Required(ErrorMessage = "Total experience months are required.")]
        public int? TotalExperienceInMonth { get; set; }

        [Required(ErrorMessage = "Relevant experience years are required.")]
        public int? RelevantExperienceInYear { get; set; }

        [Required(ErrorMessage = "Relevant experience months are required.")]
        public int? RelevantExperienceInMonth { get; set; }

        [StringLength(50)]
        [Display(Name = "Current company")]
        [Required(ErrorMessage = "Current company is required.")]
        public string CurrentCompany { get; set; }

        [StringLength(50)]
        [Display(Name = "Current title")]
        [Required(ErrorMessage = "Current title is required.")]
        public string CurrentTitle { get; set; }

        public string ResumePath { get; set; }

        public string ResumeName { get; set; }

        public string ResumeDisplayName { get; set; }

        public string Role { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        public string CandidateStatus { get; set; }

        public string TestStatus { get; set; }

        public int TestSuiteId { get; set; }

        public Guid IdentityUserId { get; set; }

        public IList<Position> PositionList { get; set; }

        public string Position { get; set; }

        public string ErrorMessage { get; set; }

        public string PanelName { get; set; }


        public string RecruiterName { get; set; }
        

        public bool IsAdmin { get; set; }

        public int UserApplicationId { get; set; }

        public HttpPostedFileBase ResumeFile { get; set; }

        public HttpPostedFileBase ProfilePhotoFile { get; set; }

        public string ProfilePhotoFilePath { get; set; }

        public bool IsCandidateReappear { get; set; }

        public DateTime ApplicationDate { get; set; }
    }
}