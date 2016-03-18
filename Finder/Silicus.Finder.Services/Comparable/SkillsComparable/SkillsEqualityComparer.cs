using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silicus.Finder.Models.DataObjects;
using System.Threading.Tasks;

namespace Silicus.Finder.Services.Comparable.SkillsComparable
{
    public class SkillsEqualityComparer : IEqualityComparer<SkillSet>
    {
        public int GetHashCode(SkillSet skill)
        {
            return skill.SkillSetId.GetHashCode();
        }

        public bool Equals(SkillSet skill1, SkillSet skill2)
        {
            if (object.ReferenceEquals(skill1, skill2))
                return true;
            if (skill1 == null || skill2 == null)
                return false;
            return skill1.SkillSetId.Equals(skill2.SkillSetId);
        }

    }
}
