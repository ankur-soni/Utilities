using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Models.DataObjects
{
    public class Employee
    {
        public Employee()
        {
            Projects = new HashSet<Project>();
            SkillSets= new HashSet<SkillSet>();
            IsActive = true;
        }

        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Select Role")]
        public string Role { get; set; }

        [Index("IX_Unique_EmployeeCode", 1, IsUnique = true)]
        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "Employee Code should contain less than 10 characters")]
        //[Remote("DoesUserCodeExist", "Employee", HttpMethod = "POST", ErrorMessage = "User code already exists. Please enter a different user code.")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        [Required(ErrorMessage = "First Name can't be blank")]
        [StringLength(20, ErrorMessage = "First Name should contain less than 20 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are       not  allowed.")]
        [StringLength(20, ErrorMessage = "Middle Name should contain less than 20 characters")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are       not  allowed.")]
        [Required(ErrorMessage = "Last Name can't be blank")]
        [StringLength(20, ErrorMessage = "Last Name should contain less than 20 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get {return FirstName+" "+ LastName;} }

        [Range(1, int.MaxValue, ErrorMessage = "Select a Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Employee Type")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a correct EmployeeType")]
        public string EmployeeType { get; set; }

        [Display(Name = "Total Experience")]
        public int? TotalExperienceInMonths { get; set; }

        [Display(Name = "Silicus Experience")]
        public int? SilicusExperienceInMonths { get; set; }

        [Display(Name = "Highest Qualification")]
        public string HighestQualification { get; set; }

       // public virtual ICollection<EmployeeSkillSet> EmployeeSkillSets { get; set; }

        [Display(Name = "Cubicle Location")]
        [ForeignKey("CubicleLocation")]
        public int CubicleLocationId { get; set; }
        public virtual CubicleLocation CubicleLocation { get; set; }

        [ForeignKey("Contact")]
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        [NotMapped]
        public IList<int> ProjectId { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        //public virtual ICollection<EmployeeProjects> EmployeeProjects { get; set; }

        [NotMapped]
        public IList<int> SkillId { get; set; }
        public virtual ICollection<SkillSet> SkillSets { get; set; } 

        [Display(Name = "Manager Recommendation")]
        [StringLength(200)]
        public string ManagerRecommendation { get; set; }

        public virtual ICollection<EmployeeTitles> EmployeeTitles{ get; set; }

        public string MembershipId { get; set; }
        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ArchieveDate { get; set; }

        [NotMapped]
        public int TitleId { get; set; }

        [NotMapped]
        public string Title { get; set; }

        public virtual ICollection<EmployeeRewards> EmployeeRewards { get; set; }
    }
}
