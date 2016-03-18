using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IUserDashboardService
    {
        IEnumerable<ProjectStatus> GetUserProjectStatus(string userName);
    }
}
