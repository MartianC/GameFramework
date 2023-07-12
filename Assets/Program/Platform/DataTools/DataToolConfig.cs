using UnityEngine;

namespace Platform
{
    public static class DataToolConfig
    {
        public static readonly string MONITOR_PATH = "Assets/HotUpdateResources/TextAsset/";
        public static readonly string SCRIPT_PATH = Application.dataPath + "/Program/HotLogic/GameData/DatasAll/";
        public static readonly string SCRIPT_TEMPLATE_PATH = Application.dataPath + "/Program/Platform/DataTools/GameData.cs.txt";
        public static readonly string GAME_DATATYPE_PATH = Application.dataPath + "/Program/HotLogic/GameData/GameDataType.cs";
        
        public const string REPLACE_DATANAME = "#DATANAME#";
        public const string REPLACE_MEMBERS = "#MEMBERS#";
        public const string REPLACE_MEMBERSINIT = "#MEMBERSINIT#";

        public static readonly string[] SUPPORTED_TYPE = new string[]
        {
            "string",
            "int",
            "float",
        };
    }
}