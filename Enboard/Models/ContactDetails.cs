using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
   public class ContactDetails
    {

        //Code change - EDMX Fix 

        //public long Id { get; set; }
        //public long UserId { get; set; }

        //[Display(Name = "Permanent Address")]
        //[Required(ErrorMessage = "Please enter Permanent Address")]
        //public string PermanantAddLine1 { get; set; }
        //public string PermanantAddLine2 { get; set; }
        //public string PermanantAddLine3 { get; set; }

        //[Display(Name = "City")]
        ////[Required(ErrorMessage = "Please enter Permanant City")]
        //public string PermanantAddCity { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select City")]
        //[Display(Name = "Permanant City")]
        //public int PermanantAddCityId { get; set; }

        //[Display(Name = "Please enter your City")]
        //public string OtherPermanantAddCity { get; set; }

        //[Display(Name = "Country")]
        //// [Required(ErrorMessage = "Please select Country")]
        //public string PermanantAddCountry { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select Country")]
        //[Display(Name = "Permanent Country")]
        //public int PermanantAddCountryId { get; set; }

        //[Display(Name = "State")]
        //// [Required(ErrorMessage = "Please select State")]
        //public string PermanantAddState { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select State")]
        //[Display(Name = "Permanent State")]
        //public int PermanantAddStateId { get; set; }

        //[Display(Name = "Please enter your State")]
        //public string OtherPermanantAddState { get; set; }

        //[Display(Name = "Zip Code")]
        //public string PermanantAddZipcode { get; set; }

        //[Display(Name = "Current Address")]
        //// [Required(ErrorMessage = "Please enter Current Address")]
        //public string CurrentAddLine1 { get; set; }
        //public string CurrentAddLine2 { get; set; }
        //public string CurrentAddLine3 { get; set; }

        //[Display(Name = "City")]
        //// [Required(ErrorMessage = "Please enter Current City")]          
        //public string CurrentAddCity { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select City")]
        //[Display(Name = "Current City")]
        //public int CurrentAddCityId { get; set; }

        //[Display(Name = "Country")]
        //// [Required(ErrorMessage = "Please enter Current Country")]
        //public string CurrentAddCountry { get; set; }

        //[Display(Name = "Country")]
        //// [Required(ErrorMessage = "Please enter Current Country")]
        //public string OtherCurrentAddCity { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select Country")]
        //[Display(Name = "Current Country")]
        //public int CurrentAddCountryId { get; set; }

        //[Display(Name = "State")]
        //// [Required(ErrorMessage = "Please enter Current State")]
        //public string CurrentAddState { get; set; }

        //[Display(Name = "State")]
        //// [Required(ErrorMessage = "Please enter Current State")]
        //public string OtherCurrentAddState { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select State")]
        //[Display(Name = "Current State")]
        //public int CurrentAddStateId { get; set; }

        //[Display(Name = "Zip Code")]
        //[DataType(DataType.PostalCode)]
        //public string CurrentAddZipcode { get; set; }

        //[Display(Name = "Home Phone Number")]
        //// [RegularExpression(@"^\d{9}(\d{2})?$", ErrorMessage = "Not a valid Phone number")]
        ////[Range(typeof(int), "8", "11",ErrorMessage = "{0} can only be between {1} and {2}")]
        //[RegularExpression(@"^[0-9]{8,11}$", ErrorMessage = "Not a valid Phone number")]
        //public string HomePhone { get; set; }
        //public string WorkPhone { get; set; }

        //[Display(Name = "Mobile Number")]
        //[Required(ErrorMessage = "Please enter Mobile Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        //public string MobileNumber { get; set; }
        //public string OfficialEmail { get; set; }

        //[Display(Name = "Email Address")]
        //[Required(ErrorMessage = "Please enter Email Address")]
        //[EmailAddress]
        //public string Email { get; set; }
        //public string CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string UpdatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedDate { get; set; }

        //[Display(Name = "Another Contact")]
        //[Required(ErrorMessage = "Please enter Another Contact")]
        //// [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        //public string AnotherContact { get; set; }

        //public bool IsBothAddSame { get; set; }

       //new columns


        public long ConDetID { get; set; }
        public long UserID { get; set; }
        [Display(Name = "Permanent Address")]
        [Required(ErrorMessage = "Please enter Permanent Address")]
        public string PermanantAddLine1 { get; set; }
        public string PermanantAddLine2 { get; set; }
        public string PermanantAddLine3 { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select City")]
        [Display(Name = "City")]
        public int PermanantCityID { get; set; }
        [Display(Name = "Please enter your City")]
        public string OtherPermanantCity { get; set; }
        [Display(Name = "Country")]
        public int PermanantCountryID { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select State")]
        [Display(Name = "State")]
        public int PermanantStateID { get; set; }
        [Display(Name = "Please enter your State")]
        public string OtherPermanantState { get; set; }
        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        public string PermanantZipcode { get; set; }
        [Display(Name = "Current Address")]
        [Required(ErrorMessage = "Please enter Current Address")]
        public string CurrentAddLine1 { get; set; }
        public string CurrentAddLine2 { get; set; }
        public string CurrentAddLine3 { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select City")]
        [Display(Name = "City")]
        public int CurrentCityID { get; set; }
        public string OtherCurrentCity { get; set; }
        [Display(Name = "Country")]
        public int CurrentCountryID { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select State")]
        [Display(Name = "State")]
        public int CurrentStateID { get; set; }
        [Display(Name = "State")]
        public string OtherCurrentState { get; set; }
        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        public string CurrentZipcode { get; set; }
        [Display(Name = "Landline number (enter STD code)")]
        //[Required(ErrorMessage = "Please enter Home Landline Number with STD Code")]
        [RegularExpression(@"^[0-9]{8,11}$", ErrorMessage = "Not a valid Phone number")]
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        [Display(Name = "Mobile Number")]
        //[Required(ErrorMessage = "Please enter Mobile Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string MobileNumber { get; set; }
        public string OfficialEmail { get; set; }
        [Display(Name = "Email Address")]
        //[Required(ErrorMessage = "Please enter Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Alternate Contact Number")]
        [Required(ErrorMessage = "Please enter Alternate Contact Number")]
        [RegularExpression(@"^[0-9]{8,11}$", ErrorMessage = "Not a valid Phone number")]
        public string AnotherContact { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsBothAddSame { get; set; }



    }

   public class City
   {
       public int Value { get; set; }
       public string Text { get; set; }
   }

   public class State
   {
       public int Value { get; set; }
       public string Text { get; set; }
   }

   public class COuntry
   {
       public int Value { get; set; }
       public string Text { get; set; }
   }
}
