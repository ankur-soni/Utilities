using System.Collections.Generic;
using System.Linq;
using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.ModelMappingService.Interfaces;

namespace Silicus.Finder.Services
{
    public class RolesService : IRolesService
    {
        //private readonly IDataContext _context;

        private readonly ICommonMapper _mapper;
        //public RolesService(IDataContextFactory dataContextFactory)
        //{
        //    _context = dataContextFactory.Create(ConnectionType.Ip);
        //}


        public RolesService(ICommonMapper commonMapper)
        {
            _mapper = commonMapper;
        }

        //public IEnumerable<Role> GetRoleDetails()
        //{
        //    var RoleList = _mapper.GetCommonDataBAseContext().Query<Silicus.UtilityContainer.Models.DataObjects.Role>().ToList();
            
            
        //    return _mapper.

        //}

        public Role GetRoleById(int Id)
        {

           var role = _mapper.GetCommonDataBAseContext().Query<Silicus.UtilityContainer.Models.DataObjects.Role>().Where(r => r.ID == Id).SingleOrDefault();
          return  _mapper.MapRoleToRole(role);
        }

        //public int Add(Role Role)
        //{
        //    _context.Add(Role);
        //    return Role.RoleId;
        //}

        //public void Update(Role Role)
        //{
        //    if (Role.RoleName != null && Role.Description != null)
        //    {
        //        _context.Update(Role);
        //    }
        //}

        //public void Delete(Role Role)
        //{
        //    if (Role.RoleName != null && Role.Description != null)
        //    {
        //        _context.Delete(Role);
        //    }
        //}
    }
}

