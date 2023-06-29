using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
    public class CreateModuleViewConfig
    {
        public const string PrefabStart = "MV_";
        public const string PrefabEnd = "_PF";
        public const string ComponentStart = "NC_";
        public const string ComponentStartReplace = "C";

        
        public const string ProcessingDirectory = "Assets/HotUpdateResources/Prefab";
        public const string ModuleScriptDirectory = "Program/HotLogic/HotUpdateFunction/Modules";
        public static readonly string TemplatePath = Application.dataPath + "/Program/Platform/Editor/CreatModuleView/CreateModuleView.cs.txt";

        
        public static Dictionary<string, string> ComponentDic = new Dictionary<string, string>()
        {
            {"_TXT","Text" },
            {"_IPF","InputField" },
            {"_IMG","Image" },
            {"_BTN","Button" },
            {"_TOG","Toggle" },
            {"_SLD","Slider"},
            {"_TF","Transform" },
            {"_RTF","RectTransform"},
        };

    }
}