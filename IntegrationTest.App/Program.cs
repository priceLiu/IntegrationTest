using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IntegrationTest.App
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine(BitConverter.IsLittleEndian);
            //ServicePointManager.DefaultConnectionLimit = 1000;
            TestFactory testFactory = new TestFactory("testSection");
            testFactory.Run();
            
            while (true)
            {
                Console.Clear();
                foreach (IntegrationTest.TestCase item in testFactory.Cases)
                {
                    Console.WriteLine(item);
                }
                System.Threading.Thread.Sleep(990);
            }

            Console.Read();
            
        }

    }
}
