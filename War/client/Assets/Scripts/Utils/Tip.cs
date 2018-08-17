using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 类似Toast的功能
/// </summary>
public class Tip : MonoBehaviour {

    public Text toast; 
    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="str">提示信息</param>
    /// <param name="time">持续时间</param>
    public void ShowText(string str,float time)
    {
        toast.text = str;
        Show(time);
    }

    private void Show(float time)
    {
        this.gameObject.SetActive(true);
        Invoke("CloseTip", time);
    }

    private void CloseTip()
    {
        Destroy(this.gameObject);
    }
}
