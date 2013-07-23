using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntegrationTest
{
    public class TestCase
    {
        public TestCase(TestCaseConf conf)
        {
            mCaseConf = conf;
            mName = Type.GetType(mCaseConf.Type).Name;
        }

        private string mName;

        private TestCaseConf mCaseConf;

        private Collect mCollect = new Collect();

        private IList<TestItem> mTestedItems = new List<TestItem>();

        private IList<SecondCounter> mSecondHistory = new List<SecondCounter>();

        protected void NextTestItem()
        {
            int tick = new Random().Next() % 20 * CaseConf.Sleep;
            if (tick == 0)
                tick = CaseConf.Sleep;
            System.Threading.Thread.Sleep(tick);
        }


        private Queue<IList<TestItem>> GetTestItems()
        {
            Queue<IList<TestItem>> result = new Queue<IList<TestItem>>();
            int count = CaseConf.Count;
            IList<TestItem> tests;
            Type type = Type.GetType(CaseConf.Type);
            int length = 0;
            while (count > 0)
            {
                tests = new List<TestItem>();
                int utils = (int)(new Random().Next() % 20) * CaseConf.Units;
                if (utils == 0)
                    utils = CaseConf.Units;
                while (utils > 0 && count > 0)
                {
                    TestItem item = (TestItem)Activator.CreateInstance(type);
                    tests.Add(item);
                    count--;
                    utils--;
                    length++;
                }
                if (tests.Count > 0)
                    result.Enqueue(tests);
            }

            return result;
        }

        private bool mStart = false;

        private System.Threading.Timer mTimer;

        private int mTestCount = 0;

        private int mErrors = 0;

        private int mCompeteds = 0;

        private int mLastTestCount = 0;

        private InvokeHandler mInvokeHandler;

        private DateTime mStartTime;

        private DateTime mEndTime;

        private bool mIsCompleted = false;

        private void OnExecute(object state)
        {
            mIsCompleted = false;
            Queue<IList<TestItem>> items = GetTestItems();
            mStartTime = DateTime.Now;
            DateTime dt = mStartTime.Add(new TimeSpan(0, CaseConf.RunTime, 0));

            while (items.Count > 0 && dt > DateTime.Now && mStart)
            {
                IList<TestItem> testitems = items.Dequeue();
                ExecuteItems(testitems);
                NextTestItem();
            }
            mEndTime = DateTime.Now;
            CollectInfo.Errors = mErrors;
            CollectInfo.Completeds = mCompeteds;
            CollectInfo.EndTime = DateTime.Now;
            CollectInfo.Counters = mCaseConf.Counters;
            foreach (TestItem item in mTestedItems)
            {
                foreach (TestCounterConf counter in CollectInfo.Counters)
                {
                    if (item.UserTime >= counter.From && item.UserTime < counter.To)
                    {
                        counter.Count++;
                        break;
                    }
                }
            }
            mIsCompleted = true;
            if (TestCompleted != null)
                TestCompleted(this);
        }

        private void ExecuteItems(IList<TestItem> items)
        {

            for (int i = 0; i < items.Count; i++)
            {
                if (mInvokeHandler.Add(items[i]))
                {
                    mInvokeHandler.Execute();
                    mInvokeHandler.Clean();
                }
            }
            if (mInvokeHandler.Items.Count > 0)
            {
                mInvokeHandler.Execute();
                mInvokeHandler.Clean();
            }
        }

        private void Completed(TestItem item)
        {
            lock (mTestedItems)
            {
                TestedItems.Add(item);
                System.Threading.Interlocked.Increment(ref mTestCount);
                if (item.LastError != null)
                {
                    System.Threading.Interlocked.Increment(ref mErrors);
                }
                else
                {
                    System.Threading.Interlocked.Increment(ref mCompeteds);
                }
            }
        }

        public void Stop()
        {
            mStart = false;
        }

        public void Test()
        {
            if (mInvokeHandler == null)
                mInvokeHandler = new InvokeHandler(CaseConf.Count, Completed, CaseConf.MaxThread);
            mStart = true;
            TestedItems.Clear();
            SecondHistory.Clear();
            mTestCount = 0;
            mLastTestCount = 0;
            mCollect.StartTime = DateTime.Now;
            mCollect.Count = mCaseConf.Count;
            if (mTimer != null)
            {
                mTimer.Dispose();
            }
            mTimer = new Timer(o =>
            {
                if (mStart)
                {
                    SecondCounter scount = new SecondCounter();
                    scount.DateTime = DateTime.Now;
                    scount.Count = mTestCount - mLastTestCount;
                    mLastTestCount = mTestCount;
                    mSecondHistory.Add(scount);

                }
            }, null, 990, 990);
            System.Threading.ThreadPool.QueueUserWorkItem(OnExecute);
        }

        public TestCaseConf CaseConf
        {
            get
            {
                return mCaseConf;
            }
        }

        public Action<TestCase> TestCompleted;

        public IList<SecondCounter> SecondHistory
        {
            get
            {
                return mSecondHistory;
            }
        }

        public IList<TestItem> TestedItems
        {
            get
            {
                return mTestedItems;
            }
        }

        public Collect CollectInfo
        {
            get
            {
                return mCollect;
            }
        }

        private string GetName(string name)
        {
            int spaclength = GetLength(name);
            return (new string(' ', (20 - spaclength)) + name + ":");
        }

        private int GetLength(string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if ((int)value[i] >= 0x4E00 && (int)value[i] <= 0x9FA5)
                {
                    result += 2;
                }
                else
                {
                    result++;
                }
            }
            return result;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int scounter = 0;
            string name = mName;
            if (name.Length % 2 != 0)
                name += " ";
            sb.Append(new string('*', (60 - name.Length) / 2));
            sb.Append(name);
            sb.Append(new string('*', (60 - name.Length) / 2));
            sb.AppendLine("\r\n");
            if (mSecondHistory.Count > 0)
                scounter = mSecondHistory[mSecondHistory.Count - 1].Count;
            sb.AppendFormat("{0}{1}\r\n", GetName("测试数量"), mCaseConf.Count);
            sb.AppendFormat("{0}{1}分钟\r\n", GetName("测试时长"), mCaseConf.RunTime);
            sb.AppendFormat("{0}{1}\r\n", GetName("开始时间"), mStartTime);
            if (mIsCompleted)
            {
                sb.AppendFormat("{0}{1}\r\n", GetName("完成时间"), mEndTime);
                sb.AppendFormat("{0}{1}\r\n", GetName("耗时"), (mEndTime-mStartTime));
            }
            else
            {
                sb.AppendFormat("{0}{1}\r\n", GetName("当前时间"), DateTime.Now);
            }
            sb.AppendFormat("{0}{1}\r\n", GetName("测试线程数"), mCaseConf.MaxThread);
            sb.AppendFormat("{0}{1}-{2}毫秒\r\n", GetName("间隔测试时间"), mCaseConf.Sleep, mCaseConf.Sleep * 20);
            sb.AppendFormat("{0}{1}-{2}\r\n", GetName("测试并发区间"), mCaseConf.Units, mCaseConf.Units * 20);
            sb.AppendFormat("{0}{1}\r\n", GetName("完成情况"), mCompeteds + mErrors);
            sb.AppendFormat("{0}{1}\r\n", GetName("成功"), mCompeteds);
            sb.AppendFormat("{0}{1}\r\n", GetName("失败"), mErrors);
            if (mIsCompleted)
            {
                sb.AppendFormat("{0}\r\n", GetName("汇总"));
                foreach (TestCounterConf item in CollectInfo.Counters)
                {
                    sb.AppendFormat("{0}{1}\r\n", GetName(item.Name), item.Count);
                }
            }
            else
            {

                sb.AppendFormat("{0}{1}/s\r\n", GetName("秒请求数"), scounter);
            }
            sb.AppendLine("\r\n" + new string('*', 60) + "\r\n");
            return sb.ToString();
        }

    }

    class InvokeHandler
    {
        public InvokeHandler(int count, Action<TestItem> completed, int threadcount)
        {
            mCount = threadcount;
            mCompleted = completed;
            mMRES = new System.Threading.ManualResetEvent[threadcount];
            for (int i = 0; i < threadcount; i++)
            {
                mMRES[i] = new ManualResetEvent(true);
            }
        }

        private System.Threading.ManualResetEvent[] mMRES;

        private Action<TestItem> mCompleted;

        private int mCount;

        private List<TestItem> mItems = new List<TestItem>();

        public List<TestItem> Items
        {
            get
            {
                return mItems;
            }
        }

        public void Clean()
        {
            Items.Clear();
        }

        public bool Add(TestItem item)
        {
            Items.Add(item);
            return Items.Count == mCount;
        }

        private void OnExecute(object state)
        {
            InvokeItem ii = (InvokeItem)state;
            ii.Item.Execute();
            mCompleted(ii.Item);
            ii.MRE.Set();
        }

        public void Execute()
        {
            ManualResetEvent[] mre = new ManualResetEvent[Items.Count];
            Array.Copy(mMRES, mre, Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                InvokeItem ii = new InvokeItem();
                ii.Item = Items[i];
                ii.MRE = mre[i];
                ii.MRE.Reset();
                System.Threading.ThreadPool.QueueUserWorkItem(OnExecute, ii);
            }
            System.Threading.WaitHandle.WaitAll(mre);

        }

        public class InvokeItem
        {
            public TestItem Item
            {
                get;
                set;
            }

            public ManualResetEvent MRE
            {
                get;
                set;
            }

        }
    }
}
