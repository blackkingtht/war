using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendDetail : UIViewBase
{
    public Image avatar;
    public Text friendName;
    public GameObject status_img_online;
    public GameObject status_img_buzy;
    public GameObject status_img_offline;
    public Text status_text;
    public GameObject messageNotice;
    public int friendId;
    //单个好友对象
    public GameObject friend;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "DeleteFriend":
                UIDispacher.Instance.DispachEvent("ClickDeleteFriend", friend);
                break;
            case "SendToFriend":
                UIDispacher.Instance.DispachEvent("ClickSendToFriend", friend);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
