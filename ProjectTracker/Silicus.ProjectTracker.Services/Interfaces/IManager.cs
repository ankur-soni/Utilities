using System.Collections.Generic;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IManager
    {
        IList<Manager> GetManagers();

        //int AddManagerDetail(ManagerService managerDetail);

        //void UpdateManagerDetail(Manager managerDetail);

        //void DeleteManagerDetail(ProjectDetail projectDetail);

        // List<ProjectDetailService> GetProjectDetails(int managerId);
    }
}
