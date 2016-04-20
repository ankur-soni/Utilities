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
        IList<int> EngagementIds(User user);
        Models.DataObjects.Role MapRoleToRole(Silicus.UtilityContainer.Models.DataObjects.Role role);
        Project MapEngagementToProject(Engagement engagement);
        Project MapBasicPropertiesOfEngagementToProject(Engagement engagement);
        SkillSet MapSkillToSkillSet(Skill skill);
        ICommonDataBaseContext GetCommonDataBAseContext();
      Models.DataObjects.Role MapRoleToRole(Silicus.UtilityContainer.Models.DataObjects.Role role);
        List<Engagement> EmployeeProjects(IList<int> engagegmetIds);
         Project MapEngagementToProjectForEmployee(Engagement engagement);
          string GetUserTitle(User user);
    }
}
