using System;
using System.IO;
using GameCore;
using Platform;
using UnityEngine;

public class DLLMgr
{
    public static string DllPath = "/HotUpdateResources/Dll/Hidden~/HotUpdateScripts.dll";
    public static string PdbPath = "/HotUpdateResources/Dll/Hidden~/HotUpdateScripts.pdb";

    public static void MakeBytes()
    {
        var bytes = FileToByte(Application.dataPath + DllPath);
        var result = ByteToFile(bytes, Application.dataPath + "/HotUpdateResources/Dll/HotUpdateScripts.dll.bytes");
        if (!result)
        {
            if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
            {
                GameDebug.LogError("DLLתByte[]解析失败");
            }
        }
        
        bytes = FileToByte(Application.dataPath + PdbPath);
        result = ByteToFile(bytes, Application.dataPath + "/HotUpdateResources/Dll/HotUpdateScripts.pdb.bytes");
        if (!result)
        {
            if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
            {
                GameDebug.LogError("PDBתByte[]解析失败");
            }
        }
    }

    /// <summary>
    /// 删除文件或目录
    /// </summary>
    /// <param name="path"></param>
    public static void Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        if (Directory.Exists(path))
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Delete(true);
        }
    }

    /// <summary>
    /// 将文件转换成byte[]数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]数组</returns>
    public static byte[] FileToByte(string fileUrl)
    {
        try
        {
            using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 将byte[]数组保存成文件
    /// </summary>
    /// <param name="byteArray">byte[]数组</param>
    /// <param name="fileName">保存至硬盘的文件路径</param>
    /// <returns></returns>
    public static bool ByteToFile(byte[] byteArray, string fileName)
    {
        bool result = false;
        try
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
                result = true;
            }
        }
        catch(Exception e)
        {
            GameDebug.LogError(e);
            result = false;
        }

        return result;
    }
}
