using System.Collections.Generic;
using Silicus.Finder.Models;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUserDetails();

        int Add(User User);

        void Update(User User);

        void Delete(User User);
    }
}
