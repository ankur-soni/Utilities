using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services.Interfaces
{
    public  interface ITestSuiteTagService
    {
        IEnumerable<TestSuiteTag> GetTestSuiteTagDetails();

        void AddAll(IEnumerable<TestSuiteTag> TestSuiteTag);

        void DeleteAll(IEnumerable<TestSuiteTag> TestSuiteTag);

    }
}
