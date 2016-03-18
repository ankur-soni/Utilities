using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Finder.Web.ViewModel
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get {return FirstName+" "+ LastName;} }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }

        [Display(Name = "Total Experience")]
        public int? TotalExperienceInMonths { get; set; }

        [Display(Name = "Silicus Experience")]
        public int? SilicusExperienceInMonths { get; set; }

        [Display(Name = "Highest Qualification")]
        public string HighestQualification { get; set; }

        public string Title { get; set; }

        [Display(Name = "Manager Recommendation")]
        public string ManagerRecommendation { get; set; }

        [Display(Name = "Skill Set")]
        public ICollection<ProjectSkillSetDetailsViewModel> SkillSets { get; set; }

        [Display(Name = "Cubicle Location")]
        public CubicleLocationViewModel CubicleLocation { get; set; }
        public ContactViewModel Contact { get; set; }
        public ICollection<Project> Projects { get; set; }
        public virtual ICollection<EmployeeRewards> EmployeeRewards { get; set; }
    }
}