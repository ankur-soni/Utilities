using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class TestSuiteTagService : ITestSuiteTagService
    {
        private readonly IDataContext _context;

        public TestSuiteTagService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<TestSuiteTag> GetTestSuiteTagDetails()
        {
            return _context.Query<TestSuiteTag>();
        }

        public void AddAll(IEnumerable<TestSuiteTag> TestSuiteTag)
        {
            _context.Add(TestSuiteTag);            
        }

        public void DeleteAll(IEnumerable<TestSuiteTag> TestSuiteTag)
        {
            if (TestSuiteTag != null)
            {
                _context.DeleteAll(TestSuiteTag);
            }
        }
    }
}
