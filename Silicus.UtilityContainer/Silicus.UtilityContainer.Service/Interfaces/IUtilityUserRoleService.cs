using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Services.Interfaces
{
    public interface IUtilityUserRoleService
    {
        List<UtilityUserRoles> GetAllRolesForUser(string userName);
    }
}
