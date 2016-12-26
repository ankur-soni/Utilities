using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [CompareAttribute("NewPassword", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public string Position { get; set; }

        public string RequisitionId { get; set; }

        public string CurrentLocation { get; set; }

        public DateTime DOB { get; set; }

        public string ContactNumber { get; set; }

        public string ClientName { get; set; }

        public string Technology { get; set; }

        public string TotalExperience { get; set; }

        public string RelevantExperience { get; set; }

        public string CurrentCompany { get; set; }

        public string CurrentTitle { get; set; }

        public string CandidateStatus { get; set; }

        public string TestStatus { get; set; }

        public string PanelId { get; set; }

        public string PanelName { get; set; }

        public string ResumePath { get; set; }

        public string ResumeName { get; set; }

        public Guid IdentityUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
