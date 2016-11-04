using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class SkillService:ISkillService
    {
        private readonly IDataContext _context;

        public SkillService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<Skill> GetSkillDetails()
        {
            return _context.Query<Skill>();
        }

        public int Add(Skill Skill)
        {
            _context.Add(Skill);
            return Skill.SkillId;
        }

        public void Update(Skill Skill)
        {
            if (Skill.SkillName != null)
            {
                _context.Update(Skill);
            }
        }

        public void Delete(Skill Skill)
        {
            if (Skill.SkillName != null)
            {
                _context.Delete(Skill);
            }
        }
    }
}
