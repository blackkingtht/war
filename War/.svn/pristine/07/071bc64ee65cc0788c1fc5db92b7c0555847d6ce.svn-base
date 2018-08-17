using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示消息工具类
/// </summary>
public class TipUtils : MonoBehaviour 
{
    private static TipUtils instance;
    public static TipUtils Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("NET").AddComponent<TipUtils>();
            }
            return instance;
        }
    }

    private GameObject m_toast;
    
    /// <summary>
    /// 显示toast
    /// </summary>
    /// <param name="str">提示信息</param>
    /// <param name="trans">挂载的父类</param>
    public void ShowToastUI(string str, Transform trans,float time)
    {
        //加载预制体
        GameObject toast = Resources.Load("Tip") as GameObject;
        // 对象初始化
        m_toast = Instantiate(toast, null, true);
        m_toast.transform.SetParent(trans);
        m_toast.transform.localScale = Vector3.one;
        m_toast.transform.localPosition = new Vector3(0,0,0);
        Tip tips = m_toast.GetComponent<Tip>();
     
        tips.ShowText(str, time);
    }
}
