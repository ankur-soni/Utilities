using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IPanelService  
    {
        IEnumerable<Panel> GetAllPanelDetails();

        IEnumerable<Panel> GetPanelDetails();

        Panel GetPositionById(int PanelId);

        Panel GetPositionByName(string Panel);

        int Add(Panel Panel);

        void Update(Panel Panel);

        void Delete(Panel Panel);
    }
}
