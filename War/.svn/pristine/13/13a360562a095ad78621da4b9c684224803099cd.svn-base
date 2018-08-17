using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillView : UIViewBase, IPointerEnterHandler, IPointerExitHandler
{
    //技能的枚举类型
    public SkillEnum skillEnum;
    //技能的图像
    public Image skill_img;
    //技能介绍对象
    public GameObject skill_info;
    //技能名字
    public Text skill_name;
    //技能介绍对象文本
    public Text skill_info_text;
    //冷却倒计时
    public Text skill_coldTime;
    //悬停计时
    private float _timer;
    //进入计时
    private bool _isEnter;
    //停留时间
    private const float stay_time = 1.0f;


    public void Update()
    {
        _timer += Time.deltaTime;
        if (_isEnter && _timer > stay_time)
        {
            UIDispacher.Instance.DispachEvent("StayOnSkill", this.gameObject);
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Skill":
                UIDispacher.Instance.DispachEvent("ClickSkill", this.gameObject);
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
        UIDispacher.Instance.DispachEvent("ExitSkill", this.gameObject);
    }
}
