using Assets.Scripts.InGameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCtrl : MonoBehaviour {

    private Dictionary<int, Skill> skills = new Dictionary<int, Skill>();
    private GameObject last_selected_skill = null;
    public AudioClip speedUP;
    public AudioClip speedDown;
    private AudioSource audioSource;

    private bool BrightisUsingSkill;
    private bool DarkisUsingSkill;
    private long BrightStartTime;
    private long DarkStartTime;
    private bool isUsedSkill;

    // Use this for initialization
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        //事件
        UIDispacher.Instance.AddEventListener("ClickSkill", ClickSkill);
        UIDispacher.Instance.AddEventListener("StayOnSkill", StayOnSkill);
        UIDispacher.Instance.AddEventListener("ExitSkill", ExitSkill);

        NetDispacher.Instance.AddEventListener("useSkill_bcst", OnUseSkill_bcst);
        
        BrightisUsingSkill = false;
        DarkisUsingSkill = false;
        BrightStartTime=0;
        DarkStartTime=0;
        isUsedSkill = false;
        LocalUser.Instance.DarkSkillStatus = SkillEnum.None;
        LocalUser.Instance.BrightSkillStatus = SkillEnum.None;
        InitSkills();
        LoadSkills();
    }

    public void Update()
    {
        if (DarkisUsingSkill)
        {
            if (DateTime.Now.Ticks/ 10000000 - DarkStartTime >= 5)
            {
                DarkisUsingSkill = false;
                LocalUser.Instance.DarkSkillStatus = SkillEnum.None;
            }
        }

        if (BrightisUsingSkill)
        {
            if (DateTime.Now.Ticks/ 10000000 - BrightStartTime >= 5)
            {
                BrightisUsingSkill = false;
                LocalUser.Instance.BrightSkillStatus = SkillEnum.None;
            }
        }


        if (isUsedSkill)
        {
            ShowSkillColdTime();
        }
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("ClickSkill", ClickSkill);
        UIDispacher.Instance.RemoveEventListener("StayOnSkill", StayOnSkill);
        UIDispacher.Instance.RemoveEventListener("ExitSkill", ExitSkill);

        NetDispacher.Instance.RemoveEventListener("useSkill_bcst", OnUseSkill_bcst);
    }

    /// <summary>
    /// 技能初始化
    /// </summary>
    private void InitSkills()
    {
        //技能1

        Skill skill1 = new Skill();
        skill1.SkillEnum = SkillEnum.SpeedUp;
        skill1.Name = Constant.skill_1_Name;
        skill1.Img = Resources.Load(Constant.skill_1_ImgPath, typeof(Sprite)) as Sprite;
        skill1.Info = Constant.skill_1_Info;
        skills.Add(1, skill1);

        //技能2
        Skill skill2 = new Skill();
        skill2.SkillEnum = SkillEnum.Stop;
        skill2.Name = Constant.skill_2_Name;
        skill2.Img = Resources.Load(Constant.skill_2_ImgPath, typeof(Sprite)) as Sprite;
        skill2.Info = Constant.skill_2_Info;
        skills.Add(2, skill2);
    }

    /// <summary>
    /// 加载技能
    /// </summary>
    private void LoadSkills()
    {
        GameObject[] _skills = GameObject.FindGameObjectsWithTag("SKILL");
        int i = 1;
        foreach(GameObject _skill in _skills)
        {
            SkillView skillView = _skill.GetComponent<SkillView>();
            Skill skill_temp = null;
            if (skills.TryGetValue(i, out skill_temp) == true)
            {
                skillView.skill_img.sprite = skill_temp.Img;
                skillView.skill_name.text = skill_temp.Name;
                skillView.skill_info_text.text = skill_temp.Info;
                skillView.skillEnum = skill_temp.SkillEnum;
            }
            i++;
        }
    }

    /// <summary>
    /// 点击技能按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickSkill(object param)
    {
        GameObject skillGameObject = (GameObject)param;
        SkillView skill=skillGameObject.GetComponent<SkillView>();
        var message = new mmopb.useSkill_req();

        if (!isUsedSkill)
        {
            if (skill.skillEnum == SkillEnum.SpeedUp)
            {
                audioSource.clip = speedUP;
            }
            if (skill.skillEnum == SkillEnum.Stop)
            {
                audioSource.clip = speedDown;
            }
            audioSource.Play();
            message.skillType = Convert.ToInt32(skill.skillEnum);
            ClientNet.Instance.Send(ProtoBuf.ProtoHelper.EncodeWithName(message));
        }
        
    }

    /// <summary>
    /// 在技能上停留显示技能详细信息
    /// </summary>
    /// <param name="param"></param>
    private void StayOnSkill(object param)
    {
        GameObject skill = (GameObject)param;
        SkillView skillView = skill.GetComponent<SkillView>();
        skillView.skill_info.SetActive(true);
    }

    /// <summary>
    /// 鼠标离开技能
    /// </summary>
    /// <param name="param"></param>
    private void ExitSkill(object param)
    {
        GameObject skill = (GameObject)param;
        SkillView skillView = skill.GetComponent<SkillView>();
        skillView.skill_info.SetActive(false);
    }

    /// <summary>
    /// 处理技能广播
    /// </summary>
    /// <param name="msg"></param>
    private void OnUseSkill_bcst(object msg)
    {
        //Debug.Log("收到技能广播");
        var handle = msg as mmopb.useSkill_bcst;
       // Debug.Log(LocalUser.Instance.PlayerId + "++++++" + handle.roleId);
        //Debug.Log(handle.skiilType);
        //Debug.Log(handle.roleId);
        if (LocalUser.Instance.PlayerId == handle.roleId)
        {
            if (LocalUser.Instance.Camp == 1)
            {
                LocalUser.Instance.BrightSkillStatus = (SkillEnum)handle.skiilType;
                BrightisUsingSkill = true;
                BrightStartTime = DateTime.Now.Ticks / 10000000;
                //Debug.Log("111111111111111");
            }
            else
            {
                LocalUser.Instance.DarkSkillStatus = (SkillEnum)handle.skiilType;
                DarkisUsingSkill = true;
                DarkStartTime = DateTime.Now.Ticks / 10000000;
                //Debug.Log("222222222222222222");
            }
            isUsedSkill = true;
        }
        else
        {
            if (LocalUser.Instance.Camp == 1)
            {
                LocalUser.Instance.DarkSkillStatus = (SkillEnum)handle.skiilType;
                DarkisUsingSkill = true;
                DarkStartTime = DateTime.Now.Ticks / 10000000;
                //Debug.Log("33333333333333");
            }
            else
            {
                LocalUser.Instance.BrightSkillStatus = (SkillEnum)handle.skiilType;
                BrightisUsingSkill = true;
                BrightStartTime = DateTime.Now.Ticks / 10000000;
                //Debug.Log("44444444444444");
            }
        }
    }

    /// <summary>
    /// 显示技能剩余时间
    /// </summary>
    private void ShowSkillColdTime()
    {
        long lastTime = 0;
        if (LocalUser.Instance.Camp == (int)Camp.Bright)
        {
            lastTime = DateTime.Now.Ticks / 10000000 - BrightStartTime;
        }
        else 
        {
            lastTime = DateTime.Now.Ticks / 10000000 - DarkStartTime;
        }
        long remainderTime = Constant.skill_1_Cold - lastTime;
        GameObject[] skills = GameObject.FindGameObjectsWithTag("SKILL");
        foreach(GameObject skill in skills)
        {
            SkillView skillView = skill.GetComponent<SkillView>();
            if (remainderTime >= 0)
            {
                int _time = (int)remainderTime;
                skillView.skill_coldTime.text = _time.ToString();
                StartCoroutine(Wait());
            }
            else
            {
                skillView.skill_coldTime.text = "";
                isUsedSkill = false;
            }
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f); 
    }
}
