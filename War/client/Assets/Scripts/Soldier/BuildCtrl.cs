using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Soldier
{
    public  class BuildCtrl: MonoBehaviour
    {
        public int PlayerID { get; private set; }       //建筑属于的玩家
        public int BuildID { get; private set; }        //建筑ID
        //public Race Race { get; private set; }          //建筑属于的种族
        public Camp camp { get; private set; }          //阵营
        public int Level { get;private  set; }
        private Animator ani;                           //动画控制器
        private int frameNum;     
        public  Quaternion StartRotate { get; private set; }               //旋转预制体使其朝向正确的方向
        public string SoldierName { get; private set; }                     //士兵名称
        public object lockObject = new object();
        /// <summary>
        /// 初始化相关信息
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="playerID"></param>
        /// <param name="camp"></param>
        /// <param name="rotate"></param>
        /// <param name="soldierName"></param>
        public void Init(int buildId, int playerID, Camp camp, Quaternion rotate, string soldierName)
        {
            BuildID = buildId;
            PlayerID = playerID;
            this.camp = camp;
            //Race = race;
            frameNum = 0;
            StartRotate = rotate;
            SoldierName = soldierName;
        }
        private void Awake()
        {
            Level = 1;
        }
        void Start()
        {           
            ani = GetComponent<Animator>();
        }
        

        //出兵
        private void StartSoldier()
        {

            string soldierName = SoldierName + "_" + this.Level.ToString();      //当前按建筑生产的士兵的名称，用于获取信息
            Vector3 newSoldierPos = transform.position;
            newSoldierPos.z -= (float)1;
            //创建士兵(导入模型和材质贴图的预制体)
            GameObject newSoldier = (GameObject)Resources.Load("Prefabs/" + SoldierName);
            SoldierCtrl newSoldierCtrl = Instantiate(newSoldier, newSoldierPos, StartRotate).AddComponent<SoldierCtrl>();
            Material material = Resources.Load("Materials/level" + Level) as Material;
            //Debug.Log(newSoldier.name + "****************");
            GameObject child = newSoldierCtrl.gameObject.transform.Find(DataMgr.Instance.ObjChildObjDic[newSoldier.name]).gameObject;
            child.GetComponent<SkinnedMeshRenderer>().material = material;
            lock (lockObject)
            {
                DataMgr.Instance.SoldierDic.Add(DataMgr.SoldierId, newSoldierCtrl);         //将士兵加入士兵字典
                newSoldierCtrl.Init(DataMgr.SoldierId++, BuildID, PlayerID, camp, Level, soldierName);
            }
         
        }     
        //每三十秒同步一次出兵
        public void StartSoldierEvery30S()
        {
            StartSoldier();
        }
        /// <summary>
        /// 升级建筑
        /// </summary>
        public void LevelUp()
        {
            if(Level < Consts.MaxLevel)
            {
                Level++;
                //更改外观
                Material material = Resources.Load("Materials/level" + Level) as Material;
                GameObject child = transform.Find(DataMgr.Instance.ObjChildObjDic[gameObject.name]).gameObject;
                child.GetComponent<SkinnedMeshRenderer>().material = material;
                
                //向服务器发送建筑升级消息
                var handle=new mmopb.sendBuileLevelUp_req();
                handle.level = Level;
                handle.roleId = LocalUser.Instance.PlayerId;
                handle.buildId = BuildID;
                handle.name = SoldierName;
                ClientNet.Instance.Send(ProtoBuf.ProtoHelper.EncodeWithName(handle));
            }
        }

        /// <summary>
        /// 服务器同步建筑等级
        /// </summary>
        public void UpdateLevel(int level)
        {
            if (level <= Consts.MaxLevel)
            {
                Level = level;

            }
                
        }
    }
}
