using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models
{
    public class UserBusinessModel
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NewPassword { get; set; }

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

        public int TotalExperienceInYear { get; set; }

        public int TotalExperienceInMonth { get; set; }

        public int RelevantExperienceInYear { get; set; }

        public int RelevantExperienceInMonth { get; set; }

        public string CurrentCompany { get; set; }

        public string CurrentTitle { get; set; }

        public string CandidateStatus { get; set; }

        public string TestStatus { get; set; }

        public string PanelId { get; set; }

        public string PanelName { get; set; }

        public string ResumePath { get; set; }

        public string ResumeName { get; set; }

        public string ProfilePhotoFilePath { get; set; }

        public Guid IdentityUserId { get; set; }

        public bool IsDeleted { get; set; }

        public int UserApplicationId { get; set; }

        public bool IsCandidateReappear { get; set; }


        public string RecruiterId { get; set; }

        public string RecruiterName { get; set; }

        public DateTime ApplicationDate { get; set; }

    }
}
