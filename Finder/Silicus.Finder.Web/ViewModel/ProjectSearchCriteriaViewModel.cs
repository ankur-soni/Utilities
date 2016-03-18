using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectSearchCriteriaViewModel
    {
        [Display(Name = "Engagement Manager")]
        public string EngagementManager { get; set; }

        [Display(Name = "Project Manager")]
        public string ProjectManager { get; set; }

        [Display(Name = "Skill")]
        public string SkillSet { get; set; }

        public string Status { get; set; }

        [Display(Name = "Project Type")]
        public string ProjectType { get; set; }
    }
}