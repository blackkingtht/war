using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public  class RoomMgr:Singleton<RoomMgr>
    {
        /// <summary>
        /// 1v1房间列表
        /// </summary>
        private  Dictionary<int,AbstractRoom> roomList1v1 = new Dictionary<int, AbstractRoom>();

        /// <summary>
        /// 房间的编号（随着房间的创建累加）
        /// </summary>
        public int roomId=0; 

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="role"></param>
        public void CreateRoom(Role role)
        {
            lock (roomList1v1)
            {
                if (role.roleTempData.status == EnumRoleStatus.Room)
                    return;
                Room_1v1 room = new Room_1v1(++roomId);
                roomList1v1.Add(roomId,room);
                room.AddRole(role);
            } 
        }
        
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="role"></param>
        public bool LeaveRoom(Role role)
        {
            lock (roomList1v1)
            {
                if (role.roleTempData.status == EnumRoleStatus.None)
                    return false;
                AbstractRoom room = role.roleTempData.room;
                room.RemoveRole(role);
                //如果移除后房间没人，则注销房间
                if (room.GetRoleNumInRoom() == 0)
                {
                    roomList1v1.Remove(room.RoomId);
                }
                return true;
            }
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="role"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public bool EnterRoom(Role role, int roomId)
        {
            lock (roomList1v1)
            {
                if (role.roleTempData.status == EnumRoleStatus.Room)
                    return false;
                foreach (var pair in roomList1v1)
                {
                    //寻找房间号相同的房间，将角色加入房间
                    if (pair.Key == roomId)
                    {
                        if (pair.Value.AddRole(role))
                            return true;                         
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取房间列表的信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,mmopb.p_roomInfo> GetRoomInfoDic()
        {
            Dictionary<int, mmopb.p_roomInfo> retDic = new Dictionary<int, mmopb.p_roomInfo>();
            lock (roomList1v1)
            {
                foreach (var pair in roomList1v1)
                {
                    mmopb.p_roomInfo roomInfo = new mmopb.p_roomInfo();
                    roomInfo.roomId = pair.Key;
                    roomInfo.roleNum = pair.Value.GetRoleNumInRoom();
                    if (pair.Value.RoomStatus == EnumRoomStatus.Fight)
                        roomInfo.isFight = true;
                    else
                        roomInfo.isFight = false;
                    retDic.Add(pair.Key, roomInfo);
                }
            }
            return retDic;
        }

    }
}
