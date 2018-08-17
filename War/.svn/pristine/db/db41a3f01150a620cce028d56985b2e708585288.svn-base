using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace server
{
    [Serializable]
    public class UserEntity : AbstractEntity
    {
        #region 重写基类属性
        /// <summary>
        /// 主键
        /// </summary>
        public override int? PKValue
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
        #endregion

        #region 实体属性

        /// <summary>
        /// 编号
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EnumEntityStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GoldCoin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Diamond { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        #endregion
    }
}