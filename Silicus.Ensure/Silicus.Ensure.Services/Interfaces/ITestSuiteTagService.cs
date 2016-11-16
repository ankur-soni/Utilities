using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface ITestSuiteTagService
    {
        IEnumerable<TestSuiteTag> GetTestSuiteTagDetails();

        void AddAll(IEnumerable<TestSuiteTag> TestSuiteTag);

        void DeleteAll(IEnumerable<TestSuiteTag> TestSuiteTag); 
    }
}
