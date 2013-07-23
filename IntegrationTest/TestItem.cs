using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest
{
    public abstract class TestItem
    {

        public Exception LastError
        {
            get;
            set;
        }

        public double UserTime
        {
            get;
            set;
        }

        protected abstract void OnExecute();

        public void Execute()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            try
            {
                OnExecute();
            }
            catch(Exception e)
            {
                LastError = e;
            }
            sw.Stop();
            UserTime = sw.Elapsed.TotalMilliseconds;

        }
    }
}
