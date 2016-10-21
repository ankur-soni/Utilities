using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
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
