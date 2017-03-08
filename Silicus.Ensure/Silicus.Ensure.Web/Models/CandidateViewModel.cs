using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class CandidateViewModel
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

        [StringLength(50)]
        [Display(Name = "Requisition id")]
        [Required(ErrorMessage = "Requisition id is required.")]
        public string RequisitionId { get; set; }

        [Required]
        [StringLength(100)]
        [System.Web.Mvc.Remote("IsDuplicateEmail", "User", AdditionalFields = "UserId", ErrorMessage = "Email already name exist.")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email.")]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Current location")]
        [Required(ErrorMessage = "Current location is required.")]
        public string CurrentLocation { get; set; }

        [Display(Name = "Current location")]
        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DOB { get; set; }

        [StringLength(10)]
        [Display(Name = "Contact number")]
        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Contact number is not a valid phone number.")]
        public string ContactNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Client name")]
        [Required(ErrorMessage = "ClientName is required.")]
        public string ClientName { get; set; }

        [StringLength(50)]
        [Display(Name = "Technology")]
        [Required(ErrorMessage = "Technology is required.")]
        public string Technology { get; set; }

        [Display(Name = "Total experience")]
        [Required(ErrorMessage = "Total experience is required.")]
        public string TotalExperience { get; set; }

        [Display(Name = "Relevant experience")]
        [Required(ErrorMessage = "Relevant experience is required.")]
        public string RelevantExperience { get; set; }

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

        public string Role { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        public string CandidateStatus { get; set; }

        public string TestStatus { get; set; }

        public int TestSuiteId { get; set; }

        public Guid IdentityUserId { get; set; }

        public IList<Position> PositionList { get; set; }

        public string ErrorMessage { get; set; }

        public string PanelName { get; set; }

        public bool IsAdmin { get; set; }
    }
}