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
        Award GetAwardById(int awardId);
        List<Engagement> GetProjectsUnderCurrentUserAsManager(string email);
        List<Department> GetDepartmentsUnderCurrentUserAsManager(string email);
        List<Criteria> GetCriteriasForAward(int awardId);
        List<User> GetResourcesInEngagement(int projectId, int userIdToExcept, int awardId);
        List<User> GetResourcesUnderDepartment(int DepartmentId, int userIdToExcept);
        List<User> GetResourcesForEditInEngagement(int engagementId, int userIdToExcept);
        List<User> GetResourcesForEditInDepartment(int DepartmentId, int userIdToExcept);
        bool AddNomination(Nomination nomination);
        int GetUserIdFromEmail(string email);
        List<WinnerData> GetWinnerData();
        List<string> GetEmailAddressOfManager(string name);
        Award GetAwardByCode(string awardCode);
    }
}
