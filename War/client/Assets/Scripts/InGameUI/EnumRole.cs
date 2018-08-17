using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoldierEnum
{
    None,              //未定义
    Orc_Assassin = 1,    //兽族刺客
    Orc_Magic = 2,       //兽族法师
    Orc_Scout = 3,       //兽族投石手
    Orc_Swordsman = 4,   //兽族剑士
    Orc_Infantry = 5,    //兽族步兵
    Terren_Worker = 6,   //人族工人
    Terren_Spear = 7,    //人族长枪兵
    Terren_Cavalry = 8,  //人族骑兵
    Terren_Archer = 9,   //人族弓箭手
    Terren_Infantry = 10 //人族步兵
}

public enum SkillEnum
{
    None=0,               //未选择技能
    SpeedUp=1,            //冲锋
    Stop=2                //停止
}