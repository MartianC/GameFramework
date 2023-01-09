using UnityEngine;

/// <summary>
/// 普通单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class TSingleton<T> where T : new()
{
    private static object _lock = new object();
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
/// <summary>
/// 继承自Mono的单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class TMonoSingleton<T> : MonoBehaviour
{
    public static T Instance;
        
    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)(object)this;
        }
        else
        {
            Destroy(this);
        }
    }
}
