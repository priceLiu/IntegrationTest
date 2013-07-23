using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest.App
{
    class TestDemo:IntegrationTest.TestItem
    {

        protected override void OnExecute()
        {
            System.Threading.Thread.Sleep(1);
            string id = Guid.NewGuid().ToString("N");
        }
    }
}
