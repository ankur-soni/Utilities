using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class TestSuiteService : ITestSuiteService
    {
        private readonly IDataContext _context;

        public TestSuiteService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<TestSuite> GetTestSuiteDetails()
        {
            return _context.Query<TestSuite>();
        }

        public int Add(TestSuite TestSuite)
        {
            _context.Add(TestSuite);
            return TestSuite.TestSuiteId;
        }

        public void Update(TestSuite TestSuite)
        {
            if (TestSuite.TestSuiteName != null)
            {
                var testSuiteTags = _context.Query<TestSuite>().Where(x => x.TestSuiteId == TestSuite.TestSuiteId).Select(x => x.TestSuiteTags).SingleOrDefault();
                _context.Update(TestSuite);
            }
        }

        public void Delete(TestSuite TestSuite)
        {
            if (TestSuite.TestSuiteName != null)
            {
                TestSuite.IsDeleted = true;
                TestSuite.ModifiedDate = System.DateTime.UtcNow;
                _context.Update(TestSuite);
            }
        }

        public IEnumerable<UserTestSuite> GetUserTestSuite()
        {
            return _context.Query<UserTestSuite>();
        }

        public int AddUserTestSuite(UserTestSuite UserTestSuite)
        {
            _context.Add(UserTestSuite);
            return UserTestSuite.UserTestSuiteId;
        }

        public void UpdateUserTestSuite(UserTestSuite UserTestSuite)
        {
            if (UserTestSuite.UserId > 0)
            {                
                _context.Update(UserTestSuite);
            }
        }

        public void DeleteUserTestSuite(UserTestSuite UserTestSuite)
        {
            if (UserTestSuite.UserId > 0)
            {
                _context.Delete(UserTestSuite);
            }
        }

        public TestSuite GetTestSuitById(int testSuiteId)
        {
            return _context.Query<TestSuite>().Where(x => x.TestSuiteId == testSuiteId).FirstOrDefault();
        }



        public UserTestSuite GetUserTestSuiteId(int userTestSuiteId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.UserTestSuiteId == userTestSuiteId).FirstOrDefault();
        }


        public void UpdateUserTestDetails(UserTestDetails UserTestDetails)
        {
            _context.Update(UserTestDetails);

        }
    }
}
