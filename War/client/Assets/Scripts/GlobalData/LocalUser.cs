﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存当前用户数据
/// </summary>
public class LocalUser : MonoBehaviour {

    //账户id
    private int playerId;
    //用户账户
    private string user_account;
    //用户昵称
    private string user_nickname;
    //金币
    private int user_coin;
    //钻石
    private int user_diamond;
    //玩家阵营
    private int camp;
    //选择卡牌
    private SoldierEnum cardStatus = SoldierEnum.None;
    //当前点击的建筑
    public int SelectBuildId;
    //能量点数
    private int energy = 20;
    //血量
    private int myHp = 20;
    private int enemyHp = 20;
    //比赛结果
    private bool isWin;
    //技能CD（所有技能共享）
    private bool isCoolDown = true;
    //当前光明方选择的技能
    private SkillEnum brightSkillStatus;
    //当前黑暗方选择的技能
    private SkillEnum darkSkillStatus;
    //音乐关闭
    private bool isMute = false;
    //音量大小
    private float voice = 1;

    private static LocalUser instance;
    public static LocalUser Instance
    {
        get
        {
            if (instance == null)
            {

                GameObject obj = new GameObject("LocalUser");
                instance = obj.AddComponent<LocalUser>();
                instance.SelectBuildId = -1;
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public void Cost(int money)
    {
        Energy -= money;
        PlayerInfoCtrl.Instance.ShowEnergy();
    }

    /// <summary>
    /// 账户方法
    /// </summary>
    public string User_account
    {
        get
        {
            return user_account;
        }

        set
        {
            user_account = value;
        }
    }

    /// <summary>
    /// 昵称方法
    /// </summary>
    public string User_nickname
    {
        get
        {
            return user_nickname;
        }

        set
        {
            user_nickname = value;
        }
    }

    /// <summary>
    /// 阵营方法
    /// </summary>
    public int Camp
    {
        get
        {
            return camp;
        }

        set
        {
            camp = value;
        }
    }

    public int PlayerId
    {
        get
        {
            return playerId;
        }

        set
        {
            playerId = value;
        }
    }

    public SoldierEnum CardStatus
    {
        get
        {
            return cardStatus;
        }

        set
        {
            cardStatus = value;
        }
    }

    public int User_coin
    {
        get
        {
            return user_coin;
        }

        set
        {
            user_coin = value;
        }
    }

    public int User_diamond
    {
        get
        {
            return user_diamond;
        }

        set
        {
            user_diamond = value;
        }
    }

    public int Energy
    {
        get
        {
            return energy;
        }

        set
        {
            energy = value;
        }
    }

    public int MyHp
    {
        get
        {
            return myHp;
        }

        set
        {
            myHp = value;
        }
    }

    public int EnemyHp
    {
        get
        {
            return enemyHp;
        }

        set
        {
            enemyHp = value;
        }
    }

    public bool IsWin
    {
        get
        {
            return isWin;
        }

        set
        {
            isWin = value;
        }
    }

    public SkillEnum DarkSkillStatus
    {
        get
        {
            return darkSkillStatus;
        }

        set
        {
            darkSkillStatus = value;
        }
    }

    public bool IsCoolDown
    {
        get
        {
            return isCoolDown;
        }

        set
        {
            isCoolDown = value;
        }
    }

    public SkillEnum BrightSkillStatus
    {
        get
        {
            return brightSkillStatus;
        }

        set
        {
            brightSkillStatus = value;
        }
    }

    public bool IsMute
    {
        get
        {
            return isMute;
        }

        set
        {
            isMute = value;
        }
    }

    public float Voice
    {
        get
        {
            return voice;
        }

        set
        {
            voice = value;
        }
    }
}