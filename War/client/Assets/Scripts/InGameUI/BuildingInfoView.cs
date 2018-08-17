﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoView : UIViewBase
{
    //建筑名字
    public Text buildName;
    //建筑等级
    public GameObject lv1;
    public GameObject lv2;
    public GameObject lv3;
    //升级消耗的能量
    public Text cost;
    //攻击力
    public Text attackPower;
    //护甲
    public Text armor;
    //生命值
    public Text hp;
    //奖励
    public Text reward;
    //登录按钮
    public Button upgradeBtn;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Upgrade":
                UIDispacher.Instance.DispachEvent("Upgrade", this.gameObject);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}