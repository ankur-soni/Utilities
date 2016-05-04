using Silicus.UtilityContainer.Security.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Silicus.UtilityContainer.Security
{
   public class Authentication : IAuthentication
    {
        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password); ;
        }
    }
}
