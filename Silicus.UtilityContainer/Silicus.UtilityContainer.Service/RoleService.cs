using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Services.Interfaces;

namespace Silicus.UtilityContainer.Services
{
    public class RoleService:IRoleService
    {
        private readonly ICommonDataBaseContext _commmonDBContext;

        public RoleService(IDataContextFactory dataContextFactory)
        {
            _commmonDBContext = dataContextFactory.CreateCommonDBContext();
        }

        public List<Role> GetAllRoles()
        {
            return _commmonDBContext.Query<Role>().OrderBy(role => role.Name).ToList();
        }

        public Role GetRoleByRoleName(string RoleName)
        {
            return _commmonDBContext.Query<Role>().Where(role => role.Name == RoleName).FirstOrDefault();
        }

        public string GetRoleName(int roleId)
        {
            return _commmonDBContext.Query<Role>().Where(role => role.ID == roleId).FirstOrDefault().Name;
        }
    }
}
