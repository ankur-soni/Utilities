using Aspose.Cells;
using Silicus.Finder.Entities;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Comparable.SkillSetComparable;
using Silicus.Finder.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Services
{
    public class SkillSetService : ISkillSetService
    {
        private readonly IDataContext _context;
        private readonly ICommonDataBaseContext _commonDBContext;
        private readonly ICommonMapper _commonMapperService;

        public SkillSetService(IDataContextFactory dataContextFactory, ICommonMapper mapper)
        {
            _commonMapperService = mapper;
            _context = dataContextFactory.Create(ConnectionType.Ip);
            _commonDBContext = dataContextFactory.CreateCommonDataBaseContext();
        }

        public void Add(SkillSet skillSet)
        {
            _context.Add(skillSet);
        }

        public List<SkillSet> GetAllSkills()
        {
            var skillSetList = new List<SkillSet>();
            var skillList = _commonDBContext.Query<Skill>().ToList();
            foreach (var skill in skillList)
            {
                skillSetList.Add(_commonMapperService.MapSkillToSkillSet(skill));
            }
            SkillSetSortByName skillSetSortByName = new SkillSetSortByName();
            skillSetList.Sort(skillSetSortByName);
            return skillSetList;
        }

        public void DeleteSkillSet(int skillSetId)
        {
            var skillset = _context.Query<SkillSet>().FirstOrDefault(p => p.SkillSetId == skillSetId);
            _context.Delete<SkillSet>(skillset);
        }

        public SkillSet GetSkillSetById(int skillSetId)
        {
            return _context.Query<SkillSet>().Where(skill => skill.SkillSetId == skillSetId).Single();

        }

        public IEnumerable<SkillSet> GetSkillSetListByName(string name)
        {
            List<SkillSet> skillsetList = new List<SkillSet>();
            if (name != null)
            {
                string _name = name.Trim().ToLower();
                skillsetList = _context.Query<SkillSet>().Where((skill => skill.Name.ToLower().Contains(_name))).ToList();
            }
            SkillSetSortByName skillSetSortByName = new SkillSetSortByName();
            skillsetList.Sort(skillSetSortByName);
            return skillsetList;
        }

        public void EditSkillSet(SkillSet selectedSkillSet)
        {
            _context.Update<SkillSet>(selectedSkillSet);
        }

        public bool CheckRedudanceForSkillSet(string skillname)
        {
           var allSkill = GetAllSkills();
           foreach(var skill in allSkill)
           {
               if (skill.Name.ToLower() == skillname.ToLower())
                   return true;
           }
           return false;
        }

        public List<string> AddAllSkills(List<SkillSet> skills)
        {
            var count = 0;
            var skillNameFailedToAdd = new List<string>();
            var allSkill = GetAllSkills();

            foreach (SkillSet skill in skills)
            {
                try
                {
                    var existingAuthorCount = GetAllSkills().Count(a => a.Name == skill.Name);
                    if (existingAuthorCount == 0)
                    {
                        _context.Add<SkillSet>(skill);
                        count++;
                    }
                    else
                    {
                        skillNameFailedToAdd.Add(skill.Name);
                    }
                    
                }
                catch (Exception ex)
                {
                    skillNameFailedToAdd.Add(skill.Name);
                }
            }
            return skillNameFailedToAdd;
        }

        public List<SkillSet> ImportSkillsFromExcel(string path)
        {
            LoadOptions loadOptionForXlsx = new LoadOptions(LoadFormat.Xlsx);
            Workbook workbook = new Workbook(path, loadOptionForXlsx);
            char column = 'A';
            var skill = new SkillSet();
            var skills = new List<SkillSet>();
            var rowcount = workbook.Worksheets["Sheet1"].Cells.MaxDataRow;
            for (int index = 2; index <= rowcount + 1; )
            {
                var cell = workbook.Worksheets["Sheet1"].Cells[column + index.ToString()];
                switch (column)
                {
                    case 'A':
                        skill.Name = cell.StringValue;
                        ++column;
                        break;
                    case 'B':
                        skill.Description = cell.StringValue;
                        skills.Add(skill);
                        skill = new SkillSet();
                        ++index;
                        column = 'A';
                        break;
                }
            }
            return skills;
        }
    }
}
