using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 创建的房间视图
/// </summary>
public class RoomView : UIViewBase
{
    //创建的房间界面对象
    public GameObject room_boot;
    //房间里面的信息对象（除按钮）
    public GameObject room;
    //聊天框对象
    public GameObject chat;
    //输入消息
    public InputField message;
    ////消息挂载的父对象
    public Transform chatTransForm;
    //点击邀请好友之后显示的好友列表对象
    public GameObject friendList;
    //好友item挂载的父对象
    public Transform friendsTransForm;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "StartGame":
                UIDispacher.Instance.DispachEvent("StartGame", go);
                break;
            case "QuitRoom":
                UIDispacher.Instance.DispachEvent("QuitRoom", go);
                break;
            case "InviteFriend":
                UIDispacher.Instance.DispachEvent("InviteFriend", go);
                break;
            case "SendMessage":
                UIDispacher.Instance.DispachEvent("SendMessage", go);
                break;
            case "CloseInviteFriends":
                UIDispacher.Instance.DispachEvent("CloseInviteFriends", go);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
