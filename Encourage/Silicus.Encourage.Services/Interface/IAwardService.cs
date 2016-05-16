using Silicus.Encourage.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface IAwardService
    {
        IEnumerable<Award> GetAllAwards();
        List<Engagement> GetProjectsUnderCurrentUserAsManager(string email);
        List<Criteria> GetCriteriasForAward(int awardId);
        List<User> GetResourcesInEngagement(int projectId);
    }
}
