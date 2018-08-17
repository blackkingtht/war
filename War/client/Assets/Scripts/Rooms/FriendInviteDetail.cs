using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendInviteDetail : UIViewBase
{
    public Text friendName;
    public int friendId;
    //可邀请好友对象
    public GameObject friend_invite;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Invite":
                UIDispacher.Instance.DispachEvent("ClickInviteTargetFriend", friend_invite);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
