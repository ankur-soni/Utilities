using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class CandidateHistoryViewModel
    {

        public TestSuiteViewModel TestSuiteViewModel { get; set; }


        public int UserId { get; set; }

        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        public string NewPassword { get; set; }

  
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

  
        [Display(Name = "Requisition id")]

        public string RequisitionId { get; set; }


   
        public string Email { get; set; }

  
        [Display(Name = "Current location")]
        public string CurrentLocation { get; set; }

        [Display(Name = "Date Of birth")]
        [Required(ErrorMessage = "Date of birth is required!")]
        public DateTime DOB { get; set; }

 
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }


        [Display(Name = "Client name")]
        public string ClientName { get; set; }


        [Display(Name = "Technology")]
        public string Technology { get; set; }


        public int? TotalExperienceInYear { get; set; }


        public int? TotalExperienceInMonth { get; set; }
        
        public int? RelevantExperienceInYear { get; set; }

     
        public int? RelevantExperienceInMonth { get; set; }


        [Display(Name = "Current company")]
        public string CurrentCompany { get; set; }

      
        [Display(Name = "Current title")]      
        public string CurrentTitle { get; set; }

        public string ResumePath { get; set; }

        public string ResumeName { get; set; }

        public string ResumeDisplayName { get; set; }

        public string Role { get; set; }

     
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