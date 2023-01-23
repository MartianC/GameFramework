using System.Diagnostics;
using libx;
using Platform;
using UnityEngine;

namespace libx
{
    public class BuildBundles
    {
        [UnityEditor.MenuItem("Tools/AssetBundle/XAsset/Bundles/Build Bundles %#&B")]
        private static void BuildAssetBundles()
        {
            DLLMgr.Delete(Application.dataPath + "/HotUpdateResources/Dll/HotUpdateScripts.bytes");
            DLLMgr.Delete(Application.dataPath + "/Program/Platform/XAsset/ScriptableObjects/Rules.asset");
            DLLMgr.Delete(Application.dataPath + "/Program/Platform/XAsset/ScriptableObjects/Manifest.asset");

            var watch = new Stopwatch();
            watch.Start();
            // var bytes = DLLMgr.FileToByte(DLLMgr.DllPath);
            // var result = DLLMgr.ByteToFile(bytes, "Assets/HotUpdateResources/Dll/HotUpdateScripts.bytes");
            DLLMgr.MakeBytes();
            watch.Stop();
            GameDebug.Log("Convert Dlls in: " + watch.ElapsedMilliseconds + " ms.");
            // if (!result)
            // {
            //     GameDebug.LogError("DLL转Byte[]出错！");
            // }

            watch = new Stopwatch();
            watch.Start();
            BuildScript.ApplyBuildRules();
            watch.Stop();
            GameDebug.Log("ApplyBuildRules in: " + watch.ElapsedMilliseconds + " ms.");

            watch = new Stopwatch();
            watch.Start();
            BuildScript.BuildAssetBundles();
            watch.Stop();
            GameDebug.Log("BuildAssetBundles in: " + watch.ElapsedMilliseconds + " ms.");
        }
    }
}