using System.Collections.Generic;
using Silicus.Finder.Models;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Services.Interfaces
{
    public interface IRolesService
    {
        IEnumerable<Role> GetRoleDetails();

        int Add(Role Role);

        void Update(Role Role);

        void Delete(Role Role);
    }
}
