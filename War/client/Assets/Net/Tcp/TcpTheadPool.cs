using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace Net.Tcp
{
    internal class TcpTheadPool
    {
        ConcurrentDictionary<int, TcpThread> Threads = new ConcurrentDictionary<int, TcpThread>();
        int m_PoolCount;
        int m_ThreadIndex;
        object m_IndexLock = new object();
        bool m_Stoped = false;

        public TcpTheadPool(int poolCount)
        {
            m_PoolCount = poolCount;
            m_ThreadIndex = 1;
            m_Stoped = false;
        }
        public TcpThread GetThread(int threadID = 0)
        {
            if (m_Stoped) {
                return null;
            }
            if (threadID == 0) {
                lock (m_IndexLock) {
                    threadID = m_ThreadIndex++;
                }
                threadID = threadID % m_PoolCount;
            }
            TcpThread thread;
            if (Threads.TryGetValue(threadID, out thread))
            {
                return thread;
            }

            thread = new TcpThread(threadID);
            if (Threads.TryAdd(threadID, thread)) {
                thread.Start();
                return thread;
            }
            return GetThread(threadID);
        }
        public void Stop()
        {
            m_Stoped = true;
            var keys = Threads.Keys;
            TcpThread thread = null;
            foreach (var key in keys)
            {
                if (Threads.TryRemove(key, out thread))
                {
                    thread.Stop();
                }
            }
            Thread.Sleep(100);
            if (Threads.Count > 0) {
                Stop();
            }
        }
    }
}
