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
  public class EducationDetails
    {
        //public long ID { get; set; } // by Sachin Khot
        public long EduDetID { get; set; } // by Sachin Khot


        public long UserId { get; set; }            
           
        [Display(Name = "Education Category")]
       // [Required(ErrorMessage = "Please select Education Category")]
        public string EducationCategory { get; set; }
     
        [Display(Name = "Education Category")]
        public string OtherEducationCategory { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Education Category")]
        [Display(Name = "Education Category")]
        public int EducationCategoryId { get; set; }

        [Display(Name = "Discipline")]
       // [Required(ErrorMessage = "Please select Degree/Deploma")]
        public string TypeofDegreeDeploma { get; set; }

        [Display(Name = "Discipline")]
        public string OtherDiscipline { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Degree / Diploma")]
        [Display(Name = "Type of Degree/Diploma")]
        public int TypeofDegreeDeplomaId { get; set; }

        public int UniversityID { get; set; }

        public string Descipline { get; set; }

        public int DisciplineID { get; set; } //added by sachin

        [Display(Name = "Affiliated to")]
        public string University_BoardName { get; set; }

        [Display(Name = "University / Board Name")]
        public string OtherUniversityName { get; set; }

        [Display(Name = "University / Board Name")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select University / Board Name")]
        public int University_BoardNameId { get; set; }

        [Display(Name = "Name of the College / Institute")]
        public string InstituteName { get; set; }

        [Display(Name = "Institute Name")]
        public string OtherCollegeName { get; set; }

        [Display(Name = "Institute Name")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Institute Name")]
        public int InstituteNameId { get; set; }

        public int CollegeID { get; set; } //added by sachin 
        

        [Display(Name = "Institute Address")]
        //[Required(ErrorMessage = "Please enter Institute Address")]
        [DataType(DataType.MultilineText)]
        public string InstituteAddress { get; set; }

        [Display(Name = "Passing Month")]
        public string PassingMonth { get; set; }

        [Required(ErrorMessage = "Please select Passing Year")]
        [Display(Name = "Passing Year")]       
        [RegularExpression("^[0-9]*$", ErrorMessage = "Year must be Numeric")]    
        public Nullable<int> PassingYear { get; set; }

        [Display(Name = "Attended From")]
        [Required(ErrorMessage = "Please select attended from")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FromDate { get; set; }
        [Display(Name = "Attended To")]
        [Required(ErrorMessage = "Please select attended to")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ToDate { get; set; }

        [Display(Name = "Roll Number")]
        public string RollNumber { get; set; }

        [Display(Name = "Percentage Score")]
        [Required(ErrorMessage = "Please enter Percentage")]
        //[Range(typeof(string), "0", "100.00",ErrorMessage = "Enter valid percentage")]
        [Range(typeof(string), "1", "100", ErrorMessage = "Enter valid percentage")]
        //[RegularExpression(@"^(\d{1,6}(\.\d{1,2})?)$|^(\d{7}(\.\d{1,2})?)$|^(\d{8}(\.\d{1,2})?)$", ErrorMessage = "Percentage must be Numeric")]      

        public string Percentage { get; set; }

        [Display(Name = "Specialization")]
        public string Specialization { get; set; }

        [Display(Name = "Specialization")]
        [Required(ErrorMessage = "Please enter Specialization")]
        public string OtherSpecialization { get; set; }
       
        [Display(Name = "Specialization")]
        //[Range(1, Int32.MaxValue, ErrorMessage = "Please select Specialization")]
        public int SpecializationId { get; set; }

        [Display(Name = "Class")]
        public string Class { get; set; }
           

        [Display(Name = "Class Option")]
        public string OtherClass { get; set; }

        [Display(Name = "Class Option")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Class")]
        public int ClassId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        [Display(Name = "Explain breaks during Education")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter the text")]
        public string BreaksDuringEducation { get; set; }

        public List<EducationDetails> educationDetialslist { get; set; } 
        
        public IPagedList<EducationDetailsHistory> EducationalDetailsList { get; set; }

        public SelectList UniversityList { get; set; }
        //public SelectList EducationCategoryList { get; set; }
    }


    public class EducationDetailsHistory 
    {
        public Int64 EduDetID { get; set; }
     
        public string Percentage { get; set; }

        public Nullable<int> PassingYear { get; set; }
     
        public string DisciplineName { get; set; }

        public string University { get; set; }

        public string EducationCategory { get; set; }

        //public Int64 CatId { get; set; }
        
    }
}
