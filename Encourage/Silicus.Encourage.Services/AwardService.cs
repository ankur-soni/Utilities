using Silicus.Encourage.DAL;
using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models.DataObjects;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class AwardService:IAwardService
    {
        private readonly IEncourageDatabaseContext _context;

        public AwardService()
        {
            IDataContextFactory contextFactory = new DataContextFactory();
            _context = contextFactory.CreateEncourageDbContext();
            _context.SaveChanges();
        }

        public IEnumerable<Models.DataObjects.Award> GetAllAwards()
        {
            return _context.Query<Award>().ToList();
        }
    }
}
