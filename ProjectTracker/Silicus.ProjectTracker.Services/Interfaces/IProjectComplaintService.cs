using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{    
    public interface IProjectComplaintService
    {

        IEnumerable<ProjectComplaint> GetProjectComplaints(int projectId, int weekId);

        int SaveProjectsComplaints(IList<ProjectComplaint> ProjectComplaintDetails, ProjectStatus ProjectStatus, string userName, int weekId);


    }
}
