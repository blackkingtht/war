using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 房间列表视图
/// </summary>
public class RoomsView : UIViewBase
{
    //房间界面对象
    public GameObject rooms;
    //房间item挂载的父对象
    public Transform parentTransForm;

    //新的邀请信息
    public GameObject newInvite;
    public Text newInviteMsg;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Return":
                UIDispacher.Instance.DispachEvent("Return", go);
                break;
            case "Refresh":
                UIDispacher.Instance.DispachEvent("Refresh", go);
                break;
            case "CreateRoom":
                UIDispacher.Instance.DispachEvent("CreateRoom", go);
                break;
            case "Yes_invite":
                UIDispacher.Instance.DispachEvent("ClickYesNewInvite", this.gameObject);
                break;
            case "No_invite":
                UIDispacher.Instance.DispachEvent("ClickNoNewInvite", this.gameObject);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
