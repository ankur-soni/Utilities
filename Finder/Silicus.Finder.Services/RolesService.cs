using System.Collections.Generic;
using System.Linq;
using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.UtilityContainer.Security;

namespace Silicus.Finder.Services
{
    public class RolesService : IRolesService
    {

        private readonly ICommonMapper _mapper;



        public RolesService(ICommonMapper commonMapper)
        {
            _mapper = commonMapper;
        }

        public IEnumerable<Role> GetRoleDetails()
        {
            var RoleList = _mapper.GetCommonDataBAseContext().Query<Silicus.UtilityContainer.Models.DataObjects.Role>().ToList();
            var employeeRoleList = new List<Role>();
            foreach (var role in RoleList)
            {
                employeeRoleList.Add(_mapper.MapRoleToRole(role));

            }
            return employeeRoleList;

        }

        public Role GetRoleById(int Id)
        {

            var role = _mapper.GetCommonDataBAseContext().Query<Silicus.UtilityContainer.Models.DataObjects.Role>().Where(r => r.ID == Id).SingleOrDefault();
            return _mapper.MapRoleToRole(role);
        }


        public List<string> getFindersRole(string email, string utility)
        {
            var authorization = new Authorization(_mapper.GetCommonDataBAseContext());
            return authorization.GetRoleForUtility(email, utility);
        }
    }
}

