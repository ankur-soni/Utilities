using Silicus.Finder.Models.DataObjects;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class EmployeeSelectViewModel
    {
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName { get { return FirstName + " " + MiddleName + " " + LastName; } }

        [Display(Name = "Experience(Months)")]
        public int? TotalExperienceInMonths { get; set; }

         [Display(Name = "Manager Recommendation")]
        public string ManagerRecommendation { get; set; }
        
        public bool isSelected { get; set; }

        [Display(Name = "Cubicle Location")]
        public virtual CubicleLocation CubicleLocation { get; set; }
        
        public virtual Contact Contact { get; set; }
    }
}