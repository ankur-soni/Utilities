using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IPositionService
    {
        IEnumerable<Position> GetAllPositionDetails();

        IEnumerable<PanelMemberDetail> GetAllPanelMemberDetails();

        IEnumerable<Position> GetPositionDetails();

        Position GetPositionById(int PositionId);

        Position GetPositionByName(string PositionName);
        
        int Add(Position Position);

        void Update(Position Position);

        void Delete(Position Position);

        IEnumerable<RecruiterMembersDetail> GetAllRecruiterMemberDetails();
        
    }
}
