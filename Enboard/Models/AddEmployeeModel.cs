using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace Models
{
    public class AddEmployeeModel
    {
        public long ID { get; set; }
        public int EmployeeMasterId { get; set; }
        [System.Web.Mvc.Remote("IsEmpNoExist", "User", AdditionalFields = "EmployeeMasterId", ErrorMessage = "Employee Number already exists!")]
        [Required(ErrorMessage = "Please enter Employee No")]
        public string EmpNo { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Please enter Date of Birth")]
        public string DOB { get; set; }
        [System.Web.Mvc.Remote("IsEmailExist", "User", AdditionalFields = "EmployeeMasterId,UserID", ErrorMessage = "Email already exists!")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        [Required(ErrorMessage = "Please enter Email")]
        //[EmailAddress(ErrorMessage ="Please enter correct Email Address!")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Please enter Joining Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        public string JoiningDate { get; set; }


        public int Active { get; set; }

        [Required(ErrorMessage = "Please Enter Reason for Leaving")]
        public string ReasonforLeaving { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please select Leaving Date")]
        public string LeavingDate { get; set; }


        public IPagedList<OfferCandidateModel> OfferCandidateList { get; set; }
        public IPagedList<AddEmployeeModelList> EmployeeDetailsList { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsEmployeeEditMode { get; set; }
        public bool IsActive { get; set; }
       
    }


    public class AddEmployeeModelList
    {
        public long ID { get; set; }
        public string EmpNo { get; set; }
        public bool isActive { get; set; }
        public string EmpName { get; set; }

        public string DOB { get; set; }

        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> JoiningDate { get; set; }


        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string ReasonforLeaving { get; set; }
        public string LeavingDate { get; set; }

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
        public string Department { get; set; }
        public string ShortJoiningDate { get; set; }
    }
}
