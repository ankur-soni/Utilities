using System.Collections.Generic;

namespace Silicus.Finder.Web.ViewModel
{
    public class ProjectsViewModel
    {
        public List<ProjectListViewModel> Projects { get; set; }
        public ProjectSearchCriteriaViewModel SearchCriteria { get; set; }
    }
}