using System.IO;
using GameCore;
using Platform;

public class DLLMgr
{
    public static string DllPath = "Assets/HotUpdateResources/Dll/Hidden~/HotUpdateScripts.dll";

    public static void MakeBytes()
    {
        var bytes = FileToByte(DllPath);
        var result = ByteToFile(bytes, "Assets/HotUpdateResources/Dll/HotUpdateScripts.dll.bytes");
        if (!result)
        {
            if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
            {
                GameDebug.LogError("DLLתByte[]解析失败");
            }
        }
    }

    /// <summary>
    /// ɾ���ļ���Ŀ¼
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
        catch
        {
            result = false;
        }

        return result;
    }
}
