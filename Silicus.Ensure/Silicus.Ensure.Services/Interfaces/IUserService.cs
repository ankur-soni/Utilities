using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUserDetails();

        int Add(User User);

        void Update(User User);

        void Delete(User User);
    }
}
