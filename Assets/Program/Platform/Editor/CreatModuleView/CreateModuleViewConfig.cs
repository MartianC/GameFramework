using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
    public class CreateModuleViewConfig
    {
        public const string PrefabStart = "MV_";
        public const string PrefabEnd = "_PF";
        public const string ComponentStart = "KC_";
        public const string ComponentStartReplace = "C_";

        
        public const string ProcessingDirectory = "Assets/HotUpdateResources/Prefab";
        public const string ModuleScriptDirectory = "/Program/HotLogic/HotUpdateFunction/Modules";
        public static readonly string TemplatePath = Application.dataPath + "/Program/Platform/Editor/CreatModuleView/CreateModuleView.cs.txt";

        
        public static Dictionary<string, string> ComponentDic = new Dictionary<string, string>()
        {
            {"TF","Transform" },
            {"RTF","RectTransform"},
            {"TXT","Text" },
            {"IPF","InputField" },
            {"IMG","Image" },
            {"BTN","Button" },
            {"TOG","Toggle" },
            {"SLD","Slider"},
        };

    }
}