using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PersonalDetails
    {
        //Code change - EDMX Fix , adding new fields
        public long PerDetID { get; set; }

        public string ContactNumber { get; set; }

        public string RequisitionID { get; set; }

        public string JoiningLocation { get; set; }

        public string ProjectName { get; set; }

        public string PrimarySkill { get; set; }

        public string OnboardingSPOCName { get; set; }

        public string RecruiterName { get; set; }

        public int DepartmentID { get; set; }

        public string Department { get; set; }

        // public int SubDocCatID { get; set; }

        public int DesignationID { get; set; }

        public string Designation { get; set; }

        public string JoiningDate { get; set; }

        public long UserID { get; set; }

        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Please enter Father Name")]
        public string FatherName { get; set; }

        [Display(Name = "Birth Place")]
        public string PlaceofBirth { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Please select State")]
        public string BirthState { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please select City")]
        public string BirthCity { get; set; }

        public string OtherPlaceOfBirth { get; set; }

        public string OtherBirthState { get; set; }

        public string OtherBirthCity { get; set; }

        [Display(Name = "Blood Group")]
        [Required(ErrorMessage = "Please enter Blood Group")]
        public int BloodGroup { get; set; }

        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Please enter Nationality")]
        public Nullable<int> Nationality { get; set; }

        [Display(Name = "PAN Number")]
        [Required(ErrorMessage = "Please enter PAN Number")]
        public string PANNumber { get; set; }

        public string SecurityNumber { get; set; }

        [Display(Name = "Aadhar Card Number")]
        [RegularExpression(@"^[0-9]{12,12}$", ErrorMessage = "Not a valid Aadhar Card Number")]
        public string AadharCardNumber { get; set; }
      
        [Display(Name = "UAN Number")]
        [RegularExpression(@"^[0-9]{12,12}$", ErrorMessage = "Not a valid UAN Number")]
        public string UANNumber { get; set; }

        [Display(Name = "Mother Tongue")]
        [Required(ErrorMessage = "Please enter Mother Tongue")]
        public string MotherTongue { get; set; }

        public Nullable<bool> IsActive { get; set; }

        public string CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string EmpNo { get; set; }

        [Display(Name = "Personal Email ID")]
        public string EmpEmail { get; set; }

        public string MaidenName { get; set; }

        public string OtherName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public string DateofBirth { get; set; }

        public string EthnicCode { get; set; }

        public string Gender { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Specific Language")]
        public string SpecificLanguage { get; set; }

        [Display(Name = "Marital Status")]
        [Required(ErrorMessage = "Please enter Marital Status")]
        public int MaritalStatID { get; set; }

        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }

        [Display(Name = "Have Passport?")]
        public string HavePassport { get; set; }

        [Display(Name = "Name as on Passport")]
        public string NameOnPassport { get; set; }

        [Display(Name = "Date Of Expiry")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> PassportExpiryDate { get; set; }

        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "Number Of Employments")]
        [Range(0, 20, ErrorMessage = "Please enter total number of Employments including current.(In case of fresher put 0)")]
        public int? NumberOfEmployments { get; set; }

        public bool FinalStatus { get; set; }

    }

    public class BloodGroup
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class Languages
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class CountryCode
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class ChangeRequestDetails
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Please enter Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Please enter Country Code")]
        public string CountryCode { get; set; }

        [Display(Name = "Personal Email ID")]
        [Required(ErrorMessage = "Please enter Email Address")]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string EmpEmail { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter Date of Birth")]
        public string DateofBirth { get; set; }
    }
}
