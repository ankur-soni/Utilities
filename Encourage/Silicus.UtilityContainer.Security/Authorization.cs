using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Silicus.UtilityContainer.Security
{
    public class Authorization : IAuthorization
    {
        private readonly ICommonDataBaseContext _commonDBContext;

        public Authorization(ICommonDataBaseContext commonDBContext)
        {
            _commonDBContext = commonDBContext;
        }

        
       
        public List<string> GetRoleForUtility(string email, string utiltyName)
        {
            
                return _commonDBContext.Query<UtilityUserRoles>().Where(x => x.User.EmailAddress.ToLower() == email.ToLower() && x.Utility.Name == utiltyName).Select(x => x.Role.Name).ToList();  
           
           
        }
    }
}
