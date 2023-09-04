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
public class TMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object _lock = new object();
    
    private static T _instance;
    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance is null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (_instance)
                    {
                        return _instance;
                    }
                    var go = new GameObject();
                    _instance = go.AddComponent<T>();
                    go.AddComponent<UnityDontDestroy>();
                    go.name = $"[{typeof(T)}]";
                }

                return _instance;
            }
        }
    }
}
