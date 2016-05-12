using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.Encourage.DAL.Interfaces;

namespace Silicus.UtilityContainer.Security
{
    public class Authorization : IAuthorization
    {
        private readonly ICommonDataBaseContext _commonDBContext;
        private readonly IEncourageDatabaseContext _encourageDBContext;

        public Authorization(ICommonDataBaseContext commonDBContext,IEncourageDatabaseContext encourageDBContext = null)
        {
            _commonDBContext = commonDBContext;
            _encourageDBContext = encourageDBContext;

        }

        
       
        public string GetRoleForUtility(string email, string utiltyName)
        {
            
                return _commonDBContext.Query<UtilityUserRoles>().Where(x => x.User.EmailAddress.ToLower() == email.ToLower() && x.Utility.Name == utiltyName).Select(x => x.Role.Name).FirstOrDefault();  
           
           
        }
    }
}
