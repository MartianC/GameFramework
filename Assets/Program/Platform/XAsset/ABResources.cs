using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libx;
using System;
using GameCore;
using Platform;

public class ABResources
{
    /// <summary>
    /// 加载完毕的资源
    /// </summary>
    static Dictionary<string, AssetRequest> assetsDict = new Dictionary<string, AssetRequest>();
    /// <summary>
    /// 同步加载热更资源
    /// Load hot update resource
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    //public static T LoadRes<T>(string path, MatchMode mode = MatchMode.AutoMatch) where T : UnityEngine.Object
    //{
    //    string name = ResPath(path, mode);
    //    var res = Assets.LoadAsset(name, typeof(T));
    //    AddAssetRequest(name, res);
    //    return res.asset as T;
    //}
    /// <summary>
    /// 同步加载资源(异步写法)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="callBack"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static void LoadRes<T>(string path, Action<T, AssetRequest> callBack, MatchMode mode = MatchMode.AutoMatch) where T : UnityEngine.Object
    {
        string name = ResPath(path, mode);
        var res = Assets.LoadAsset(name, typeof(T));
        AddAssetRequest(name, res); 
        callBack?.Invoke(res.asset as T, res);
        //return res;
    }
    /// <summary>
    /// 异步并行加载热更资源（可加回调）
    /// Load hot update resource async but parallel (can add callback)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <param name="mode"></param>
    public static void LoadResAsync<T>(string path, Action<T, AssetRequest> callback, MatchMode mode = MatchMode.AutoMatch) where T : UnityEngine.Object
    {
        var res = Assets.LoadAssetAsync(ResPath(path, mode), typeof(T));
        res.completed += delegate (AssetRequest resource)
        {
            if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
            {
                GameDebug.Log(resource.url + "     " + resource.loadState + "        " + resource.refCount);
            }
            AddAssetRequest(resource.url, resource);
            callback?.Invoke(resource.asset as T, res);
        };
        //return res;
    }

    /// <summary>
    /// 场景加载的进度
    /// Progress of loading a scene
    /// </summary>
    public static float LoadSceneProgress;

    /// <summary>
    /// 异步并行加载场景（可加回调）
    /// Load hot update scene async but parallel (can add callback)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <param name="additive"></param>
    public static async void LoadSceneAsync(string path, Action callback = null, bool additive = false)
    {
        var req = Assets.LoadSceneAsync(path, additive);
        req.completed += delegate
        {
            callback?.Invoke();
            LoadSceneProgress = 1;
        };
        while (!req.isDone)
        {
            LoadSceneProgress = req.progress;
            await System.Threading.Tasks.Task.Delay(1);
        }
    }
    static void AddAssetRequest(string name, AssetRequest req)
    {
        if (!assetsDict.TryGetValue(name, out var value))
        {
            assetsDict[name] = req;
        }
    }
    public static void UnloadRequest(string name, MatchMode mode)
    {
        if (assetsDict.TryGetValue(ResPath(name, mode), out var value))
        {
            Assets.UnloadAsset(value);
        }
    }
    public static string ResPath(string path, MatchMode mode)
    {
        if (path.Contains("Assets/HotUpdateResources/"))
        {
            path = path.Replace("Assets/HotUpdateResources/", "");
            path = path.Substring(path.IndexOf("/") + 1);
        }
        switch (mode)
        {
            case MatchMode.AutoMatch:
                return path;
            case MatchMode.Animation:
                return "Assets/HotUpdateResources/Controller/" + path;
            case MatchMode.Material:
                return "Assets/HotUpdateResources/Material/" + path;
            case MatchMode.Prefab:
                return "Assets/HotUpdateResources/Prefab/" + path;
            case MatchMode.Scene:
                return "Assets/HotUpdateResources/Scene/" + path;
            case MatchMode.ScriptableObject:
                return "Assets/HotUpdateResources/ScriptableObject/" + path;
            case MatchMode.TextAsset:
                return "Assets/HotUpdateResources/TextAsset/" + path;
            case MatchMode.UI:
                return "Assets/HotUpdateResources/UI/" + path;
            case MatchMode.Other:
                return "Assets/HotUpdateResources/Other/" + path;
            case MatchMode.Dll:
                return "Assets/HotUpdateResources/Dll/" + path;
            case MatchMode.UIAtlas:
                return "Assets/HotUpdateResources/UIAtlas/" + path;
            case MatchMode.Audio:
                return "Assets/HotUpdateResources/Audio/" + path;
            default:
                return path;
        }
    }
    public bool FileExists(string fileName, MatchMode mode)
    {
        var str = ResPath(fileName, mode);
        return false;
    }
    public enum MatchMode
    {
        AutoMatch = 1,
        Animation = 2,
        Material = 3,
        Prefab = 4,
        Scene = 5,
        ScriptableObject = 6,
        TextAsset = 7,
        UI = 8,
        Other = 9,
        Dll = 10,
        UIAtlas = 11,
        Audio = 12,
    }
}