using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Tcp
{
    public interface ISTcpConnection
    {
        /// <summary>
        /// 当前连接的客户端IP
        /// </summary>
        string IP
        {
            get;
        }
        /// <summary>
        /// 当前连接的客户端端口
        /// </summary>
        int Port
        {
            get;
        }
        /// <summary>
        /// 向客户端发送二进制消息
        /// </summary>
        /// <param name="message"></param>
        void Send(byte[] message);
        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
    }
}
