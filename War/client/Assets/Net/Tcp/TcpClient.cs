using System.Collections.Concurrent;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Net.Crypto;

namespace Net.Tcp
{
    public class TcpClient: ICTcpConnection
    {
        #region Queue
        ConcurrentQueue<byte[]> m_SendQueue = new ConcurrentQueue<byte[]>();
        ConcurrentQueue<byte[]> m_RecvQueue = new ConcurrentQueue<byte[]>();
        ICTcpActor m_IActor = null;
        #endregion

        #region 构造函数
        public TcpClient(ICTcpActor iActor)
        {
            m_IActor = iActor;
        }
        #endregion

        #region TcpEvent
        ConcurrentQueue<TcpEvent> m_TcpEvents = new ConcurrentQueue<TcpEvent>();
        void AddEvent(TcpEventType _type, SocketError _error, string _message)
        {
            m_TcpEvents.Enqueue(new TcpEvent() { type = _type, error = _error, message = _message });
        }
        #endregion

        #region IP
        IPAddress ParseIpAddressV6(string address)
        {
            IPAddress addrOut = null;
            if (IPAddress.TryParse(address, out addrOut))
            {
                return addrOut;
            }

            try
            {
                IPAddress[] addrList = Dns.GetHostAddresses(address);
                for (int i = 0; i < addrList.Length; i++)
                {
                    addrOut = addrList[i];
                    if (addrOut.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                AddEvent(TcpEventType.EventIPPrase, SocketError.SocketError, e.Message);
            }

            return addrOut;
        }
        public bool IsConnected
        {
            get
            {
                return !IsConnecting && ((m_Socket == null) ? false : m_Socket.Connected);
            }
        }
        protected bool IsConnecting
        {
            get;
            private set;
        }
        public bool IsRunning {
            get;
            private set;
        }
        public ICTcpActor GetActor()
        {
            return m_IActor;
        }
        public void Start()
        {
            if (!IsRunning) {
                IsRunning = true;
                m_IActor?.Initialize(this);
            }
        }

        byte m_SendIdx = 0;
        int m_RecvIdx = 0;
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

        Socket m_Socket;
        const int RECEIVE_BUFFER_LENGTH = 1024 * 1024 * 1;
        const int SEND_BUFFER_LENGTH = 1024 * 1024 * 1;
        string m_Address;
        int m_Port;
        public bool Connect(string addr, int port)
        {
            if (!IsRunning) {
                TcpLogger.LogError("Not running");
                return false;
            }
            var ip = ParseIpAddressV6(addr);
            if (ip == null)
            {
                TcpLogger.LogError("Unknown addr = " + addr);
                return false;
            }

            Close();

            TcpLogger.Log("Connect " + addr + " -> " + ip + ":" + port);

            m_Socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, SEND_BUFFER_LENGTH);
            m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, RECEIVE_BUFFER_LENGTH);

            m_Address = addr;
            m_Port = port;

            try
            {
                //m_Socket.Blocking = false;
                IsConnecting = true;
                m_Socket.BeginConnect(new IPEndPoint(ip, port), new AsyncCallback(ConnectCallback), m_Socket);
                return true;
            }
            catch (Exception e)
            {
                AddEvent(TcpEventType.EventConnected, SocketError.SocketError, e.Message);
                return false;
            }
        }
        public void Close()
        {
            if (m_Socket != null && m_Socket.Connected)
            {
                try
                {
                    //m_Socket.Disconnect(true)
                    m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    TcpLogger.LogError(e.Message);
                }
                try
                {
                    m_Socket.Close();
                }
                catch (Exception e)
                {
                    TcpLogger.LogError(e.Message);
                }
            }
            m_Socket = null;
            IsConnecting = false;
            m_RecvIdx = 0;
            m_SendIdx = 0;
            m_SendQueue = new ConcurrentQueue<byte[]>();
            m_RecvQueue = new ConcurrentQueue<byte[]>();
            InitAesEncryptor();
            InitAesDecryptor();
        }
        void ConnectCallback(IAsyncResult ar)
        {
            var res = SocketError.Success;
            var msg = "";
            try
            {
                var s = (Socket)ar.AsyncState;
                s.EndConnect(ar);
            }
            catch (SocketException e)
            {
                Close();
                res = e.SocketErrorCode;
                msg = e.Message;
            }

            if (res == SocketError.Success)
            {
                //m_Socket.Blocking = true;
                var state = new StateObject();
                state.m_Socket = m_Socket;
                m_Socket.BeginReceive(state.m_Buffer, 0, state.m_BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);

                IsConnecting = false;
                byte[] data = null;
                while (m_SendQueue.TryDequeue(out data))
                {
                    BeginSend(data);
                }
            }
            else
            {
                IsConnecting = false;
            }
            AddEvent(TcpEventType.EventConnected, res, msg);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var state = (StateObject)ar.AsyncState;
                var s = state.m_Socket;

                int bytesRead = s.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //增加读取的索引
                    state.m_BufferReceivedSize += bytesRead;
                    //拆分包
                    TcpTools.SplitPack(ref state.m_Buffer, ref state.m_BufferReceivedSize, ref state.m_BufferSize, OnRecvPack);
                    //继续接收
                    s.BeginReceive(state.m_Buffer, state.m_BufferReceivedSize, state.m_BufferSize - state.m_BufferReceivedSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    Close();
                    AddEvent(TcpEventType.EventReceiveData, SocketError.NoData, "NoData");
                }
            }
            catch (SocketException e)
            {
                Close();
                AddEvent(TcpEventType.EventReceiveData, e.SocketErrorCode, e.Message);
            }
        }

        void OnRecvPack(byte[] pack)
        {
            if (!IsConnected)
            {
                TcpLogger.LogError("Socket not connected, recvied pack will be drop.");
                return;
            }
            pack = TcpTools.Decode(pack, m_AesDecryptor, ref m_RecvIdx);//解压缩，解加密
            m_RecvQueue.Enqueue(pack);
        }

        void TryBeginSend(byte[] data)
        {
            if (!IsConnected)
            {
                m_SendQueue.Enqueue(data);
                return;
            }
            BeginSend(data);
        }
        void BeginSend(byte[] data)
        {
            try
            {
                data = TcpTools.Encode(data, m_AesEncryptor, CurSendIdx);
                m_Socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), m_Socket);
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

        /// <summary>
        /// 网络消息会被push 起来 然后 在这里派发
        /// </summary>
        public void Update()
        {
            //处理事件
            TcpEvent evt;
            while (m_TcpEvents.TryDequeue(out evt))
            {
                if (evt.type == TcpEventType.EventConnected)
                {
                    m_IActor?.OnConnected(evt.error);
                }
                else if (evt.type == TcpEventType.EventDisconnected)
                {
                    m_IActor?.OnDisconnected(evt.error);
                }
                else if (evt.error == SocketError.SocketError) {
                    m_IActor?.OnDisconnected(evt.error);
                }
            }
            byte[] msg;
            while (m_RecvQueue.TryDequeue(out msg))
            {
                m_IActor?.Handle(msg);
            }
        }

        public void Send(byte[] buffer)
        {
            TryBeginSend(buffer);
        }
        #endregion

        #region StateObject
        //存储Recv收到的数据
        class StateObject
        {
            const int BufferSize = 1024;
            public Socket m_Socket = null;
            public byte[] m_Buffer = new byte[BufferSize];
            public int m_BufferReceivedSize = 0;
            public int m_BufferSize = BufferSize;
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
    }
}
