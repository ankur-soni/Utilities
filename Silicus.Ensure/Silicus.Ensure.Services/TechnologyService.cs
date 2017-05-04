using Silicus.Ensure.Entities;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Ensure.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly IDataContext _context;

        public TechnologyService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }
        public IEnumerable<TechnologyBusinessModel> GetAllTechnologies()
        {
            return _context.Query<Technology>().Select(
                tech => new TechnologyBusinessModel
                {
                    TechnologyId = tech.TechnologyId,
                    TechnologyName = tech.TechnologyName,
                    Description = tech.Description,
                    CreatedBy = tech.CreatedBy,
                    CreatedDate = tech.CreatedDate,
                    ModifiedBy = tech.ModifiedBy,
                    ModifiedDate = tech.ModifiedDate,
                    IsActive = tech.IsActive
                }
                );

        }

        public IDictionary<int, int> GetAllTechnologiesWithQuestionCount(int userId)
        {
            return
           (from tech in _context.Query<Technology>()
            join ques in _context.Query<Question>()
                        on tech.TechnologyId equals ques.TechnologyId
            where ques.Status != QuestionStatus.Approved && !ques.IsDeleted
            && ((ques.ModifiedBy==null && ques.CreatedBy!=userId) || (ques.ModifiedBy != userId))
            group ques by ques.TechnologyId into grouped
            select new
            {
                Count = grouped.Count(),
                TechnologyId = grouped.Key
            }).ToDictionary(mc => mc.TechnologyId,
                                 mc => mc.Count);

        }


        public int? Add(TechnologyBusinessModel technology)
        {
            var technologyEntity = TechnologyBusinessModelToEntity(technology);
            if (technologyEntity != null)
            {
                _context.Add(technologyEntity);
                return technologyEntity.TechnologyId;
            }
            return null;
        }

        public int? Update(TechnologyBusinessModel technology)
        {
            var technologyEntity = TechnologyBusinessModelToEntity(technology);
            if (technologyEntity != null)
            {
                var existingTechnology = GetTechnologyById(technology.TechnologyId);
                technologyEntity.CreatedDate = existingTechnology.CreatedDate;
                _context.Update(technologyEntity);
                return technologyEntity.TechnologyId;
            }
            return null;
        }

        public TechnologyBusinessModel GetTechnologyByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var technology = GetAllTechnologies().SingleOrDefault(tech => tech.TechnologyName.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
                return technology;
            }
            return null;
        }

        public bool IsTechnologyAssosiatedWithQuetion(string technologyName)
        {
            if (!string.IsNullOrWhiteSpace(technologyName))
            {
                var technology = _context.Query<Technology>().FirstOrDefault(y => y.TechnologyName.Trim().Equals(technologyName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (technology != null)
                {
                    return _context.Query<Question>().Any(y => y.TechnologyId == technology.TechnologyId);
                }
            }

            return false;
        }
        public TechnologyBusinessModel GetTechnologyById(int technologyId)
        {
            return _context.Query<Technology>().Where(y => y.TechnologyId == technologyId).Select(
                 tech => new TechnologyBusinessModel
                 {
                     TechnologyId = tech.TechnologyId,
                     TechnologyName = tech.TechnologyName,
                     Description = tech.Description,
                     CreatedBy = tech.CreatedBy,
                     CreatedDate = tech.CreatedDate,
                     ModifiedBy = tech.ModifiedBy,
                     ModifiedDate = tech.ModifiedDate,
                     IsActive = tech.IsActive
                 }
                 ).FirstOrDefault();
        }
        private Technology TechnologyBusinessModelToEntity(TechnologyBusinessModel technology)
        {
            if (technology == null)
            {
                return null;
            }
            return new Technology
            {
                TechnologyId = technology.TechnologyId,
                TechnologyName = technology.TechnologyName,
                Description = technology.Description,
                CreatedBy = technology.CreatedBy,
                CreatedDate = technology.CreatedDate,
                ModifiedBy = technology.ModifiedBy,
                ModifiedDate = technology.ModifiedDate,
                IsActive = technology.IsActive
            };
        }
    }
}
