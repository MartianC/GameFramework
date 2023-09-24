using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Platform
{
    [InitializeOnLoad]
    public class CreateModuleView : Editor
    {
        
        private static ViewInfo _currViewInfo;

        static CreateModuleView()
        {
            //PrefabStage.prefabSaved += CreatViewAuto;
            PrefabUtility.prefabInstanceUpdated += CreatViewAuto;
        }

        [MenuItem("Assets/Create/Create ModuleView", false, 20)]
        public static void CreateViewMenu()
        {
            var ids = Selection.assetGUIDs;
            foreach (var id in ids)
            {
                CreatView(AssetDatabase.GUIDToAssetPath(id));
            }
            AssetDatabase.Refresh();
        }
        
        public static void CreatViewAuto(GameObject go)
        {
            //CreatView(PrefabStageUtility.GetCurrentPrefabStage().assetPath);
            var assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(go));
            CreatView(assetPath);
            AssetDatabase.Refresh();
        }

        private static void CreatView(string assetPath)
        {
            if (!IsMatchPrefabPath(assetPath))
            {
                return;
            }
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (go is null || !IsMatchPrefabName(go.name))
            {
                return;
            }
            var path = assetPath.Replace(go.name + ".prefab", String.Empty);
            var content = CreateContent(go.transform);
            CreateScript(path, content);
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

        private static bool IsMatchPrefabPath(string assetPath)
        {
            return assetPath.StartsWith(CreateModuleViewConfig.ProcessingDirectory);
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