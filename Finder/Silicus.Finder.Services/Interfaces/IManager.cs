using System.Collections.Generic;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Services.Interfaces
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
