using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetNoticeCtrl : MonoBehaviour {

    public NetNoticeView netNoticeView;
	// Use this for initialization
	void Start () {
        UIDispacher.Instance.AddEventListener("ClickConfirmNet", ClickConfirmNet);
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("ClickConfirmNet", ClickConfirmNet);
    }

    /// <summary>
    /// 点击确定按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickConfirmNet(object param)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LogAndReg");
    }
}
