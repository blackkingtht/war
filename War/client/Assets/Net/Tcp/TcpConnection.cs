using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
using Net.Crypto;

namespace Net.Tcp
{
    public class TcpConnection<T> : ISTcpConnection,ISTcpConnectionUpdate where T : class,ISTcpActor,new()
    {
        #region Fields
        ConcurrentQueue<byte[]> m_RecvQueue;
        Socket m_ClientSocket;
        TcpServer<T> m_Server;
        T m_IActor;
        #endregion

        #region TcpEvent
        ConcurrentQueue<TcpEvent> m_TcpEvents = new ConcurrentQueue<TcpEvent>();
        void AddEvent(TcpEventType _type, SocketError _error, string _message)
        {
            m_TcpEvents.Enqueue(new TcpEvent() { type = _type, error = _error, message = _message });
        }
        #endregion

        #region Properties
        public bool IsRunning {
            get;
            private set;
        }
        public int ThreadID {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        protected TcpConnection(TcpServer<T> server,Socket sock)
        {
            m_Server = server;
            m_ClientSocket = sock;
        }
        #endregion
        #region Create
        public static void AcceptConnected(TcpServer<T> server, Socket sock) {
            var conn = new TcpConnection<T>(server, sock);
            var ep = (IPEndPoint)(sock.RemoteEndPoint);
            conn.IP = ep.Address.ToString();
            conn.Port = ep.Port;
            var thread = server.ThreadPool.GetThread();
            conn.ThreadID = thread.ThreadID;
            thread.ConnectionUpdate(conn, true);
        }

        #endregion
        #region Method
        public void Initialize()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                m_RecvQueue = new ConcurrentQueue<byte[]>();
                m_SendIdx = 0;
                m_RecvIdx = 0;
                InitAesEncryptor();
                InitAesDecryptor();
                m_IActor = new T();
                m_IActor.Initialize(this);
                m_ClientSocket.BeginReceive(m_Buffer, 0, m_Buffer.Length,SocketFlags.None,new AsyncCallback(HandleDataReceived),m_ClientSocket);
            }
        }

        public void Update(long tick)
        {
            if (!IsRunning)
            {
                return;
            }
            byte[] msg;
            while (m_RecvQueue.TryDequeue(out msg))
            {
                m_IActor?.Handle(msg);
            }
            m_IActor?.Tick(tick);
        }

        public void Stop()
        {
            Close();
        }
        public void Close()
        {
            if (IsRunning)
            {
                IsRunning = false;
                m_Server?.RemoveConnection();
                m_ClientSocket?.Shutdown(SocketShutdown.Both);
                m_ClientSocket?.Close();
                m_Buffer = null;
                m_RecvQueue = null;
                m_ClientSocket = null;
                m_Server = null;
                var actor = m_IActor;
                m_IActor = null;
                actor?.UnInitialize();
            }
        }

        public void Send(byte[] buffer)
        {
            BeginSend(buffer);
        }
		object m_EncodeLock = new object();
        void BeginSend(byte[] data)
        {
            try
            {
				lock (m_EncodeLock){
					data = TcpTools.Encode(data, m_AesEncryptor, CurSendIdx);
					m_ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), m_ClientSocket);
				}
			}
            catch (SocketException e)
            {
                Close();
                AddEvent(TcpEventType.EventSendData, e.SocketErrorCode, e.Message);
            }
        }
        void SendCallback(IAsyncResult ar)
        {
            try
            {
                var s = (Socket)ar.AsyncState;
                s.EndSend(ar);
            }
            catch (SocketException e)
            {
                Close();
                AddEvent(TcpEventType.EventSendData, e.SocketErrorCode, e.Message);
            }
        }

        void HandleDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                var client = ar.AsyncState as Socket;
                try
                {
                    int bytesRead = client.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        m_BufferReceivedSize += bytesRead;
                        TcpTools.SplitPack(ref m_Buffer, ref m_BufferReceivedSize, ref m_BufferSize, PushPack);
						client.BeginReceive(m_Buffer, m_BufferReceivedSize, m_BufferSize - m_BufferReceivedSize, SocketFlags.None, new AsyncCallback(HandleDataReceived), client);
                    }
                    else
                    {
                        Close();
                    }
                }
                catch (SocketException)
                {
                    Close();
                }
            }
        }

        void PushPack(byte[] pack)
        {
            pack = TcpTools.Decode(pack, m_AesDecryptor, ref m_RecvIdx);//解压缩，解加密
            m_RecvQueue.Enqueue(pack);
        }
        #endregion

        #region Encode&Decode
        byte m_SendIdx;
        int m_RecvIdx;
        byte CurSendIdx
        {
            get
            {
                byte idx = m_SendIdx;
                m_SendIdx++;
                if (m_SendIdx > 0x1F)
                {
                    m_SendIdx = 0;
                }
                return idx;
            }
        }

        public string IP {
            get;
            private set;
        }

        public int Port {
            get;
            private set;
        }

        static readonly byte[] AesKey = new byte[] { 1, 9, 8, 7, 0, 4, 1, 2, 1, 9, 9, 1, 0, 2, 1, 0 };
        static readonly byte[] AesIV = new byte[] { 1, 9, 8, 7, 0, 4, 1, 2, 1, 9, 9, 1, 0, 2, 1, 0 };

        AesDecryptor m_AesDecryptor;
        AesEncryptor m_AesEncryptor;

        void InitAesDecryptor()
        {
            m_AesDecryptor = new AesDecryptor(AesKey, AesIV);
        }
        void InitAesEncryptor()
        {
            m_AesEncryptor = new AesEncryptor(AesKey, AesIV);
        }

        #endregion

        #region Buffer
        const int BufferSize = 1024;
        byte[] m_Buffer = new byte[BufferSize];
        int m_BufferReceivedSize = 0;
        int m_BufferSize = BufferSize;
        #endregion
    }
}
