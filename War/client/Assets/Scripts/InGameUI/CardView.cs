﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : UIViewBase,IPointerEnterHandler,IPointerExitHandler
{
    //卡片角色的名字
    public Text role_name;
    //卡片角色的图像
    public Image role_img;
    //使用所需能量
    public Text role_energy;
    //卡片类型
    public SoldierEnum soldierType = SoldierEnum.Terren_Cavalry;
    //角色详细信息
    public GameObject role_info;
    //角色详细信息文本
    public Text role_info_text;
    //悬停计时
    private float _timer;
    //进入计时
    private bool _isEnter;
    //停留时间
    private const float stay_time = 1.0f;

    public void Update()
    {
        _timer += Time.deltaTime;
        if(_isEnter && _timer > stay_time)
        {
            UIDispacher.Instance.DispachEvent("StayOnCard", this.gameObject);
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Card":
                UIDispacher.Instance.DispachEvent("ClickCard", this.gameObject);
                break;
        }
    }
    
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _timer = 0;
        _isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isEnter = false;
        UIDispacher.Instance.DispachEvent("ExitCard", this.gameObject);
    }
}