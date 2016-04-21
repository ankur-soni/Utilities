using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Security.Interface
{
    public interface IUserSecurityService
    {
        string PasswordSignInAsync(string userName, string password);
    }
}
