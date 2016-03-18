using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IInfrastructureDetailsService
    {
        IList<InfrastructureDetails> GetInfrastructureDetails(int projectId, int WeekId);

        int SaveInfrastructureDetails(IList<InfrastructureDetails> InfrastructureDetails, ProjectStatus projectStatus, int weekId, string userName);
    }
}
