using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IProjectMappingService
    {       

        IEnumerable<string> GetAssignedProjects(string userName);

        bool SaveProjectsToUser(List<string> selectedValues, string userName);

        bool IsProjectMapped(int projectId,string userName);

        int GetCountsOfAssignedProject();

        int GetCountsOfNonAssignedProject();

        
    }
}

