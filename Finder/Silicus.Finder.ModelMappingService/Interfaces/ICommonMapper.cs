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
        IList<int> EngagementIds(User user);
        Employee MapUserToEmployee(User user);
        Project MapEngagementToProject(Engagement engagement);
        SkillSet MapSkillToSkillSet(Skill skill);
        ICommonDataBaseContext GetCommonDataBAseContext();
      Models.DataObjects.Role MapRoleToRole(Silicus.UtilityContainer.Models.DataObjects.Role role);
        List<Engagement> EmployeeProjects(IList<int> engagegmetIds);
         Project MapEngagementToProjectForEmployee(Engagement engagement);
          string GetUserTitle(User user);
    }
}
