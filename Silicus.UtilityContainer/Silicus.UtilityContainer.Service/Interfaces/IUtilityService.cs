using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Models.ViewModels;

namespace Silicus.UtilityContainer.Services.Interfaces
{
    public interface IUtilityService
    {
        List<Utility> GetAllUtilities();
        Utility FindUtility(int utilityId);
        List<UtilityRole> GetAllRolesForAnUtility(int utilityId);
        void SaveUtilityRole(UtilityRoleViewModel newUtilityRole);

    }
}
