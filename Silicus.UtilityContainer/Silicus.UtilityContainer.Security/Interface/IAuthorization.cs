using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Security.Interface
{
    public interface IAuthorization
    {
        string GetRoleForUtility(string email, string utiltyName);
    }
}
