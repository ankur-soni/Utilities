using Silicus.UtilityContainer.Security.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Security
{
    public class UserSecurityService : IUserSecurityService
    {
        private readonly IAuthentication _authenticationService;
        public UserSecurityService(IAuthentication authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public string PasswordSignInAsync(string userName, string password)
        {
            var userAuthentication = _authenticationService.ValidateUser(userName, password);
            if (userAuthentication)
                return "Success";
            else
                return "Failure";
        }

        
    }
}
