using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Mvc;

namespace Models
{
    public class EmployementModel
    {

        public long EmploymentDetID { get; set; }
        public long UserId { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please Enter Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Address")]
        [Required(ErrorMessage = "Please Enter Company Address")]
        public string CompanyAddress { get; set; }

        [Display(Name = "City")]
        public string ComapnyCity { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select City")]
        [Display(Name = "City")]
        public int? CompanyCityId { get; set; }

        [Display(Name = "State")]
        public string CompanyState { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select State")]
        [Display(Name = "State")]
        public int? CompanyStateId { get; set; }

        [Display(Name = "Country")]
        public string CompanyCountry { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Company")]
        [Display(Name = "Country")]
        public int? CompanyCountryId { get; set; }

        [Display(Name = "Zipcode")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Company Zipcode must be numeric")]
        public string CompanyZipcode { get; set; }

        [Display(Name = "Company Phone Number")]
        //[Required(ErrorMessage = "Please enter Company Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Company Phone Number must be numeric")]
        public string CompanyPhoneNumber { get; set; }

        //[Display(Name = "Company Website")]
        //[Required(ErrorMessage = "Please enter Comany Website")]
        //[RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?", ErrorMessage = "Enter Valid Web Address")]
        //[Url]
        public string CompanyWebsite { get; set; }

        [Display(Name = "From Date")]
        [Required(ErrorMessage = "Please select Date Of Joining")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "Please select Relieving Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ToDate { get; set; }

        [Display(Name = "Job Title")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "For Job Title use letters only please")]
        // [Required(ErrorMessage = "Please enter Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Please enter Current Designation")]
        public string Designation { get; set; }

        //[Display(Name = "Previous Employee Id")]
        public string PreviousEmpId { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Current Salary per Annum must be numeric")]
        [Display(Name = "CTC")]
        [Required(ErrorMessage = "Please enter Current Salary per Annum")]
        public string CTC { get; set; }

        [Display(Name = "Reason For Leaving")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter Reason For Leaving")]
        public string ReasonForLeave { get; set; }

        [Display(Name = "Supervisor Name")]
        //[Required(ErrorMessage = "Please enter Supervisor Name")]
        public string SupervisorName { get; set; }

        [Display(Name = "Supervisor Designation")]
        //[Required(ErrorMessage = "Please enter Supervisor Designation")]
        public string SupervisorDesignation { get; set; }

        [Display(Name = "Supervisor Email")]
        //[Required(ErrorMessage = "Please enter Supervisor Email")]
        [EmailAddress]
        public string SupervisorEmail { get; set; }

        [Display(Name = "Supervisor  Phone")]
        //[Required(ErrorMessage = "Please enter Supervisor Phone")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid Phone number")]
        public string SupervisorPhone { get; set; }

        [Display(Name = "HR Name")]
        // [Required(ErrorMessage = "Please enter HR Name")]
        public string HRName { get; set; }

        [Display(Name = "HR Email")]
        // [Required(ErrorMessage = "Please enter HR Email")]
        [EmailAddress]
        public string HREmail { get; set; }

        [Display(Name = "HR Phone")]
        // [Required(ErrorMessage = "Please enter HR Phone")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid Phone number")]
        public string HRPhone { get; set; }

        [Display(Name = "Other Comapny City")]
        public string OtherComapnyCity { get; set; }

        [Display(Name = "Other Company State")]
        public string OtherCompanyState { get; set; }

        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string PastEmploymentEmpID { get; set; }

        public string EmployementNo { get; set; }
        public Nullable<bool> Active { get; set; }

        public List<EmploymetDetailsHistory> EmploymnetDetailsList { get; set; }

        //Admin - View user details
        public IPagedList<EmploymetDetailsHistory> EmploymnetDetailsList1 { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        [Display(Name = "Currency")]
        public Nullable<int> CurrencyID { get; set; }

        // [Required(ErrorMessage = "Please select the value")]
        [Display(Name = "Is This Your Current Employment?")]
        public Nullable<bool> IsCurrentEmployment { get; set; }

        public SelectList CurrencyList { get; set; }

        //[Display(Name = "Number Of Employments")]
        //[Range(0, 20, ErrorMessage = "Please enter total number of Employments including current.(In case of fresher put 0)")]
        //public int? NumberOfEmployments { get; set; }

    }

    public class EmploymetDetailsHistory
    {

        public Int64 EmploymentId { get; set; }

        public string EmployementNo { get; set; }

        public Int64 CurrencyID { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please Enter Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Address")]
        [Required(ErrorMessage = "Please Enter Company Address")]
        public string CompanyAddress { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Current Salary per Annum must be numeric")]
        [Display(Name = "CTC")]
        [Required(ErrorMessage = "Please enter Current Salary per Annum")]
        public string CTC { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "Please select Relieving Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public string ToDate { get; set; }

        [Display(Name = "Reason For Leaving")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter Reason For Leaving")]
        public string ReasonForLeave { get; set; }

        [Display(Name = "From Date")]
        [Required(ErrorMessage = "Please select Date Of Joining")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public string FromDate { get; set; }


        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Please enter Current Designation")]
        public string Designation { get; set; }

        public string SupervisiorName { get; set; }

        public bool IsCurrentEmployment { get; set; }

    }


    public class ErrorMessageModel
    {
        public string MessageType {get;set;}
        public string Message {get;set;}
    }

    public class Employment
    {
        [Display(Name = "Enter number of employments in your career")]
        [Range(0, 20, ErrorMessage = "Please enter total number of Employments including current and it should not exceed 20.(In case of fresher put 0)")]
        public int? NumberOfEmployments { get; set; }
    }
}
