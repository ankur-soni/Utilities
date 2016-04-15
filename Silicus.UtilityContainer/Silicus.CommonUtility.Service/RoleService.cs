using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainerr.Entities;
using Silicus.UtilityContainer.Services.Interfaces;

namespace Silicus.UtilityContainer.Services
{
    public class RoleService:IRoleService
    {
        private readonly ILocalDataBaseContext _localDBContext;

        public RoleService(IDataContextFactory dataContextFactory)
        {
            _localDBContext = dataContextFactory.CreateLocalDBContext();
        }

        public List<Role> GetAllRoles()
        {
            return _localDBContext.Query<Role>().ToList();
        }
    }
}
