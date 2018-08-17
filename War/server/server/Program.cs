using Net.Tcp;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
   public class Program
    {
        public class SActor : ISTcpActor
        {
            //当前SActor所属角色
            public Role m_role;
            private ISTcpConnection tcpConnection;
            public void Handle(byte[] message)
            {
                string msgName;
                var msg = ProtoHelper.DecodeWithName(message,out msgName);

                //派发事件
                EventDispacher.Instance.DispachEvent(msgName, msg, m_role);
            }
            public void Send(byte[] msg)
            {
                tcpConnection.Send(msg);
            }
            
            public void Initialize(ISTcpConnection conn)
            {
                tcpConnection = conn;
                //一个SActor对应与一个角色
                m_role = new Role(this);
                //将该角色加入到所有角色集合
                lock (RoleMgr.Instance.AllRole)
                {
                    RoleMgr.Instance.AllRole.Add(m_role);
                }
                          
            }
            public void Tick(long tick)
            {

            }
            public void UnInitialize()
            {
                lock(RoleMgr.Instance.AllRole)
                {
                    RoleMgr.Instance.AllRole.Remove(m_role);
                }
                lock (RoleMgr.Instance.AllRoleDic)
                {
                    if (RoleMgr.Instance.AllRoleDic.ContainsKey(m_role.RoleId))
                    {
                        RoleMgr.Instance.AllRoleDic.Remove(m_role.RoleId);
                    }
                }
            }
        }
        /// <summary>
        /// 初始化所有控制器
        /// </summary>
        static void InitAllController()
        {
            RoleController.Instance.Init();
            RoomController.Instance.Init();
        }

        static void Main(string[] args)
        {
            InitAllController();
            var server = new TcpServer<SActor>(26001);
            server.Start();
            Console.WriteLine("服务器开启");
            Console.ReadLine();
        }
    }
}
