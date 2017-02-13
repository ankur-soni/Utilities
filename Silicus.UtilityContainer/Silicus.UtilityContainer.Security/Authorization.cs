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
            var result = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.User.EmailAddress.ToLower() == email.ToLower() && x.Utility.Name.ToLower() == utiltyName.ToLower() && x.IsActive).Select(x => x.Role.Name).ToList();
            return result;
        }

        public List<string> GetNameOfContributors()
        {
            return _commonDBContext.Query<Credits>().Where(x => x.UtilityId == 3).OrderBy(x => x.Name).Select(x => x.Name).ToList();
        }
    }
}
