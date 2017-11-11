using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PagedList;


namespace Models
{
  public class ManageEmployeeModel
    {
        public long? ID { get; set; }
        public string EmpNo { get; set; }                       
        public string EmpName { get; set; }
        public string Designation { get; set; }
         [Required(ErrorMessage = "Please select Designation")]
        public int? DesignationID { get; set; }

        public string Department { get; set; }
      [Required(ErrorMessage = "Please select Department")]  
      public int? DepartmentId { get; set; }

        [Display(Name = "Total Years Experience")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Year value must be numeric")]    
        [Required(ErrorMessage = "Enter Total Experience in Years")]  
        public decimal? TotalExprInYears { get; set; }

       
         [Range(0, 11, ErrorMessage = "Value Must be Between 1 to 11")]
         [RegularExpression("^[0-9]*$", ErrorMessage = "Month value must be numeric")]    
        [Required(ErrorMessage = "Enter Total Experience in Months")]  
        public decimal? TotalExprInmonths { get; set; }

        public int UserId {get;set;}
        public IPagedList<ManageEmployeesListModel> ManageEmployeesList { get; set; }
     
       
    }


    public class ManageEmployeesListModel 
    {
        public long? ID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public string DesignationID { get; set; }
        public string Department { get; set; }
        public string DepartmentId { get; set; }
        public string TotalExprInYears { get; set; }
        public string TotalExprInMonths { get; set; }

        public string TotalExperience { get; set; }
    }
}
