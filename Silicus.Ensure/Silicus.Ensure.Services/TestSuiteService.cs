using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class TestSuiteService:ITestSuiteService
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
                _context.Update(TestSuite);
            }
        }

        public void Delete(TestSuite TestSuite)
        {
            if (TestSuite.TestSuiteName != null)
            {
                _context.Delete(TestSuite);
            }
        }
    }
}
