using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest
{
    public class Collect
    {
        public int Count
        {
            get;
            set;
        }

        public int Errors
        {
            get;
            set;
        }

        public int Completeds
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }

        public TestCounterConfCollection Counters
        {
            get;
            set;
        }

    }
}
