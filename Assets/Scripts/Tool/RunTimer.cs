using System;
using UnityEngine;

public class RunTimer :  MonoSingleton<RunTimer>
{
    #region Update

    /// <summary>
    /// 运行时间
    /// 可在子线程调用
    /// </summary>
    public float NowTime { get; private set; }

    /// <summary>
    /// 运行时间(忽略时间缩放)
    /// 可在子线程调用
    /// </summary>
    public float UnscaledTime { get; private set; }

    /// <summary>
    /// 本帧与上帧间隔时间
    /// </summary>
    public float DeltaTime
    {
        get { return Time.deltaTime; }
    }

    /// <summary>
    /// 本帧与上帧间隔时间(忽略时间缩放)
    /// </summary>
    public float UnscaledDeltaTime
    {
        get { return Time.unscaledDeltaTime; }
    }

    #region 事件

    private Action onUpdate;

    /// <summary>
    /// 刷新事件
    /// </summary>
    public event Action OnUpdate
    {
        add { onUpdate += value; }
        remove { onUpdate -= value; }
    }

    #endregion

    public bool IsUpdate = false;

    void Update()
    {
        NowTime = Time.time;
        //if (!IsUpdate)
        EventHelper.Instance.Do(onUpdate);
    }

    #endregion

    #region LateUpdate

    #region 事件

    private Action onLateUpdate;

    public event Action OnLateUpdate
    {
        add { onLateUpdate += value; }
        remove { onLateUpdate -= value; }
    }

    #endregion

    void LateUpdate()
    {
        EventHelper.Instance.Do(onLateUpdate);
    }

    #endregion


    #region FixedUpdate

    public float FixedRunTime
    {
        get { return Time.fixedTime; }
    }

    public float FixedDeltaTime
    {
        get { return Time.fixedDeltaTime; }
    }

    #region 事件

    private Action onFixedUpdate;

    public event Action OnFixedUpdate
    {
        add { onFixedUpdate += value; }
        remove { onFixedUpdate -= value; }
    }

    #endregion

    void FixedUpdate()
    {
        UnscaledTime = Time.unscaledTime;
        EventHelper.Instance.Do(onFixedUpdate);
    }

    #endregion
}