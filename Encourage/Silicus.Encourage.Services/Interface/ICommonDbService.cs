using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface ICommonDbService
    {
        UtilityContainer.Entities.ICommonDataBaseContext GetCommonDataBaseContext();
        string FindDisplayNameFromEmail(string email);
        List<User> GetUserWithMultipleRoles();
    }
}
