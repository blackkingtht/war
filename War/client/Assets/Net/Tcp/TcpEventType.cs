using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Tcp
{
    internal enum TcpEventType
    {
        /// <summary>
        /// None
        /// </summary>
        EventNone = 0,
        /// <summary>
        /// 连接
        /// </summary>
        EventConnected = 1,
        /// <summary>
        /// 断开连接
        /// </summary>
        EventDisconnected = 2,
        /// <summary>
        /// ip 解析失败
        /// </summary>
        EventIPPrase = 3,
        /// <summary>
        /// 接收数据
        /// </summary>
        EventReceiveData = 4,
        /// <summary>
        /// 发送数据
        /// </summary>
        EventSendData = 5,
    }
}
