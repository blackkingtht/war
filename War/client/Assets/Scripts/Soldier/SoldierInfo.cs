using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Default,                    //默认状态
    Run,                        //行走状态
    Fight,                      //攻击状态
    Dead                        //死亡状态
}
public enum Race
{
    Orc,                        //兽族
    Terren,                     //人族
    Undead,                     //亡灵族
    Elf                         //精灵族
}
public enum AttackSpecies
{
    Normol,                     //普通攻击
    Spell,                      //魔法攻击
    Pierce                      //穿刺攻击
}
public enum ArmorSpecies
{
    Light,                      //轻甲
    Heavy,                      //重甲
    Leather                     //皮甲
}
public enum Camp
{
    Bright = 1,                     //光明阵营
    Dark                        //黑暗阵营
}
public class SoldierInfo{
    public String Name { get; set; }
    public int MAXHP { get;  set; }          //血量上限
    public int AttackPower { get;  set; }    //攻击力    
    public int Armor { get;  set; }          //护甲值
    public float Speed { get;  set; }        //速度
    public float AttackRange { get;  set; }  //攻击范围
    //public int Level { get;  set; }          //兵种等级（根据建筑等级）
    public Race race { get;  set; }          //种族
    public AttackSpecies attackSpecies { get;  set; }    //攻击类型
    public ArmorSpecies armorSpecies { get;  set; }      //护甲类型
    public int Cost { get; set; }            //花费
    public int Reward { get; set; }          //突破的奖励
}
