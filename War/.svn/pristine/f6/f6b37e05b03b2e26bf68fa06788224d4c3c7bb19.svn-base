using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 注册登录控制器
/// </summary>
public class LogAndRegController : MonoBehaviour 
{
    /// <summary>
    /// 信息状态常亮
    /// </summary>
    private const int isAccountNull = 0;
    private const int isAccountWrong = 1;
    private const int isNicknameNull = 2;
    private const int isPasswordNull = 3;
    private const int isPwdConfirmWrong = 4;
    private const int isInfoRight = 5;

    //登录视图
    public LoginView loginView;
    //注册视图
    public RegisterView registerView;
    //UI根界面
    public Canvas root;

    //按钮声音特效
    public AudioSource button;


    public void Start()
    {
        //初始设置显示登录界面，隐藏注册界面
        loginView.login.SetActive(true);
        registerView.register.SetActive(false);

        //事件
        UIDispacher.Instance.AddEventListener("Login", OnClickBtnLogin);
        UIDispacher.Instance.AddEventListener("CreateAccount", OnClickBtnCreateAccount);
        UIDispacher.Instance.AddEventListener("Register", OnClickBtnRegister);
        UIDispacher.Instance.AddEventListener("ReturnLogin", OnClickBtnReturnLogin);

        NetDispacher.Instance.AddEventListener("login_ack", LoginHandle);
        NetDispacher.Instance.AddEventListener("register_ack", RegisterHandle);

        //音量设置
        GameObject canvas = GameObject.Find("Canvas");
        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        audioSource.volume = LocalUser.Instance.Voice;
        audioSource.mute = LocalUser.Instance.IsMute;
    }

    public void Update()
    {
        //音量设置
        GameObject canvas = GameObject.Find("Canvas");
        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        audioSource.volume = LocalUser.Instance.Voice;
        audioSource.mute = LocalUser.Instance.IsMute;
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("Login", OnClickBtnLogin);
        UIDispacher.Instance.RemoveEventListener("CreateAccount", OnClickBtnCreateAccount);
        UIDispacher.Instance.RemoveEventListener("Register", OnClickBtnRegister);
        UIDispacher.Instance.RemoveEventListener("ReturnLogin", OnClickBtnReturnLogin);
        NetDispacher.Instance.RemoveEventListener("login_ack", LoginHandle);
        NetDispacher.Instance.RemoveEventListener("register_ack", RegisterHandle);
    }

    /// <summary>
    /// 在登录界面点击登录按钮，进行登录
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnLogin(object param)
    {
        button.Play();
        var account = loginView.Account_log.text;
        var password = loginView.Password_log.text;
        //账户格式验证
        var isAccountRight = AccountFormatVerification(account);
        if (isAccountRight)
        {
            var loginMsg = new mmopb.login_req();
            loginMsg.account = account;
            loginMsg.password = password;
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(loginMsg));
        }
        else
        {
            TipUtils.Instance.ShowToastUI("用户名不正确", root.transform, 2.0f);
        }
    }

    /// <summary>
    /// 登录界面点击创建账户转到注册界面
    /// </summary>
    /// <param name="param"></param>
    public void OnClickBtnCreateAccount(object param)
    {
        loginView.login.SetActive(false);
        registerView.register.SetActive(true);
    }

    /// <summary>
    /// 在注册界面点击注册按钮，进行注册
    /// </summary>
    /// <param name="param"></param>
    public void OnClickBtnRegister(object param)
    {
        button.Play();
        //获取输入，发送给服务器，接受结果，成功则跳转到登陆界面，否则显示错误消息
        string account = registerView.Account_reg.text;
        string nickName = registerView.Nickname_reg.text;
        string password = registerView.Password_reg.text;
        string pwdConfirm = registerView.PasswordConfirm_reg.text;

        var registerMsg = new mmopb.register_req();

        //注册信息验证
        var verification = InfoVerification(account, nickName, password, pwdConfirm);
        switch (verification)
        {
            case isAccountNull:
                TipUtils.Instance.ShowToastUI("用户名不能为空", root.transform, 2.0f);
                break;
            case isAccountWrong:
                TipUtils.Instance.ShowToastUI("用户名不正确", root.transform, 2.0f);
                break;
            case isNicknameNull:
                TipUtils.Instance.ShowToastUI("昵称不能为空", root.transform, 2.0f);
                break;
            case isPasswordNull:
                TipUtils.Instance.ShowToastUI("密码不能为空", root.transform, 2.0f);
                break;
            case isPwdConfirmWrong:
                TipUtils.Instance.ShowToastUI("两次密码不匹配", root.transform, 2.0f);
                break;
            case isInfoRight:
                registerMsg.account = account;
                registerMsg.password = password;
                registerMsg.nickname = nickName;
                ClientNet.Instance.Send(ProtoHelper.EncodeWithName(registerMsg));
                break;
        }
    }
    
    /// <summary>
    /// 在注册界面点击返回按钮，返回到登录界面
    /// </summary>
    /// <param name="param"></param>
    public void OnClickBtnReturnLogin(object param)
    {
        loginView.login.SetActive(true);
        registerView.register.SetActive(false);
    }

    /// <summary>
    /// 登录时处理接收的服务器消息，成功进入游戏界面
    /// </summary>
    /// <param name="msg"></param>
    private void LoginHandle(object msg)
    {
        var ack = msg as mmopb.login_ack;
        var succ = ack.succ;
        if (succ)
        {
            LocalUser.Instance.User_account = loginView.Account_log.text;
            LocalUser.Instance.PlayerId = ack.roleId;
            LocalUser.Instance.User_nickname = ack.nickName;
            LocalUser.Instance.User_coin = ack.goldCoin;
            LocalUser.Instance.User_diamond = ack.diamond;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
        }
        else
        {
            TipUtils.Instance.ShowToastUI(ack.error, root.transform, 2.0f);
        }
    }

    /// <summary>
    /// 注册时处理接收到的服务器消息，注册成功跳转到登录界面，否则提示错误消息
    /// </summary>
    /// <param name="msg"></param>
    private void RegisterHandle(object msg)
    {
        var ack = msg as mmopb.register_ack;
        var succ = ack.succ;
        if (succ)
        {
            loginView.login.SetActive(true);
            registerView.register.SetActive(false);
            TipUtils.Instance.ShowToastUI("注册成功", root.transform, 2.0f);
        }
        else
        {
            TipUtils.Instance.ShowToastUI(ack.error, root.transform, 2.0f);
            Debug.Log(ack.error);
        }
    }

    /// <summary>
    /// 账户格式验证，只能为字母和数字
    /// </summary>
    /// <param name="account">用户账号</param>
    /// <returns></returns>
    private bool AccountFormatVerification(string account)
    {
        var _account = account;
        Regex reg = new Regex(@"^\w+$");

        //要加上一个是否为空的判断
        if (_account != "")
        {
            if (reg.IsMatch(_account))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 注册信息验证
    /// </summary>
    /// <param name="account">账户</param>
    /// <param name="nickname">昵称</param>
    /// <param name="password">密码</param>
    /// <param name="passwordConfirm">确认密码</param>
    /// <returns></returns>
    private int InfoVerification(string account, string nickname, string password, string passwordConfirm)
    {
        Regex reg = new Regex(@"^\w+$");

        //账号判断
        if (account == "")
        {
            return isAccountNull;
        }
        else
        {
            if (!reg.IsMatch(account))
            {
                return isAccountWrong;
            }
        }

        //昵称判断
        if (nickname == "")
        {
            return isNicknameNull;
        }

        //密码判断
        if (password == "" || passwordConfirm == "")
        {
            return isPasswordNull;
        }
        else
        {
            if (!passwordConfirm.Equals(password))
            {
                return isPwdConfirmWrong;
            }
        }

        return isInfoRight;
    }
}
