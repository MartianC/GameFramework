using System;
using System.Collections.Generic;
using GameCore;
using Platform;

namespace HotLogic
{
    public class ModuleConfig
    {
        #region 模块名字 命名规范：功能简写加下划线  例：MODE_GAME_LOGIN ==> GL

        #region UI

        public const string MODULE_GAME_LOGIN = "GL"; //登陆

        #endregion

        #endregion

        #region 注册模块名对应的Type

        private static Dictionary<string, Type> _modulesType = new Dictionary<string, Type>
        {
            #region UI

            { MODULE_GAME_LOGIN, typeof(LoginModule) }, //登陆

            #endregion
        };

        #endregion
        
        public static Dictionary<string, Type> ModulesType { get => _modulesType; }

        public static Type GetModule(string moduleName)
        {
            if (ModulesType.ContainsKey(moduleName))
            {
                return ModulesType[moduleName];
            }
            else
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("模块没有注册 请将模块注册到ModuleConfig中！ 模块名： " + moduleName);
                }
            }
            return null;
        }
    }
}