using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public class Room_1v1 : AbstractRoom
    {
        public Room_1v1(int id)
        {
            roomStatus = EnumRoomStatus.Prepare;
            roomId = id;
            roleList = new List<Role>();
        }
        private int roomId;

        private List<Role> roleList;

        private EnumRoomStatus roomStatus;

        protected override int MaxRoleNum { get { return Consts.maxRoleNumIn1V1; } }

        protected override List<Role> RoleList { get{return roleList;} }

        public override EnumRoomStatus RoomStatus { get { return roomStatus; } }

        public override int RoomId { get { return roomId; } }
    }
}
