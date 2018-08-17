using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Net.Tcp
{
    public class TcpServer<T> where T : class, ISTcpActor, new()
    {
        #region Fields
        int m_MaxClient;
        int m_ClientCount;
        Socket m_Socket;
        object m_Lock = new object();
        #endregion

        #region Properties
        public bool IsRunning {
            get;
            private set;
        }
        public IPAddress Address {
            get;
            private set;
        }
        public int Port {
            get;
            private set;
        }
        internal TcpTheadPool ThreadPool {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        public TcpServer(int listenPort)
            : this(IPAddress.Any,listenPort,1024)
        {
        }
        public TcpServer(IPEndPoint localEP)
            : this(localEP.Address,localEP.Port, 1024)
        {
        }
        public TcpServer(IPAddress localIPAddress, int listenPort, int maxClient)
        {
            Address = localIPAddress;
            Port = listenPort;
            m_MaxClient = maxClient;
            m_Socket = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ThreadPool = new TcpTheadPool(MulThread ? maxClient / 256 : 1);
        }
        #endregion

        #region Method
        /// <summary>
        /// 设置多线程,在new TcpServer之前调用
        /// </summary>
        public static bool MulThread {
            get;
            set;
        }
        public void Start(int backlog = 1024)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                m_Socket.Bind(new IPEndPoint(Address, Port));
                m_Socket.Listen(backlog);
                m_Socket.BeginAccept(new AsyncCallback(HandleAcceptConnected), m_Socket);
            }
        }
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                m_Socket.Close();
            }
        }

        void HandleAcceptConnected(IAsyncResult ar)
        {
            if (IsRunning)
            {
                var server = ar.AsyncState as Socket;
                var client = server.EndAccept(ar);
                if (m_ClientCount >= m_MaxClient)
                {
                    client.Close();
                    TcpLogger.LogError("Accept more than max " + m_MaxClient);
                }
                else
                {
                    lock (m_Lock)
                    {
                        m_ClientCount++;
                    }
                    TcpConnection<T>.AcceptConnected(this, client);
                }
                server.BeginAccept(new AsyncCallback(HandleAcceptConnected), server);
            }
        }
        internal void RemoveConnection()
        {
            lock (m_Lock)
            {
                m_ClientCount--;
            }
        }
        #endregion
    }
}
