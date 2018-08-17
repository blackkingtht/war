using System;
using System.Collections.Generic;
using System.Text;
using static server.Program;

namespace server
{
   public class Role
    {
        #region 属性
        public SActor Client_SActor { get; set; }
        public int UserName { get; set; }
        public RoleTempData roleTempData;
        public string NickName { get; set; }
        public int RoleId { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SActor"></param>
        public Role(SActor SActor)
        {
            Client_SActor = SActor;
            roleTempData = new RoleTempData();
        }
        #endregion
    }
    public class RoleTempData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public  RoleTempData()
        {
            status = EnumRoleStatus.None;
        }
        /// <summary>
        /// 角色状态
        /// </summary>
        public  EnumRoleStatus status;

        /// <summary>
        /// 玩家阵营
        /// </summary>
        public int Camp = 1;

        /// <summary>
        /// 玩家所在房间
        /// </summary>
        public  AbstractRoom room;

        /// <summary>
        /// 玩家是否是房主
        /// </summary>
        public bool isOwner=false;
    }
}
