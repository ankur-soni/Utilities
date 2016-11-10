using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IPositionService
    {
        IEnumerable<Position> GetPositionDetails();

        Position GetPositionById(int PositionId);
        
        int Add(Position Position);

        void Update(Position Position);
    }
}
