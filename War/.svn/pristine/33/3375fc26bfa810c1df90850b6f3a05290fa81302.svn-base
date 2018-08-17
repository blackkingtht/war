using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录视图
/// </summary>
public class LoginView: UIViewBase {

    //账号输入框
    public InputField Account_log;
    //密码输入框
    public InputField Password_log;
    //登录界面对象
    public GameObject login;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Login":
                UIDispacher.Instance.DispachEvent("Login", go);
                break;
            case "CreateAccount":
                UIDispacher.Instance.DispachEvent("CreateAccount", go);
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
        Account_log.text = null;
        Password_log.text = null;
    }
}
