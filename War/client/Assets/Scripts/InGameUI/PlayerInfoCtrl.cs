﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoCtrl : MonoBehaviour {

    public PlayInfoView playInfoView;

    public static PlayerInfoCtrl Instance;
 
    // Use this for initialization
    private void Awake()
    {
        Instance = this; 
    }

    void Start () {
        
        playInfoView.myHp.text = LocalUser.Instance.MyHp.ToString();
        playInfoView.enemyHp.text = LocalUser.Instance.EnemyHp.ToString();
        playInfoView.energy.text = LocalUser.Instance.Energy.ToString();
        NetDispacher.Instance.AddEventListener("gameOver_bcst", OnGameOver_bcst);
    }
    
    private void OnDestroy()
    {
        NetDispacher.Instance.RemoveEventListener("gameOver_bcst", OnGameOver_bcst);
    }

    /// <summary>
    /// 显示玩家信息
    /// </summary>
     public void ShowInfo()
     {
        playInfoView.energy.text = LocalUser.Instance.Energy.ToString();
        playInfoView.myHp.text = LocalUser.Instance.MyHp.ToString();
        playInfoView.enemyHp.text = LocalUser.Instance.EnemyHp.ToString();
     }
    public void ShowEnergy()
    {
        playInfoView.energy.text = LocalUser.Instance.Energy.ToString();
    }

    /// <summary>
    /// 接收服务器游戏结束的消息
    /// </summary>
    /// <param name="param"></param>
    private void OnGameOver_bcst(object msg)
    {
        var handle = msg as mmopb.gameOver_bcst;
        if (handle.gameResultInfoList.ContainsKey(LocalUser.Instance.PlayerId))
        {
            GameResult.Instance.IsWinGame = handle.gameResultInfoList[LocalUser.Instance.PlayerId].isWinner;
            GameResult.Instance.ResultList = handle.gameResultInfoList;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameResult");
        }
    }
}