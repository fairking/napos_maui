using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Napos.Domain.Tests.Helpers
{
    public class AlphabeticalTestOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
        }
    }
}
