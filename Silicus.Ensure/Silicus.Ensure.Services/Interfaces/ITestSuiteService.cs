using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface ITestSuiteService
    {
        IEnumerable<TestSuite> GetTestSuiteDetails();

        int Add(TestSuite TestSuite);

        void Update(TestSuite TestSuite);

        void Delete(TestSuite TestSuite);
    }
}
