using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class APIResponse
    {
        /// public AddUser candidtes { get; set; }
          public List<JobViteCandidateBusinessModel> candidates { get; set; }

    }

    public class LoginDetails
    {
       
        public long UserId { get; set; }

        [Display (Name="User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter User Name")]
        public string Email { get; set; }

        
        public Nullable<int> Active { get; set; }
        public Nullable<System.DateTime> ActivatedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int RoleId { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }

        public string ShortDOB { get; set; }
        public string ShortJoiningDate { get; set; }
        public bool IsOnboarded { get; set; }
        public DateTime LastLogin { get; set; }

        //Code change - Newly added columns
       
        public string ContactNumber { get; set; }
        public string RequisitionID { get; set; }
        public string JoiningLocation { get; set; }
        public string ProjectName { get; set; }
        public string PrimarySkill { get; set; }
        public string OnboardingSPOCName { get; set; }
        public string RecruiterName { get; set; }
        public int DepartmentID { get; set; }
        //public int SubDocCatID { get; set; }
        public int DesignationID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> IsSubmitted { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string Designation { get; set; }
        public string EmpNo { get; set; }
        public double? OverAllUploadPecentage { get;  set;}
    }

    public class LostPasswordModel
    {
        //[Required(ErrorMessage = "We need your email to send you a reset link!")]
        [Display(Name = "Your Email Id")]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Enter your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Enter your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }       

        //[Required(ErrorMessage = "Enter your Date of Birth")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
      //  [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DOB { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required]
        //[Display(Name = "Confirm Password")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "New password and confirmation does not match.")]
        //public string ConfirmPassword { get; set; }

        
    }
    public class AddEditUserModel
    {
       

        public long UserID { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter Date of Birth")]
        public string DOB { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please enter Email Address")]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        [System.Web.Mvc.Remote("IsUserEmailExist", "User", AdditionalFields = "UserID", ErrorMessage = "Email already exists!")]
        public string Email { get; set; }
        public string EmpNo { get; set; }
        [Required(ErrorMessage = "Please enter Expected Joining Date")]
        public Nullable<System.DateTime> JoiningDate { get; set; }
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Please enter Contact Number")]
        public string ContactNumber { get; set; }
        public string RequisitionID { get; set; }
        [Display(Name = "Joining Location")]
        [Required(ErrorMessage = "Please enter Joining Location")]
        public string JoiningLocation { get; set; }
        public string ProjectName { get; set; }

        public string PrimarySkill { get; set; }
        public string OnboardingSPOCName { get; set; }
        public string RecruiterName { get; set; }
        [Display(Name = "Department")]
        [Required(ErrorMessage = "Please select Department")]
        public int DepartmentID { get; set; }
        //[Display(Name = "Education Category")]
        //public int SubDocCatID { get; set; }
        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Please select Designation")]
        public int DesignationID { get; set; }
        public int RoleID { get; set; }
        public Nullable<System.DateTime> ActivatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsSubmitted { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }

        //newly added
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter Country Code")]
        public string CountryCode { get; set; }

        //[Required(ErrorMessage = "Please enter Password")]
        //public string AdminPassword { get; set; }

        public bool SSC { get; set; }
        public bool HSC { get; set; }
        public bool Graduation { get; set; }
        public bool PostGraduation { get; set; }
        public bool PostGraduateDiploma { get; set; }
        public bool Diploma { get; set; }
        public bool DoctoratePhD { get; set; }
        public bool Other { get; set; }

        public IEnumerable<EducationCategory> AvailableEducationCategories { get; set; }
        public IEnumerable<EducationCategory> SelectedEducationCategories { get; set; }
        public PostedEducationCategories PostedEducationCategories { get; set; }

    }

    public class PostedEducationCategories
    {
        //this array will be used to POST values from the form to the controller
        public string[] EducationCategoryIds { get; set; }
    }

    public class EducationCategory
    {
        //Integer value of a checkbox
        public int Id { get; set; }

        //String name of a checkbox
        public string Name { get; set; }

        //Boolean value to select a checkbox
        //on the list
        public bool IsSelected { get; set; }

        //Object of html tags to be applied
        //to checkbox, e.g.:'new{tagName = "tagValue"}'
        public object Tags { get; set; }

    }

    public class OfferCandidateModel
    {

        public long UserID { get; set; }
        public string EmployeeName { get; set; }

        public string DOB { get;set; }

        public string EmailAddress { get; set; }

        public string ShortDOB { get; set; }
    
    }

    public class ChangePassword
    {
        [Required(ErrorMessage = "Please enter old password")]
        [Display(Name = "Old Password")]
        public string PrevPassword { get; set; }

        [Required(ErrorMessage = "Please enter new password")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm new password")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }

        public long UserID { get; set; }
    }

    public class ChangePasswordResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }
    }
    public class AddUser
    {
        public string email { get; set; }
        public string lastName { get; set; }

    }

    public class JobViteCandidateBusinessModel
    {
        public string address { get; set; }
        public string address2 { get; set; }
        //public Application application { get; set; }
        public string city { get; set; }
        public string companyName { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string eId { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string homePhone { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string mobile { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string stateCode { get; set; }
        public string stateName { get; set; }
        public string title { get; set; }
        public string workPhone { get; set; }
        public string workStatus { get; set; }


    }

}