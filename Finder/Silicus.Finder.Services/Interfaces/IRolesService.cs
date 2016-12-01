using System.Collections.Generic;
using Silicus.Finder.Models;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Services.Interfaces
{
    public interface IRolesService
    {
        IEnumerable<Role> GetRoleDetails();

       Role GetRoleById(int Id);

       string getFindersRole(string email, string utility);
    }
}
