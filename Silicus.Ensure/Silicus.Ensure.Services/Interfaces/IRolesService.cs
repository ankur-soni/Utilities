using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IRolesService
    {
        IEnumerable<Role> GetRoleDetails();

        int Add(Role Role);

        void Update(Role Role);

        void Delete(Role Role);
    }
}
