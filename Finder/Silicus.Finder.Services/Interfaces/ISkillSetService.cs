using Silicus.Finder.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.Finder.Services.Interfaces
{

    public interface ISkillSetService
    {
        //int Add(SkillSet SkillSet);
        //List<SkillSet> GetAllSkillSet();

        void Add(SkillSet skillSet);
        List<SkillSet> GetAllSkills();
        void DeleteSkillSet(int skillSetId);
        SkillSet GetSkillSetById(int skillSetId);
        IEnumerable<SkillSet> GetSkillSetListByName(string name);
        void EditSkillSet(SkillSet selectedSkillSet);
        bool CheckRedudanceForSkillSet(string skillname);
        List<SkillSet> ImportSkillsFromExcel(string path);
        List<string> AddAllSkills(List<SkillSet> skills);
    }
}
