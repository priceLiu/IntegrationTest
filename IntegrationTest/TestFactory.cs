using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest
{
    public class TestFactory
    {
        public TestFactory(string config)
        {
            mSection = (TestSection)System.Configuration.ConfigurationManager.GetSection(config);
        }

        private TestSection mSection;

        private void OnTestCompleted(TestCase value)
        {


        }

        private List<TestCase> mCases = new List<TestCase>();

        public IList<TestCase> Cases
        {
            get
            {
                return mCases;
            }
        }

        public void Run()
        {
            foreach (TestCaseConf item in mSection.Cases)
            {
                TestCase tc = new TestCase(item);
                tc.TestCompleted = OnTestCompleted;
                Cases.Add(tc);
            }
            foreach (TestCase tc in Cases)
            {
                tc.Test();
            }

        }
    }
}
