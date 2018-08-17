using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Soldier
{
    public class BulletCtrl : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Consts.BulletSpeed);
        }
        private void OnTriggerEnter(Collider other)     //子弹碰到物体消失
        {
            Destroy(this.gameObject);
        }
    }
}