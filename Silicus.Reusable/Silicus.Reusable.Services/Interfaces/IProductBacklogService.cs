using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Silicus.FrameworxProject.Models;
using System.Collections.Generic;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface IProductBacklogService
    {
        IEnumerable<ProductBacklog> GetAllProductBacklog(string projectName);
        WorkItem Accept(int workItemId, string userName);
        IEnumerable<TeamProjectReference> GetTeamProjects();
    }
}
