using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IProjectResourceService
    {
        IList<ProjectResourceUtilization> GetProjectResources(int projectId, int WeekId);

        int SaveProjectResources(IList<ProjectResourceUtilization> ProjectResourceDetails, ProjectStatus projectStatus, int weekId, string userName);
    }
}
