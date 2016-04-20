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
        Employee MapUserToEmployee(User user);
        Project MapEngagementToProject(Engagement engagement);
        SkillSet MapSkillToSkillSet(Skill skill);
        ICommonDataBaseContext GetCommonDataBAseContext();
        Project MapBasicPropertiesOfEngagementToProject(Engagement engagement);
        IList<int> EngagementIds(User user);
        List<Engagement> EmployeeProjects(IList<int> engagementIds);
    }
}
