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
        private readonly SilicusUtilityContext _context;

        public RoleService()
        {
            _context = new SilicusUtilityContext();
        }
        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
