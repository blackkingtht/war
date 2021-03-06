﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Soldier
{
    /// <summary>
    /// 挂载在两个终点的脚本
    /// </summary>
    public class DestinationTrigger : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            SoldierCtrl soldierCtrl = other.gameObject.GetComponent<SoldierCtrl>();
            DataMgr.Instance.SoldierDic.Remove(soldierCtrl.id);
            Destroy(other.gameObject);
            if (soldierCtrl.playerId == LocalUser.Instance.PlayerId)
            {
                string key = soldierCtrl.SoldierName;
                var message = new mmopb.soldierBreak_req();
                message.keyName = key;
                ClientNet.Instance.Send(ProtoBuf.ProtoHelper.EncodeWithName(message));
            }  
        }
    }
}