using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
	public class EmployeeSearchCriteriaViewModel
    {
        public string Title { get; set; }

        [Display(Name = "Employee Type")]
        public string EmployeeType { get; set; }

        [Display(Name = "Project Name")]
        public string Project { get; set; }

        [Display(Name = "Skill")]
        public string SkillSet { get; set; }
	}
}