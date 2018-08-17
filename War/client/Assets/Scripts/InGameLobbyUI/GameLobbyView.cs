using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyView : UIViewBase
{
    public GameObject profile;
    public GameObject assert;
    public GameObject setting;
    public GameObject game;
    public GameObject friends;
    public GameObject chat;
    public GameObject shop;
    public GameObject bag;

    //游戏大厅部分对象，好友，按钮。商店，游戏，背包
    public GameObject partHome;

    //设置详细信息对象
    public GameObject settingDetail;

    //新消息通知
    public GameObject newMessage;

    //新好友通知
    public GameObject newFriend;
    public Text newFriendMsg;

    //新的邀请信息
    public GameObject newInvite;
    public Text newInviteMsg;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "GoldCoin":
                UIDispacher.Instance.DispachEvent("ClickGoldCoin", this.gameObject);
                break;
            case "Diamond":
                UIDispacher.Instance.DispachEvent("ClickDiamond", this.gameObject);
                break;
            case "Setting":
                UIDispacher.Instance.DispachEvent("ClickSetting", this.gameObject);
                break;
            case "1V1":
                UIDispacher.Instance.DispachEvent("Click1V1", this.gameObject);
                break;
            case "Friends":
                UIDispacher.Instance.DispachEvent("ClickFriends", this.gameObject);
                break;
            case "SendMessage":
                UIDispacher.Instance.DispachEvent("ClickSendMessage", this.gameObject);
                break;
            case "ShoppingStore":
                UIDispacher.Instance.DispachEvent("ClickShoppingStore", this.gameObject);
                break;
            case "Bag":
                UIDispacher.Instance.DispachEvent("ClickBag", this.gameObject);
                break;
            case "Yes":
                UIDispacher.Instance.DispachEvent("ClickYesNewFriend", this.gameObject);
                break;
            case "No":
                UIDispacher.Instance.DispachEvent("ClickNoNewFriend", this.gameObject);
                break;
            case "Yes_invite":
                UIDispacher.Instance.DispachEvent("ClickYesNewInvite", this.gameObject);
                break;
            case "No_invite":
                UIDispacher.Instance.DispachEvent("ClickNoNewInvite", this.gameObject);
                break;
            case "MusicButton":
                UIDispacher.Instance.DispachEvent("ClickMusicButton", this.gameObject);
                break;
            case "LogoutAccount":
                UIDispacher.Instance.DispachEvent("ClickLogoutAccount", this.gameObject);
                break; 
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }

}
