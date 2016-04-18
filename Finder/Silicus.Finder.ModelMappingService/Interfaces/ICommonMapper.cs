using Silicus.Finder.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
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
    }
}
