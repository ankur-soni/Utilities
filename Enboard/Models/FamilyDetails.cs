using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class FamilyDetails
    {
        public long FamDetID { get; set; }
        public long UserID { get; set; }

        [Display(Name = "Relationship")]
        [Required(ErrorMessage = "Please Select Relationship")]
        public int RelationshipID { get; set; }

        //[Display(Name = "Full Name")]
        //[Required(ErrorMessage = "Please Enter Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Dependent")]
        public bool Dependent { get; set; }
        public int DependentID { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Name as in passport")]
        public string NameAsInPassport { get; set; }
        [Display(Name = "Passport number")]
        public string PassportNumber { get; set; }

        [Display(Name = "Occupation")]
        public string Occupation { get; set; }
        [Display(Name = "Place of Birth")]
        public string PlaceofBirth { get; set; }
        [Display(Name = "Notes If Any")]
        public string NotesIfAny { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Display(Name = "Passport Issue Date")]
        public DateTime? PassportIssueDate { get; set; }
        [Display(Name = "Passport Expiry Date")]
        public DateTime? PassportExpiryDate { get; set; }

        [Display(Name = "BloodGroup")]
        public string BloodGroup { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public IPagedList<FamilyHistory> FamilyHistoryList { get; set; }

        [Display(Name = "Contact in Emergency")]
        public string EmergencyContact { get; set; }

        //newly added
        public string CountryCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Is Emergency Contact")]
        public Nullable<bool> IsEmergencyContact { get; set; }
    }

    public class RelationshipName
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class FamilyHistory
    {
        public int ID { get; set; }
        public long FamDetID { get; set; }
        public string RelationShipName { get; set; }
        //[Display(Name = "Full Name")]
        //[Required(ErrorMessage = "Please Enter Full Name")]
        //public string FullName { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Dependent { get; set; }
        public string IsActive { get; set; }

        //newly added
        public string CountryCode { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int RelationshipID { get; set; }
        public long UserID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Is Emergency Contact")]
        public Nullable<bool> IsEmergencyContact { get; set; }

    }
}
