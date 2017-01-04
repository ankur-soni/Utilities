using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IPanelMemberService
    {
        PanelMemberDetail GetPanelMeberDetails(int userId);
        bool UpesertPanelMeberDetail(PanelMemberDetail panelMemberDetail);
    }
}
