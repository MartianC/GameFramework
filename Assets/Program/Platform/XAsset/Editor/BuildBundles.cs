using System.Diagnostics;
using libx;
using Platform;

namespace JEngine.Editor
{
    public class BuildBundles
    {
        [UnityEditor.MenuItem("Tools/AssetBundle/XAsset/Bundles/Build Bundles %#&B")]
        private static void BuildAssetBundles()
        {
            DLLMgr.Delete("Assets/HotUpdateResources/Dll/HotUpdateScripts.bytes");
            // DLLMgr.Delete(Directory.GetParent(Application.dataPath)+"/Assets/XAsset/ScriptableObjects/Rules.asset");
            // DLLMgr.Delete(Directory.GetParent(Application.dataPath)+"/Assets/XAsset/ScriptableObjects/Manifest.asset");
            DLLMgr.Delete("/Assets/Program/Platform/XAsset/ScriptableObjects/Rules.asset");
            DLLMgr.Delete("/Assets/Program/Platform/XAsset/ScriptableObjects/Manifest.asset");

            var watch = new Stopwatch();
            watch.Start();
            var bytes = DLLMgr.FileToByte(DLLMgr.DllPath);
            var result = DLLMgr.ByteToFile(bytes, "Assets/HotUpdateResources/Dll/HotUpdateScripts.bytes");
            watch.Stop();
            GameDebug.Log("Convert Dlls in: " + watch.ElapsedMilliseconds + " ms.");
            if (!result)
            {
                GameDebug.LogError("DLL转Byte[]出错！");
            }

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