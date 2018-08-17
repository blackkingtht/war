using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using mmopb;

public class GameResult : MonoBehaviour
{
    //存储游戏数据
    private Dictionary<Int32, mmopb.p_gameResultInfo> resultList;

    //本地用户游戏结果
    private bool isWinGame = false;


    private static GameResult instance;
    public static GameResult Instance
    {
        get
        {
            if (instance == null)
            {

                GameObject obj = new GameObject("GameResult");
                instance = obj.AddComponent<GameResult>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public Dictionary<int, p_gameResultInfo> ResultList
    {
        get
        {
            return resultList;
        }

        set
        {
            resultList = value;
        }
    }

    public bool IsWinGame
    {
        get
        {
            return isWinGame;
        }

        set
        {
            isWinGame = value;
        }
    }
}
