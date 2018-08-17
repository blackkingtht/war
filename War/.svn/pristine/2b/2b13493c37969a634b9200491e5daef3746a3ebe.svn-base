using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public abstract class AbstractRoom
    {
        /// <summary>
        /// 房间内最大人数
        /// </summary>
        protected abstract int MaxRoleNum { get; }

        /// <summary>
        /// 房间内玩家列表
        /// </summary>
        protected abstract List<Role> RoleList { get; }

        /// <summary>
        /// 房间当前状态
        /// </summary>
        public abstract EnumRoomStatus RoomStatus { get; }

        public abstract int RoomId{ get; }

        public bool AddRole(Role role)
        {
            //避免多线程同时修改list.
            lock (RoleList)
            {
                if (RoleList.Count >= MaxRoleNum)
                    return false;
                //初始化角色在房间中的临时数据
                RoleTempData roleTempData = role.roleTempData;
                roleTempData.room = this;
                roleTempData.status = EnumRoleStatus.Room;
                roleTempData.Camp = SelectCamp();
                if (RoleList.Count == 0)
                    roleTempData.isOwner = true;
                RoleList.Add(role);
            }
            return true;
        }

        public void RemoveRole(Role role)
        {
            //避免多线程同时修改list.
            lock (RoleList)
            {
                if (!RoleList.Contains(role))
                    return;
                RoleTempData roleTempData = role.roleTempData;
                roleTempData.room = null;
                roleTempData.status = EnumRoleStatus.None;
                roleTempData.Camp = 1;
                RoleList.Remove(role);
                //如果玩家当前是房主，那么退出的时候更新房主信息。
                if (roleTempData.isOwner)
                {
                    UpdateOwner();
                    roleTempData.isOwner = false;
                }
            }
        }
        /// <summary>
        /// 当房主退出时，选择一个新房主。
        /// </summary>
        public void UpdateOwner()
        {
            lock (RoleList)
            {
                if (RoleList.Count == 0) return;
                  RoleList[0].roleTempData.isOwner = true;
            }
        }

        /// <summary>
        /// 向房间内角色广播角色信息
        /// </summary>
        /// <param name="msg"></param>
        public void BroadCastRoleInfoInTheRoom()
        {
            Dictionary<int, mmopb.p_roleInfoInRoom> dic = new Dictionary<int, mmopb.p_roleInfoInRoom>();
            lock (RoleList)
            {
                foreach (var role in RoleList)
                {
                    mmopb.p_roleInfoInRoom info = new mmopb.p_roleInfoInRoom();
                    info.camp = role.roleTempData.Camp;
                    info.isOwner = role.roleTempData.isOwner;
                    info.nickName = role.NickName;
                    dic.Add(role.RoleId, info);
                    
                }
                foreach (Role role in RoleList)
                {
                    var handle = new mmopb.roleInfoInRoom_bcst();
                    handle.roleInfoInRoomList = dic;
                       role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(handle));
                }
            }
        } 

        /// <summary>
        /// 向房间内角色广播开始战斗消息
        /// </summary>
        public void BroadCastStartBattleInTheRoom()
        {
            long ticks=System.DateTime.Now.ToUniversalTime().Ticks/10000; //服务器当前时间毫秒数
            var startBattle_Bcst = new mmopb.startBattle_bcst();
            startBattle_Bcst.ticks = ticks;
            foreach (var role in RoleList)
            {
                role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(startBattle_Bcst));
            }
        }

        /// <summary>
        /// 根据情况选择阵营。
        /// </summary>
        /// <returns></returns>
        private int SelectCamp()
        {
            int count1 = 0;
            int count2 = 0;
            foreach (Role role in RoleList)
            {
                if (role.roleTempData.Camp == 1)
                {
                    count1++;
                }
                else
                {
                    count2++;
                }
            }
            return count1 <= count2 ? 1 : 2;
        }

        /// <summary>
        /// 返回房间人数
        /// </summary>
        /// <returns></returns>
        public int GetRoleNumInRoom()
        {
            lock (RoleList)
            {
                return RoleList.Count;
            }
        }
    }
}
