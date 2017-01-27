using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Test;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface ITestSuiteService
    {
        IEnumerable<TestSuite> GetTestSuiteDetails();

        TestSuite GetTestSuiteByName(string testSuiteByName);

        TestSuite GetTestSuitById(int testSuiteId);

        UserTestSuite GetUserTestSuiteId(int userTestSuiteId);

        UserTestSuite GetUserTestSuiteByUdi_TestSuitId(int userId, int testsuitId);

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

        TestDetailsBusinessModel GetUserTestDetailsByUserTestSuitId(int? userTestSuitId, int? questionNumber,int questionType);

        int AssignSuite(UserTestSuite userTestSuite, TestSuite testSuite);

        IEnumerable<Question> GetPriview(TestSuite testSuite);

        void TestSuiteActivation();

        List<int> GetAllUserIdsForTestSuite(int testSuiteId);

        QuestionNavigationBusinessModel GetNavigationDetails(int userTestSuiteId);

        int GetQuestionType(int questionId);

        TestSummaryBusinessModel GetTestSummary(int userTestSuiteId);

        bool IsAllQuestionEvaluated(int? userTestSuitId);
    }
}
