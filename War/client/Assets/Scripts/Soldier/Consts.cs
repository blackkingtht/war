using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Soldier
{
    public class Consts
    {
        public const float BuildCorrectY = 0.4f;        //建筑物Y轴校准

        public const int MaxLevel = 3;                  //最高等级

        public const float ReduceDamageCoefficient = 0.06f;     //减伤系数

        public const float RotateSpeed = 100f;          //士兵角速度

        public const int DetectionRange = 8;            //探测范围

        public const int CorpseFrameNum = 150;          //士兵死亡后尸体保留帧数

        public const float InfluenceSpeedDefault = 0;        //不施放技能时速度被影响的系数

        public const float InfluencceSpeedUp = -1.5f;        //冲锋时速度被影响的系数

        public const float InfluencceSlowDown = 1.5f;        //减速时速度被影响的系数

        public const int BulletSpeed = 6;                   //远程子弹速度


    }
}
