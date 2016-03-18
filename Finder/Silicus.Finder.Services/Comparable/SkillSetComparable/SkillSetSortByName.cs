using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Services.Comparable.SkillSetComparable
{
    class SkillSetSortByName : IComparer<SkillSet>
    {
        public int Compare(SkillSet skillset, SkillSet _skillset)
        {
            return string.Compare(skillset.Name, _skillset.Name);
            throw new NotImplementedException();
        }
    }
}
