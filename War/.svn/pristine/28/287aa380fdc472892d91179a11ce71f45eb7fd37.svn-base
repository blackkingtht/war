using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Tcp
{
    public interface ISTcpActor
    {
        /// <summary>
        /// 初始化连接回调
        /// </summary>
        /// <param name="conn"></param>
        void Initialize(ISTcpConnection conn);
        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="message"></param>
        void Handle(byte[] message);
        /// <summary>
        /// 服务器帧回调每大约10毫秒调用1次，与Handle同线程 且再handle之后触发.tick 是当帧的微秒数
        /// </summary>
        void Tick(long tick);
        /// <summary>
        /// 连接关闭回调
        /// </summary>
        void UnInitialize();
    }
}
