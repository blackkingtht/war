using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public  class ReturnValue<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnValue()
        {

        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="value"></param>
        public ReturnValue(T value):this()
        {
            Value = value;
        }

        #region 属性

        #region Id 获取和设置返回实体的Id 
        /// <summary>
        /// 获取和设置返回实体的Id
        /// </summary>
        public int Id { get; set; }
        #endregion

        #region Value 获取或设置返回值
        /// <summary>
        /// 获取或设置返回值
        /// </summary>
        public T Value { get; set; }
        #endregion

        #region  Message 获取或设置提示信息
        /// <summary>
        /// 获取或设置提示信息
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region HasError 获取或设置是否有错
        /// <summary>
        /// 获取或设置是否有错
        /// </summary>
        public bool HasError { get; set; }
        #endregion

        #region ReturnCode 获取和设置返回代码(存储过程的返回值)
        /// <summary>
        /// 获取和设置返回代码(存储过程的返回值)
        /// </summary>
        public int ReturnCode { get; set; }
        #endregion

        #endregion
    }
}
