using GameCore;
using UnityEngine;

namespace Platform
{
public class GameDebug
{
    public static void Log(object message)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.Log(DebugHeader() + message);
    }
    public static void Log(object message, Object content)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.Log(DebugHeader() + message, content);
    }
    public static void LogFormat(string format, params object[] args)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogFormat(DebugHeader() + format, args);
    }
    public static void LogWarning(object message)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogWarning(DebugHeader() + message);
    }
    public static void LogWarning(object message, Object content)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogWarning(DebugHeader() + message, content);
    }
    public static void LogWarningFormat(string format, params object[] args)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogWarningFormat(DebugHeader() + format, args);
    }
    public static void LogError(object message)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogError(DebugHeader() + message);
    }
    public static void LogError(object message, Object content)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogError(DebugHeader() + message, content);
    }
    public static void LogErrorFormat(string format, params object[] args)
    {
        if (GameConfig.DebugLevel == EDebugLevel.Prod) { return; }
        Debug.LogErrorFormat(DebugHeader() + format, args);
    }
    static string DebugHeader()
    {
        return System.DateTime.Now.ToString("<color=#32CD32><b>[yyyy-MM-dd HH:mm:ss]</b></color>"); //加粗显示
        //return System.DateTime.Now.ToString("<color=#32CD32>[yyyy-MM-dd HH:mm:ss] </color>");
        //return "";
    }
}
}