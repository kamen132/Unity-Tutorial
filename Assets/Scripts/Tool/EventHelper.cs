 using System;

 public class EventHelper
    {
        /// <summary>
    /// 单例
    /// </summary>
    public static EventHelper Instance = new EventHelper();

    /// <summary>
    /// 异常事件
    /// </summary>
    public static Action<Exception> OnException;

    /// <summary>
    /// 触发一个异常
    /// </summary>
    /// <param name="ex"></param>
    public static void DoException(Exception ex)
    {
        EventHelper.Instance.Do(OnException, ex);
    }

    /// <summary>
    /// 触发一个异常
    /// </summary>
    /// <param name="message"></param>
    public static void DoException(string message)
    {
        EventHelper.Instance.Do(OnException, new Exception(message));
    }

    public void Do(Action action)
    {
        if (action != null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                DoException(ex);
            }
        }
    }

    public void Do<T1>(Action<T1> action, T1 t1)
    {
        if (action != null)
        {
            try
            {
                action(t1);
            }
            catch (Exception ex)
            {
                DoException(ex);
            }
        }
    }

    public void Do<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2)
    {
        if (action != null)
        {
            try
            {
                action(t1, t2);
            }
            catch (Exception ex)
            {
                DoException(ex);
            }
        }
    }

    public void Do<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
        if (action != null)
        {
            try
            {
                action(t1, t2, t3);
            }
            catch (Exception ex)
            {
                DoException(ex);
            }
        }
    }

    public void Do<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        if (action != null)
        {
            try
            {
                action(t1, t2, t3, t4);
            }
            catch (Exception ex)
            {
                DoException(ex);
            }
        }
    }
    }