using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest
{
    public abstract class MultiThreadTest:TestItem
    {

        private Stack<WaitItem> mPool = new Stack<WaitItem>(256);

        private WaitItem Pop()
        {
            lock (mPool)
            {
                if (mPool.Count > 0)
                    return mPool.Pop();
                return new WaitItem();
            }
        }

        private void Push(WaitItem item)
        {
            lock (mPool)
            {
                mPool.Push(item);
            }
        }

        protected abstract int GetCount();

        protected override void OnExecute()
        {

            int count = GetCount();
            List<WaitItem> waitItems = new List<WaitItem>();
            int index = 0;
            while (count > 0)
            {
                WaitItem wi = Pop();
                wi.Index = index;
                waitItems.Add(wi);
                index++;
                count--;
                if (waitItems.Count >= Threads)
                {
                    RunItems(waitItems);
                }
                
            }
            if (waitItems.Count > 0)
                RunItems(waitItems);
        }

        private void RunItems(List<WaitItem> waitItems)
        {
            try
            {
                System.Threading.ManualResetEvent[] mre = new System.Threading.ManualResetEvent[waitItems.Count];
                for (int i = 0; i < waitItems.Count; i++)
                {
                    mre[i] = waitItems[i].MRE;
                }
                foreach (WaitItem item in waitItems)
                {
                    item.MRE.Reset();
                    System.Threading.ThreadPool.QueueUserWorkItem(ThreadExecute, item);
                }
                System.Threading.WaitHandle.WaitAll(mre);
                foreach (WaitItem item in waitItems)
                {
                    if (item.Error != null)
                        throw item.Error;
                }
            }
            finally
            {
                foreach (WaitItem item in waitItems)
                {
                    Push(item);
                }
                waitItems.Clear();
            }
        }

        private void ThreadExecute(object state)
        {
            WaitItem wi = (WaitItem)state;
            try
            {
                OnThreadExecute(wi.Index);
            }
            catch (Exception e_)
            {
                wi.Error = e_;
            }
            finally
            {
                wi.MRE.Set();
            }
        }

        protected abstract void OnThreadExecute(int Index);

        public int Threads
        {
            get;
            set;
        }

        public class WaitItem
        {
            public System.Threading.ManualResetEvent MRE = new System.Threading.ManualResetEvent(true);

            public Exception Error;

            public int Index;

            public void Reset()
            {
                MRE.Reset();
                Error = null;
            }
        }
    }
}
