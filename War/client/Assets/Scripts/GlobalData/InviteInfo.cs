using System;
using System.Collections;
using System.Collections.Generic;
using mmopb;
using UnityEngine;

public class InviteInfo : MonoBehaviour {

    private Dictionary<Int32, mmopb.p_roleInfoInRoom> playerList;

    //邀请接受状态
    private bool isAgreeInvite = false;


    private static InviteInfo instance;
    public static InviteInfo Instance
    {
        get
        {
            if (instance == null)
            {

                GameObject obj = new GameObject("InviteInfo");
                instance = obj.AddComponent<InviteInfo>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }


    public bool IsAgreeInvite
    {
        get
        {
            return isAgreeInvite;
        }

        set
        {
            isAgreeInvite = value;
        }
    }

    public Dictionary<int, p_roleInfoInRoom> PlayerList
    {
        get
        {
            return playerList;
        }

        set
        {
            playerList = value;
        }
    }
}
