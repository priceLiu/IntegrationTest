using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace IntegrationTest
{
    public class UrlTest:TestItem
    {
        public string URL
        {
            get;
            set;
        }

        public string ProxyHost
        {
            get;
            set;
        }

        protected override void OnExecute()
        {
            HttpWebRequest request =(HttpWebRequest) WebRequest.Create(URL);
            if (!string.IsNullOrEmpty(ProxyHost))
            {
                WebProxy myProxy = new WebProxy(ProxyHost);
                
                request.Proxy = myProxy;
            }
           
            using (HttpWebResponse myWebResponse = (HttpWebResponse)request.GetResponse())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(myWebResponse.GetResponseStream(),Encoding.UTF8))
                {
                    string body = reader.ReadToEnd();
                }
            }
        }
    }

    public class MultiUrlTest : MultiThreadTest
    {
        public string URLFile
        {
            get;
            set;
        }

        public string ProxyHost
        {
            get;
            set;
        }

        static System.Collections.Hashtable mTables = new System.Collections.Hashtable();

        static IList<string> GetUrlList(string file)
        {
            List<string> urls = null;
            lock (mTables)
            {
                urls=(List<string>)mTables[file];
                if (urls == null)
                {
                    urls = new List<string>();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
                    {
                        string url = reader.ReadLine();
                        while (!string.IsNullOrEmpty(url))
                        {
                            urls.Add(url);
                            url = reader.ReadLine();
                        }
                        mTables[file]=urls;
                    }
                }
            }
            return urls;

        }

        protected override int GetCount()
        {
            return GetUrlList(URLFile).Count;
        }
        
        protected override void OnExecute()
        {
           
            base.OnExecute();
        }

        protected override void OnThreadExecute(int count)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUrlList(URLFile)[count]);
            if (!string.IsNullOrEmpty(ProxyHost))
            {
                WebProxy myProxy = new WebProxy();
                Uri newUri = new Uri(ProxyHost);
                request.Proxy = myProxy;
            }
            using (HttpWebResponse myWebResponse = (HttpWebResponse)request.GetResponse())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(myWebResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string body = reader.ReadToEnd();
                }
            }
        }
    }
}
