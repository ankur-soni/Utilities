﻿using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface ITestSuiteService
    {
        IEnumerable<TestSuite> GetTestSuiteDetails();

        TestSuite GetTestSuitById(int testSuiteId);

        UserTestSuite GetUserTestSuiteId(int userTestSuiteId);

        UserTestSuite GetUserTestSuiteByUdi_TestSuitId(int userId,int testsuitId);

        UserTestDetails GetUserTestDetailsId(int? userTestDetailsId);

        int Add(TestSuite TestSuite);

        void Update(TestSuite TestSuite);

        void Delete(TestSuite TestSuite);

        IEnumerable<UserTestSuite> GetUserTestSuite();

        int AddUserTestSuite(UserTestSuite UserTestSuite);

        void UpdateUserTestSuite(UserTestSuite UserTestSuite);

        int AddUserTestDetails(UserTestDetails UserTestDetails);

        void UpdateUserTestDetails(UserTestDetails UserTestDetails);

        void DeleteUserTestSuite(UserTestSuite UserTestSuite);

        UserTestSuite GetUserTestSuiteByUserId(int userId);

        IEnumerable<UserTestDetails> GetUserTestDetailsListByUserTestSuitId(int userTestSuitId);

        dynamic GetUserTestDetailsByUserTestSuitId(int? userTestSuitId);

        int AssignSuite(UserTestSuite userTestSuite, TestSuite testSuite);

        void TestSuiteActivation();
    }
}
