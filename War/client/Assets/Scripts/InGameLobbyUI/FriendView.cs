using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendView : UIViewBase
{
    //好友界面对象
    public GameObject friends;
    //好友item挂载的父对象
    public Transform parentTransForm;
    //添加的好友昵称
    public Text friendNameAdd;
    //聊天界面
    public GameObject chatPanle;
    //聊天输入内容
    public Text chatMsg;
    //消息item挂载的父对象
    public Transform chatParentTransForm;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Add":
                UIDispacher.Instance.DispachEvent("ClickAddFriend", this.gameObject);
                break;
            case "CloseFriends":
                UIDispacher.Instance.DispachEvent("ClickCloseFriends", this.gameObject);
                break;
            case "SendToTargetFriend":
                UIDispacher.Instance.DispachEvent("ClickSendToTargetFriend", this.gameObject);
                break;
            case "CloseChat":
                UIDispacher.Instance.DispachEvent("ClickCloseChat", this.gameObject);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
