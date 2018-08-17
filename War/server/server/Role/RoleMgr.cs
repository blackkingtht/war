using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class RoleMgr
    {
        #region 单例
        private static RoleMgr instance;
        private static object lockObj = new object();
        public static RoleMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        instance = new RoleMgr();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        RoleMgr()
        {
            m_AllRole = new List<Role>();
            m_AllRoleDic = new Dictionary<int, bool>();
        }
        #endregion

        #region 所有角色集合
        //所有角色集合
        private List<Role> m_AllRole;
        public List<Role> AllRole
        {
            get
            {
                return  m_AllRole;
            }
        }

        /// <summary>
        /// key是roleId。主要用来判断玩家是否在线
        /// </summary>
        private Dictionary<int,bool> m_AllRoleDic;
        public Dictionary<int, bool> AllRoleDic
        {
            get
            {
                return m_AllRoleDic;
            }
        }
        #endregion
    }
}
