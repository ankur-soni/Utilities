using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Silicus.FrameworxProject.Models;
using System.Collections.Generic;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface IProductBacklogService
    {
        IEnumerable<ProductBacklog> GetAllProductBacklog(string projectName);
        WorkItem UpdateTimeAllocated(ProductBacklog productBackloge);
        WorkItem UpdateTimeSpent(ProductBacklog productBacklog);
        IEnumerable<TeamProjectReference> GetTeamProjects();
        ProductBacklog GetWorkItemDetails(int id);
        void UpdateAssignee(ProductBacklog productBacklog);
        void AddWorkItem(ProductBacklog productBacklog, string projectName);
        bool IsFrameworxUser(string emailAddress);
    }
}
