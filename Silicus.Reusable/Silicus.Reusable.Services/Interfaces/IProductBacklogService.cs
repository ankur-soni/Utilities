using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Silicus.FrameworxProject.Models;
using System.Collections.Generic;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface IProductBacklogService
    {
        IEnumerable<ProductBacklog> GetAllProductBacklog(string projectName);
        WorkItem UpdateTimeAllocated(int workItemId, double time);
        WorkItem UpdateTimeSpent(int workItemId, double time);
        IEnumerable<TeamProjectReference> GetTeamProjects();
        ProductBacklog GetWorkItemDetails(int id);
        void UpdateAssignee(ProductBacklog productBacklog);
    }
}
