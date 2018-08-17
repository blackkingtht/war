using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public class RoleController : Singleton<RoleController>, IDisposable
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            EventDispacher.Instance.AddEventListener("register_req", OnRegister);
            EventDispacher.Instance.AddEventListener("login_req", OnLogin);
        }

        /// <summary>
        /// 接收客户端的注册请求消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnRegister(object msg, Role role)
        {
            ReturnValue<UserEntity> ret = new ReturnValue<UserEntity>(); 
            var handle = msg as mmopb.register_req;
            //查询有无相同账号的账户

            if (UserCacheModel.Instance.GetEntity(string.Format("UserName='{0}'", handle.account)) != null)
            {
                ret.HasError = true;
                ret.Message = "已存在相同的账号";
            }
            //查询有无相同昵称的账户
            else if(UserCacheModel.Instance.GetEntity(string.Format("NickName='{0}'", handle.nickname)) != null)
            {
                ret.HasError = true;
                ret.Message = "已存在相同的昵称";
            }
            //没有就进行创建
            else
            {
                Console.WriteLine("开始注册");
                UserEntity entity = new UserEntity
                {
                    Status = EnumEntityStatus.Released,
                    UserName = handle.account,
                    Pwd = handle.password,
                    NickName = handle.nickname,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    GoldCoin = 0,
                    Diamond = 0
                };
                UserCacheModel.Instance.Create(entity);
                ret.Value = entity;
                ret.HasError = false;
                Console.WriteLine("注册成功");
            }
            RegisterReturn(role, ret);
        }


        /// <summary>
        /// 服务器响应注册消息
        /// </summary>
        /// <param name="role"></param>
        private void RegisterReturn(Role role,ReturnValue<UserEntity> ret)
        {
            mmopb.register_ack register_Ack = new mmopb.register_ack();
            if (!ret.HasError)
            {
               
                register_Ack.succ = true;
               
                Console.WriteLine("返回注册成功消息");
            }
            else{
                register_Ack.succ = false;
                register_Ack.error = ret.Message;
                Console.WriteLine("返回注册失败消息");
            }
            role.Client_SActor.Send(ProtoHelper.EncodeWithName(register_Ack));
        }

        /// <summary>
        /// 接收客户端的登陆请求
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="role"></param>
        private void OnLogin(object msg, Role role)
        {
            mmopb.login_ack login_Ack = new mmopb.login_ack();
            var handle = msg as mmopb.login_req;
            string condition = string.Format("UserName='{0}'And Pwd='{1}'", handle.account, handle.password);
            UserEntity entity=UserCacheModel.Instance.GetEntity(condition);
            if (entity != null)
            {           
                login_Ack.succ = true;
               
                role.NickName = entity.NickName;
                role.RoleId = Convert.ToInt32(entity.Id);
                RoleMgr.Instance.AllRoleDic.Add(Convert.ToInt32(entity.Id), true);
                Console.WriteLine("登陆成功");
            }
            else
            {
                login_Ack.succ = false;
                Console.WriteLine("登陆失败");
            }
        }

        /// <summary>
        /// 服务器响应登陆消息
        /// </summary>
        /// <param name="role"></param>
        private void LoginReturn(Role role)
        {

        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            EventDispacher.Instance.RemoveEventListener("register_req", OnRegister);
            EventDispacher.Instance.RemoveEventListener("login_req", OnLogin);
        }
    }
}
