using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Net.Tcp
{
    internal class TcpThread
    {
        public int ThreadID
        {
            get;
            private set;
        }
        Thread m_Thread = null;
        public TcpThread(int threadID)
        {
            ThreadID = threadID;
        }
        bool m_Running = false;
        public void Start()
        {
            if (!m_Running)
            {
                m_Running = true;
                m_Thread = new Thread(Run);
                m_Thread.Start();
            }
        }
        internal class QueueItem
        {
            public ISTcpConnectionUpdate conn;
            public bool AddOrRemove; //true add , false remove
        }
        List<ISTcpConnectionUpdate> m_TcpConnections = new List<ISTcpConnectionUpdate>();
        List<ISTcpConnectionUpdate> m_TcpConnectionsToRemove = new List<ISTcpConnectionUpdate>();
        ConcurrentQueue<QueueItem> m_TcpConnectionsToUpdate = new ConcurrentQueue<QueueItem>();
        /// <summary>
        /// true add , false remove
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="AddOrRemove"></param>
        public void ConnectionUpdate(ISTcpConnectionUpdate connection,bool addOrRemove)
        {
            m_TcpConnectionsToUpdate.Enqueue(new QueueItem() {conn = connection, AddOrRemove  = addOrRemove });
        }

        void Run()
        {
            long tick;
            long now;
            long dt;
            while (m_Running)
            {
                tick = DateTime.Now.Ticks;
                Update(tick/10);
                now = DateTime.Now.Ticks;
                dt = (now - tick)/10000;
                if (dt < 10)
                {
                    Thread.Sleep((int)(10 - dt));
                }
            }
            ConnectionsStop();
        }
        void Update(long tick)
        {
            QueueItem item = null;
            bool error = false;
            while (m_TcpConnectionsToUpdate.TryDequeue(out item))
            {
                if (item.AddOrRemove)
                {
                    error = false;
                    try
                    {
                        item.conn.Initialize();
                    }
                    catch (Exception e)
                    {
                        error = true;
                        item.conn.Stop();
                        TcpLogger.LogError(e.Message);
                    }
                    if (!error)
                    {
                        m_TcpConnections.Add(item.conn);
                    }
                }
                else
                {
                    m_TcpConnections.Remove(item.conn);
                }
            }
            foreach (var conn in m_TcpConnections)
            {
                error = false;
                try
                {
                    conn.Update(tick);
                }
                catch (Exception e)
                {
                    conn.Stop();
                    TcpLogger.LogError(e.Message);
                }
                if (error)
                {
                    m_TcpConnectionsToRemove.Add(conn);
                }
            }
            if (m_TcpConnectionsToRemove.Count > 0)
            {
                foreach (var conn in m_TcpConnectionsToRemove)
                {
                    m_TcpConnections.Remove(conn);
                }
                m_TcpConnectionsToRemove.Clear();
            }
        }
        void ConnectionsStop()
        {
            foreach (var conn in m_TcpConnections)
            {
                conn.Stop();
            }
            m_TcpConnections.Clear();
        }
        public void Stop()
        {
            if (m_Running)
            {
                m_Running = false;
                m_Thread.Join();
                m_Thread = null;
            }
        }
    }
}
