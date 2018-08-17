using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Tcp
{
    public interface ICTcpConnection
    {
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool Connect(string addr, int port);
        /// <summary>
        /// 向服务器发送二进制消息
        /// </summary>
        /// <param name="message"></param>
        void Send(byte[] message);
        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
    }
}
