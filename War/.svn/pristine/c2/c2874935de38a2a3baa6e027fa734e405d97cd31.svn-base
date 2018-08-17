using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class EventDispacher
    {
        #region 单例
        private static EventDispacher instance;
        public static EventDispacher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventDispacher();
                }
                return instance;
            }
        }
        #endregion

        #region  委托原型及消息委托字典
        //委托原型
        public delegate void DelegateHandler(object msg,Role role);
        //消息名称，委托字典
        private Dictionary<string, DelegateHandler> dicDelegateHandler = new Dictionary<string, DelegateHandler>();
        #endregion

        #region AddEventListener 注册事件监听
        /// <summary>
        /// 注册事件监听
        /// </summary>
        /// <param name="msgName"></param>
        public void AddEventListener(string msgName,DelegateHandler handler)
        {
            if (dicDelegateHandler.ContainsKey(msgName))
            {
                dicDelegateHandler[msgName] += handler;
            }
            else
            {
                dicDelegateHandler.Add(msgName, handler);
            }
        }
        #endregion

        #region RemoveEventListener 去除事件监听
        /// <summary>
        /// 去除事件监听
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="handler"></param>
        public void RemoveEventListener(string msgName,DelegateHandler handler)
        {
            if (dicDelegateHandler.ContainsKey(msgName))
            {
                dicDelegateHandler[msgName] -= handler;
            }
        }
        #endregion

        #region  DispachEvent 派发事件
        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="role"></param>
        /// <param name="msg"></param>
        public void DispachEvent(string msgName,object msg,Role role)
        {
            if (dicDelegateHandler.ContainsKey(msgName))
            {
                dicDelegateHandler[msgName](msg,role);
            }
        }
        #endregion
    }
}
