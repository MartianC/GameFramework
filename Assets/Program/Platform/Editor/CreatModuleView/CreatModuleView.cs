using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Platform
{
    public class CreatModuleView : UnityEditor.Editor
    {
        
        private static ViewInfo _currViewInfo;
        
        
        
        
        public static string CreatModuleViewContent(Transform prefab)
        {
            var result = GetTempScriptContent();
            _currViewInfo = new ViewInfo(GetModuleViewName(prefab.name));
            var children = prefab.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                if (!IsMatchComponentName(child.name))
                {
                    continue;
                }
                _currViewInfo.Components.Add(child.name);
            }

            return result;
        }

        private static string GetTempScriptContent()
        {
            throw new System.NotImplementedException();
        }

        #region Replace

        

        #endregion

        #region Tools

        
        private static bool IsMatchPrefabName(string name)
        {
            return name.StartsWith(CreateModuleViewConfig.PrefabStart) && name.EndsWith(CreateModuleViewConfig.PrefabEnd);
        }
        
        private static bool IsMatchComponentName(string name)
        {
            if (!name.StartsWith(CreateModuleViewConfig.ComponentStart))
            {
                return false;
            }
            var strings = name.Split('_');
            if (strings.Length < 3)
            {
                return false;
            }
            if (!CreateModuleViewConfig.ComponentDic.ContainsKey(strings[strings.Length - 1]))
            {
                return false;
            }
            return true;
        }

        private static string GetModuleViewName(string prefabName)
        {
            return prefabName.Replace(CreateModuleViewConfig.PrefabStart, "").Replace(CreateModuleViewConfig.PrefabEnd, "");
        }
        
        public static string GetSuffix(string value)
        {
            var strings = value.Split('_');
            return strings[strings.Length - 1];
        }

        
        #endregion
    }
}