using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public  class RoomController : Singleton<RoomController>, IDisposable
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            EventDispacher.Instance.AddEventListener("roomList_req", OnRoomList);
            EventDispacher.Instance.AddEventListener("creteRoom_req", OnCreateRoom);
            EventDispacher.Instance.AddEventListener("enterRoom_req", OnEnterRoom);
            EventDispacher.Instance.AddEventListener("leaveRoom_req", OnLeaveRoom);
            EventDispacher.Instance.AddEventListener("startBattle_req", OnStartBattle);
        }

      

        #region 处理请求房间列表消息
        /// <summary>
        /// 接收客户端请求房间列表的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnRoomList(object msg, Role role)
        {
            Console.WriteLine("接收客户端请求房间列表的消息");
            RoomListReturn(role);
        }

        /// <summary>
        /// 服务器返回请求房间列表消息
        /// </summary>
        /// <param name="role"></param>
        private void RoomListReturn(Role role)
        {
            var roomList_ack = new mmopb.roomList_ack();
            roomList_ack.roomList = RoomMgr.Instance.GetRoomInfoDic();
            role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(roomList_ack));
            Console.WriteLine("服务器返回请求房间列表消息");
        }

        #endregion

        #region 处理创建房间消息
        /// <summary>
        /// 接收客户端创建房间的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnCreateRoom(object msg, Role role)
        {
            Console.WriteLine("接收客户端创建房间的消息");
            //创建一个房间。
            RoomMgr.Instance.CreateRoom(role);
            CreateRoomReturn(role);
        }

        /// <summary>
        /// 服务器返回创建房间消息
        /// </summary>
        /// <param name="role"></param>
        private void CreateRoomReturn(Role role)
        {
            var create_ack = new mmopb.createRoom_ack();
            create_ack.camp = role.roleTempData.Camp;
            create_ack.isOwner = role.roleTempData.isOwner;
            create_ack.nickName = role.NickName;
            role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(create_ack));
            Console.WriteLine("服务器返回创建房间消息");
        }

        #endregion

        #region 处理进入房间消息
        /// <summary>
        /// 接收客户端进入房间的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnEnterRoom(object msg, Role role)
        {
            Console.WriteLine("接收客户端进入房间的消息");
            bool isSuc = false;
            var handle= msg as mmopb.enterRoom_req;
            //进入房间
            if (RoomMgr.Instance.EnterRoom(role, handle.roomId))
            {
                isSuc = true;           
            }
            else
            {
                isSuc = false;
            }
            EnterRoomReturn(role,isSuc);
        }

        /// <summary>
        /// 服务器返回进入房间消息
        /// </summary>
        /// <param name="role"></param>
        private void EnterRoomReturn(Role role,bool isSuc)
        {
            var enterRoom_ack = new mmopb.enterRoom_ack();
            if (isSuc)
            {               
                enterRoom_ack.isSuc = true;
                role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(enterRoom_ack));
                //向该玩家房间所有人广播
                role.roleTempData.room.BroadCastRoleInfoInTheRoom();
            }
            else
            {
                enterRoom_ack.isSuc = false;
                role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(enterRoom_ack));
            }
            Console.WriteLine("服务器返回进入房间消息");
        }
        #endregion

        #region 处理离开房间消息
        /// <summary>
        /// 接收客户端离开房间消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnLeaveRoom(object msg, Role role)
        {
            Console.WriteLine("接收客户端离开房间消息");
            bool isSuc = false;
            //离开房间
            if (RoomMgr.Instance.LeaveRoom(role))
            {
                isSuc = true;
            }
            else
            {
                isSuc = false;
            }
            LeaveRoomReturn(role,isSuc);
        }

        /// <summary>
        /// 服务器返回离开房间消息
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isSuc"></param>
        private void LeaveRoomReturn(Role role,bool isSuc)
        {
            Console.WriteLine("服务器返回离开房间消息");
            var leaveRoom_ack = new mmopb.leaveRoom_ack();
            if (isSuc)
            {
                leaveRoom_ack.isSuc = true;
                role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(leaveRoom_ack));
                //向该玩家房间所有人广播
                Console.WriteLine("向该玩家房间所有人广播");
                role.roleTempData.room.BroadCastRoleInfoInTheRoom();
            }
            else
            {
                leaveRoom_ack.isSuc = false;
                role.Client_SActor.Send(ProtoBuf.ProtoHelper.EncodeWithName(leaveRoom_ack));
            }
        }
        #endregion

        #region 处理开始战斗消息
        /// <summary>
        /// 接收客户端开始战斗消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnStartBattle(object msg, Role role)
        {
            
        }

        /// <summary>
        /// 服务器向房间内玩家广播开始战斗消息
        /// </summary>
        private void BroadCastStartBattle(Role role)
        {
            
        }

        #endregion
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            EventDispacher.Instance.RemoveEventListener("roomList_req", OnRoomList);
            EventDispacher.Instance.RemoveEventListener("creteRoom_req", OnCreateRoom);
            EventDispacher.Instance.RemoveEventListener("enterRoom_req", OnEnterRoom);
            EventDispacher.Instance.RemoveEventListener("leaveRoom_req", OnLeaveRoom);
        }
    }
}
