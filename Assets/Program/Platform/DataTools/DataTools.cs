using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Platform
{
    public class DataTools : AssetPostprocessor
    {
        private static TextAsset _textAsset;
        private static string _summary;
        private static string _dataName;
        private static string[] _memberSummarys;
        private static string[] _members;
        private static string[] _memberTypes;
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
            {
                if (!assetPath.StartsWith(DataToolConfig.MONITOR_PATH) || !assetPath.EndsWith(".txt"))
                {
                    return;
                }

                _textAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset)) as TextAsset;
                Parse();
                GenerateGameDataScript();
                ReGenerateGameDataType();
            }
        }

        private static void Parse()
        {
            var data = _textAsset.text;
            var lines = data.Split('\r');
            _summary = lines[0].Trim();
            _dataName = lines[1].Trim();
            _memberSummarys = lines[2].Split('\t');
            _members = lines[3].Split('\t');
            _memberTypes = lines[4].Split('\t');
            
            for (int i = 0; i < _members.Length; i++)
            {
                _memberSummarys[i] = _memberSummarys[i].Trim();
                _members[i] = _members[i].Trim();
                _memberTypes[i] = _memberTypes[i].Trim();
            }
        }

        private static void GenerateGameDataScript()
        {
            string scriptTemplate = File.ReadAllText(DataToolConfig.SCRIPT_TEMPLATE_PATH);
            scriptTemplate = scriptTemplate.Replace("#DATANAME#", _dataName);
            StringBuilder memberBuilder = new StringBuilder();
            StringBuilder memberInitBuilder = new StringBuilder();
            for (int i = 0; i < _members.Length; i++)
            {
                if (!CheckMemberType(_memberTypes[i]))
                {
                    GameDebug.LogError("生成数据类{_dataName}Data失败，不支持的数据类型:" + _memberTypes[i]);
                    return;
                }

                AddMemberDefine(memberBuilder, i);
                AddMemberInit(memberInitBuilder, i);
            }
            scriptTemplate = scriptTemplate.Replace("#MEMBERS#", memberBuilder.ToString());
            scriptTemplate = scriptTemplate.Replace("#MEMBERSINIT#", memberInitBuilder.ToString());
            
            try{
                string scriptPath = DataToolConfig.SCRIPT_PATH + _dataName + "Data.cs";
                if (File.Exists(scriptPath))
                {
                    File.Delete(scriptPath);
                }
                //创建并写入scriptTemplate
                File.WriteAllText(scriptPath, scriptTemplate, Encoding.UTF8);
                GameDebug.Log($"生成数据类{_dataName}Data.cs成功");
                //刷新编辑器
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                GameDebug.LogError(e);
            }
        }

        private static void ReGenerateGameDataType()
        {
            string dataTypeTemplate = File.ReadAllText(DataToolConfig.GAME_DATATYPE_PATH);
            string pattern = "//AUTOGEN_BEGIN(.*?)//AUTOGEN_END";
            string[] split = Regex.Split(dataTypeTemplate, pattern, RegexOptions.Singleline);
            if (split.Length < 3)
            {
                GameDebug.LogError("GameDataType.cs模板格式错误");
                return;
            }
            var types = split[1].Split('\r');
            if (types.Any(p => p.Trim() == _dataName + ','))
            {
                //GameDebug.LogError($"GameDataType.cs中已存在{_dataName}类型");
                return;
            }
            split[1] += $"{_dataName},\r";
            StringBuilder sb = new StringBuilder();
            sb.Append(split[0]);
            sb.Append("//AUTOGEN_BEGIN");
            sb.Append(split[1]);
            sb.Append("\t\t//AUTOGEN_END");
            sb.Append(split[2]);
            File.WriteAllText(DataToolConfig.GAME_DATATYPE_PATH, sb.ToString(), Encoding.UTF8);
        }
        
        
        private static bool CheckMemberType(string type)
        {
            return DataToolConfig.SUPPORTED_TYPE.Contains(type);
        }
        
        
        private static void AddMemberDefine(StringBuilder sb, int idx)
        {
            sb.Append($"\t\t/// <summary>\r");
            sb.Append($"\t\t/// {_memberSummarys[idx]}\r");
            sb.Append($"\t\t/// </summary>\r");
            sb.Append($"\t\tpublic {_memberTypes[idx]} {_members[idx]};\r");
        }
        private static void AddMemberInit(StringBuilder sb, int idx)
        {
            switch (_memberTypes[idx])
            {
                case "string":
                    sb.Append($"\t\t\t{_members[idx]} = split[{idx}];\r");
                    break;
                case "int":
                    sb.Append($"\t\t\t{_members[idx]} = int.Parse(split[{idx}]);\r");
                    break;
                case "float":
                    sb.Append($"\t\t\t{_members[idx]} = float.Parse(split[{idx}]);\r");
                    break;
                default:
                    break;
            }
        }
    }
}