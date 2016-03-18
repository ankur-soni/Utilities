using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Web.ViewModel
{
    public class EmployeeCreateViewModel
    {
        public EmployeeCreateViewModel()
        {
            Projects = new HashSet<Project>();
            SkillSets= new HashSet<SkillSet>();
            IsActive = true;
        }

        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Select Role *")]
        [Display(Name = "Role *")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "Employee Code should contain less than 10 characters")]
        [Remote("DoesUserCodeExist", "Employee", HttpMethod = "POST", ErrorMessage = "User code already exists. Please enter a different user code.")]
        [Display(Name = "Employee Code *")]
        public string EmployeeCode { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        [Required(ErrorMessage = "First Name can't be blank")]
        [StringLength(20, ErrorMessage = "First Name should contain less than 20 characters")]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are       not  allowed.")]
        [StringLength(20, ErrorMessage = "Middle Name should contain less than 20 characters")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are       not  allowed.")]
        [Required(ErrorMessage = "Last Name can't be blank")]
        [StringLength(20, ErrorMessage = "Last Name should contain less than 20 characters")]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get {return FirstName+" "+ LastName;} }

        [Range(1, int.MaxValue, ErrorMessage = "Select a Gender")]
        [Display(Name = "Gender *")]
        public Gender Gender { get; set; }

        [Display(Name = "Employee Type *")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a correct EmployeeType")]
        public EmployeeType EmployeeType { get; set; }

        
        [Display(Name = "Total Experience")]
        //[Remote("CheckForSilicusExperience", "Employee", AdditionalFields = "SilicusExperienceInMonths", HttpMethod = "POST", ErrorMessage = "Silicus Experience must be greater than equal to Total Experience.")]
        [Range(0, int.MaxValue, ErrorMessage = "input not valid")]
        public int? TotalExperienceInMonths { get; set; }

        [Display(Name = "Silicus Experience")]
        [Range(0, int.MaxValue, ErrorMessage = "input not valid")]
        [Remote("CheckForSilicusExperience", "Employee", AdditionalFields = "TotalExperienceInMonths", HttpMethod = "POST", ErrorMessage = "Silicus Experience must be greater than equal to Total Experience.")]
        public int? SilicusExperienceInMonths { get; set; }

        [Display(Name = "Highest Qualification")]
      //  [Required(ErrorMessage = "Enter your Highest Qualification")]
        public string HighestQualification { get; set; }

       // public virtual ICollection<EmployeeSkillSet> EmployeeSkillSets { get; set; }
        
        [Display(Name = "Cubicle Location")]
        [ForeignKey("CubicleLocation")]
        public int CubicleLocationId { get; set; }      // composite key in Location, Foreign key in Employee
        public virtual CubicleLocationCreateViewModel CubicleLocation { get; set; }

        
        [ForeignKey("Contact")]
        public int ContactId { get; set; }
        public virtual CreateContactViewModel Contact { get; set; }

        [NotMapped]
        public IList<int> ProjectId { get; set; }
        public virtual ICollection<Project> Projects { get; set; }   // rename at the time of mapping otherwise Project_ProjectId column will get created

        //public virtual ICollection<EmployeeProjects> EmployeeProjects { get; set; }

        [NotMapped]
        public IList<int> SkillId { get; set; }
        public virtual ICollection<SkillSet> SkillSets { get; set; } 

        [Display(Name = "Manager Recommendation")]
        [StringLength(200)]
        public string ManagerRecommendation { get; set; }

        public string MembershipId { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Title")]
        public int TitleId { get; set; }

        public virtual ICollection<EmployeeRewards> EmployeeRewards { get; set; }        
    }
}