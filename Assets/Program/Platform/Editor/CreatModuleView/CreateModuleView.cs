using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Platform
{
    public class CreateModuleView : UnityEditor.Editor
    {
        
        private static ViewInfo _currViewInfo;


        [MenuItem("Assets/Create/Create ModuleView", false, 20)]
        public static void CreateView()
        {
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                if (!(obj is GameObject) || !IsMatchPrefabName(obj.name))
                {
                    continue;
                }
                var path = AssetDatabase.GetAssetPath(obj);
                if (!path.StartsWith(CreateModuleViewConfig.ProcessingDirectory))
                {
                    continue;
                }
                path = path.Replace(obj.name + ".prefab", String.Empty);
                var content = CreateContent((obj as GameObject).transform);
                CreateScript(path, content);
            }
            AssetDatabase.Refresh();
        }


        private static string CreateContent(Transform prefab)
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
            
            result = result.Replace("#SCRIPTNAME#", _currViewInfo.Name + "ModuleView");
            result = result.Replace("#KEY#", GetKey());
            result = result.Replace("#COMPONENT#", GetComponents());
            result = result.Replace("#KEYSET#", GetKetSet());
            result = result.Replace("#FINDCOMPONENT#", GetFindComponent());

            return result;
        }


        private static void CreateScript(string prefabPath, string content)
        {
            var filePath = Application.dataPath 
                           + CreateModuleViewConfig.ModuleScriptDirectory 
                           + prefabPath.Replace(CreateModuleViewConfig.ProcessingDirectory, String.Empty)
                           + $"{_currViewInfo.Name}/";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = _currViewInfo.Name + "ModuleView.cs";
            filePath += fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Create(filePath).Dispose();
            File.WriteAllText(filePath, content);
        }

        #region GetReplace

        
        
        private static string GetKey()
        {
            var sb = new StringBuilder();
            foreach (var component in _currViewInfo.Components)
            {
                sb.Append($"        private const string {component} = \"{component}\";\n");
            }
            return sb.ToString();
        }
        
        private static string GetComponents()
        {
            var sb = new StringBuilder();
            foreach (var component in _currViewInfo.Components)
            {
                sb.Append($"        public {GetType(component)} {component.Replace(CreateModuleViewConfig.ComponentStart, CreateModuleViewConfig.ComponentStartReplace)} {{ get; private set; }}\n");
            }
            return sb.ToString();
        }
        
        private static string GetKetSet()
        {
            var sb = new StringBuilder();
            foreach (var component in _currViewInfo.Components)
            {
                sb.Append($"                {component},\n");
            }
            return sb.ToString();
        }

        private static string GetFindComponent()
        {
            var sb = new StringBuilder();
            foreach (var component in _currViewInfo.Components)
            {
                sb.Append($"            {component.Replace(CreateModuleViewConfig.ComponentStart, CreateModuleViewConfig.ComponentStartReplace)} = GetObject<{GetType(component)}>({component});\n");
            }
            return sb.ToString();
        }


        #endregion

        #region Tools

        private static string GetTempScriptContent()
        {
            return File.ReadAllText(CreateModuleViewConfig.TemplatePath);
        }

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
        
        private static string GetType(string componentName)
        {
            return CreateModuleViewConfig.ComponentDic[GetSuffix(componentName)];
        }
        
        private static string GetSuffix(string componentName)
        {
            var strings = componentName.Split('_');
            return strings[strings.Length - 1];
        }


        #endregion
    }
}