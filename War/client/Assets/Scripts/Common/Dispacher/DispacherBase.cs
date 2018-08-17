﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispacherBase<T, P, X>
    where T : new()
    where P : class
{
    #region 单例
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
    #endregion
    #region 委托原型和委托字典
    /// <summary>
    /// 委托原型
    /// </summary>
    /// <param name="param"></param>
    public delegate void HandleDelegate(P param);

    /// <summary>
    /// 委托字典
    /// </summary>
    private Dictionary<X, HandleDelegate> HandleDelegateDic = new Dictionary<X, HandleDelegate>();
    #endregion

    /// <summary>
    /// 注册事件监听
    /// </summary>
    /// <param name="key"></param>
    /// <param name="handle"></param>
    public void AddEventListener(X key,HandleDelegate handle)
    {
        if (HandleDelegateDic.ContainsKey(key))
        {
            HandleDelegateDic[key] += handle;
        }
        else
        {
            HandleDelegateDic.Add(key, handle);
        }
    }

    /// <summary>
    /// 取消事件监听
    /// </summary>
    /// <param name="key"></param>
    /// <param name="handle"></param>
    public void RemoveEventListener(X key,HandleDelegate handle)
    {
        if (HandleDelegateDic.ContainsKey(key))
        {
            HandleDelegateDic[key] -= handle;
        }
        else
        {
            Debug.Log("事件未注册");
        }
    }

    public void DispachEvent(X key,P param)
    {
        if (HandleDelegateDic.ContainsKey(key)&& HandleDelegateDic[key]!=null)
        {
            HandleDelegateDic[key](param);
        }
        else
        {
            Debug.Log("不存在相应的事件");
        }
    }
}