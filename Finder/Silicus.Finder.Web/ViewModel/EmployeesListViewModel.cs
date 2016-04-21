using Silicus.Finder.Models.DataObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Web.ViewModel
{
    public class EmployeesListViewModel
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName { get { return FirstName + " " + MiddleName + " " + LastName; } }

        public Gender Gender { get; set; }

        [Display(Name = "Employee Type")]
        public string EmployeeType { get; set; }

        [Display(Name = "Highest Qualification")]

        public string HighestQualification { get; set; }

        [Display(Name = "Experience(Months)")]
        public int? TotalExperienceInMonths { get; set; }

        [Display(Name = "Silicus Experience")]
        public int? SilicusExperienceInMonths { get; set; }

        [Display(Name = "Manager Recommendation")]
        public string ManagerRecommendation { get; set; }

        public bool isSelected { get; set; }
        
        public string Title { get; set; }

        [Display(Name = "Cubicle Location")]
        public virtual CubicleLocation CubicleLocation { get; set; }

        public virtual Contact Contact { get; set; }
    }
}