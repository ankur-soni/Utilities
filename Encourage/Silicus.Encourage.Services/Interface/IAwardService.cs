using Silicus.Encourage.Models;
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
        Award GetAwardFromNominationId(int nominationId);
        IEnumerable<Award> GetAllAwards();
        List<Engagement> GetProjectsUnderCurrentUserAsManager(string email);
        List<Department> GetDepartmentsUnderCurrentUserAsManager(string email);
        List<Criteria> GetCriteriasForAward(int awardId);
        List<User> GetResourcesInEngagement(int projectId, int userIdToExcept);
        List<User> GetResourcesUnderDepartment(int DepartmentId, int userIdToExcept);
        bool AddNomination(Nomination nomination);
        int GetUserIdFromEmail(string email);


    }
}
