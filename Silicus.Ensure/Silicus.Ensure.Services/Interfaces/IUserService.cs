using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUserDetailsAll();

        IEnumerable<User> GetUserDetails();

        int Add(User User);

        User GetUserById(int userId);

        void Update(User User);

        void Delete(User User);

        User GetUserByEmail(string email);

        IEnumerable<User> GetUserByRole(string role);

        dynamic GetTestSuiteDetailsOfUser(int? userId);

        dynamic GetTestSuiteDetailsWithQuestions(int? userTestSuiteId);
    }
}
