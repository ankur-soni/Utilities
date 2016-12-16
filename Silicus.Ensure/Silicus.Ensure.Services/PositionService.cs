using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class PositionService : IPositionService
    {
        private readonly IDataContext _context;

        public PositionService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<Position> GetPositionDetails()
        {
            return _context.Query<Position>();
        }

        public Position GetPositionById(int PositionId)
        {
            return _context.Query<Position>().Where(x => x.PositionId == PositionId).FirstOrDefault();
        }

        public Position GetPositionByName(string PositionName)
        {
            return _context.Query<Position>().Where(x => x.PositionName.ToLower() == PositionName.ToLower()).FirstOrDefault();
        }

        public int Add(Position Position)
        {
            _context.Add(Position);
            return Position.PositionId;
        }

        public void Update(Position Position)
        {
            if (Position.PositionName != null)
            {
                _context.Update(Position);
            }
        }

        public void Delete(Position Position)
        {
            if (Position.PositionId > 0)
            {
                Position.IsDeleted = true;
                _context.Update(Position);
            }
        }
    }
}
