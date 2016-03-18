using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectSkillSetDetailsViewModel
    {
        [Display(Name = "SkillSet Id")]
        public int SkillSetId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}