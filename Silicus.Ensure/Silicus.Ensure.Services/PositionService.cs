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
    }
}
