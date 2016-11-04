using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;


namespace Silicus.Ensure.Services.Interfaces
{
    public interface ISkillService
    {
        IEnumerable<Skill> GetSkillDetails();

        int Add(Skill Tag);

        void Update(Skill Tag);

        void Delete(Skill Tag);
    }
}
