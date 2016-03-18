using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IChangeRequestDetailsService
    {
        IList<ChangeRequestDetails> GetChangeRequestDetails(int projectId, int WeekId);

        int SaveChangeRequestDetails(IList<ChangeRequestDetails> ChangeRequestDetails, ProjectStatus projectStatus, int weekId, string userName);
    }
}

