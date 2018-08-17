using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCtrl : MonoBehaviour {

    public ExitGameView exitgameView;
    void Start()
    {
        exitgameView.exit.SetActive(false);

        //事件
        UIDispacher.Instance.AddEventListener("ClickBtnExit", ClickBtnExit);
        UIDispacher.Instance.AddEventListener("ClickBtnCancel", ClickBtnCancel);

        NetDispacher.Instance.AddEventListener("leaveRoom_ack", ExitGameHandle);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitgameView.exit.SetActive(true);
        }
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("ClickBtnExit", ClickBtnExit);
        UIDispacher.Instance.RemoveEventListener("ClickBtnCancel", ClickBtnCancel);

        NetDispacher.Instance.RemoveEventListener("leaveRoom_ack", ExitGameHandle);
    }

    /// <summary>
    /// 点击退出游戏按钮，向服务器发送离开房间消息
    /// </summary>
    /// <param name="param"></param>
    private void ClickBtnExit(object param)
    {
        var quitMsg = new mmopb.leaveRoom_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(quitMsg));
    }

    /// <summary>
    /// 点击取消按钮关闭退出界面
    /// </summary>
    /// <param name="param"></param>
    private void ClickBtnCancel(object param)
    {
        exitgameView.exit.SetActive(false);
    }

    /// <summary>
    /// 处理服务器返回的消息，成功退出游戏返回游戏大厅
    /// </summary>
    /// <param name="msg">退出房间ack</param>
    private void ExitGameHandle(object msg)
    {
        var quitMsg = msg as mmopb.leaveRoom_ack;
        if (quitMsg.isSuc)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
        }
    }

}
