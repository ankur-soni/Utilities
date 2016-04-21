using Silicus.Finder.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainerr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.ModelMappingService.Interfaces
{
    public interface ICommonMapper
    {
        ICommonDataBaseContext GetCommonDataBAseContext();

        Employee MapUserToEmployee(User user);

        List<Engagement> EmployeeProjects(IList<int> engagegmetIds);

        IList<int> GetEngagementIds(User user);

        Models.DataObjects.Role MapRoleToRole(Silicus.UtilityContainer.Models.DataObjects.Role role);

        Project MapEngagementToProject(Engagement engagement);

        Employee MapBasicPropertiesOfUserToEmployee(User user);

        Project MapBasicPropertiesOfEngagementToProject(Engagement engagement);

        SkillSet MapSkillToSkillSet(Skill skill);
        SkillSet MapSkillToEmployee(SkillSet skillSet);

        IList<int> GetSkillIds(User user);

        bool UserIsActive(User user);

        string GetUserTitle(User user);

        string GetResourceType(User user);
    }
}
