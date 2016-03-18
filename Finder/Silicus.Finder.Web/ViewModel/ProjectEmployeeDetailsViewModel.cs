using Silicus.Finder.Models.DataObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectEmployeeDetailsViewModel
    {
        public int EmployeeId { get; set; }

        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get {return FirstName+" "+ LastName;} }

        public string Title { get; set; }

        [Display(Name = "Total Experience")]
        public int? TotalExperienceInMonths { get; set; }

        [Display(Name = "Cubicle Location")]
        public int CubicleLocationId { get; set; }      
        public virtual CubicleLocation CubicleLocation { get; set; }

        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public int TitleId { get; set; }

        public virtual ICollection<EmployeeTitles> EmployeeTitles { get; set; }

       
    }
}