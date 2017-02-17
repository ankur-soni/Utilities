using System.Collections.Generic;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Test;
using System;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserBusinessModel> GetUserDetailsAll();

        IEnumerable<UserBusinessModel> GetUserDetails();

        int Add(UserBusinessModel User);

        UserBusinessModel GetUserById(int userId);

        void Update(UserBusinessModel User);

        void Delete(int userId);

        UserBusinessModel GetUserByEmail(string email);

        IEnumerable<User> GetUserByRole(string role);

        dynamic GetTestSuiteDetailsOfUser(int? userId);

        dynamic GetTestSuiteDetailsWithQuestions(int? userTestSuiteId);

        IEnumerable<UserTestSuite> GetAllTestSuiteDetails();

        UserApplicationDetails GetUserApplicationDetailsById(int userApplicationId);

        void UpdateUserApplicationTestDetails(int UserApplicationDetailsId);

        CandidateInfoBusinessModel GetCandidateInfo(UserBusinessModel user);

        UserBusinessModel GetUserByUserApplicationId(int UserApplicationDetailId);

        IEnumerable<UserBusinessModel> GetCandidates(string firstName, string lastName, DateTime dob);
    }
}
