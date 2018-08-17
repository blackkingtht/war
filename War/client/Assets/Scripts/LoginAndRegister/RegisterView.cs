using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 注册视图
/// </summary>
public class RegisterView: UIViewBase
{
    //账号输入框
    public InputField Account_reg;
    //昵称输入框
    public InputField Nickname_reg;
    //密码输入框
    public InputField Password_reg;
    //密码确认
    public InputField PasswordConfirm_reg;
    //注册页面对象
    public GameObject register;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Register":
                UIDispacher.Instance.DispachEvent("Register", go);
                break;
            case "ReturnLogin":
                UIDispacher.Instance.DispachEvent("ReturnLogin", go);
                break;
            //case "ForgotPassword":
            //    break;
            //case "Help":
            //    break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        Account_reg.text = null;
        Nickname_reg.text = null;
        Password_reg.text = null;
        PasswordConfirm_reg.text = null;
    }
}