using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using Silicus.UtilityContainer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Services
{
    public class UtilityUserRoleService : IUtilityUserRoleService
    {
        private readonly ICommonDataBaseContext _commmonDBContext;

        public UtilityUserRoleService(IDataContextFactory dataContextFactory)
        {
            _commmonDBContext = dataContextFactory.CreateCommonDBContext();
        }

        public List<UtilityUserRoles> GetAllRolesForUser(string userName)
        {
            return _commmonDBContext.Query<UtilityUserRoles>().Where(x => x.User.UserName == userName).ToList();
        }

        public List<UtilityUserRoles> GetAllUserRolesForUtility(int utilityId)
        {
            return _commmonDBContext.Query<UtilityUserRoles>().Where(x => x.UtilityId == utilityId).ToList();
        }
    }
}
