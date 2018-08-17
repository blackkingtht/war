using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Net.Tcp
{
    public interface ICTcpActor
    {
        /// <summary>
        /// 连接初始化回调
        /// </summary>
        /// <param name="conn"></param>
        void Initialize(ICTcpConnection conn);
        /// <summary>
        /// Connect 回调
        /// </summary>
        /// <param name="err"></param>
        void OnConnected(SocketError err);
        /// <summary>
        /// 断开连接回调
        /// </summary>
        /// <param name="err"></param>
        void OnDisconnected(SocketError err);
        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="message"></param>
        void Handle(byte[] message);
    }
}
