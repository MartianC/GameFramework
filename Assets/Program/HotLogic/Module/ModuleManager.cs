using System;
using System.Collections.Generic;

namespace HotLogic
{
    public class ModuleManager
    {
        private Dictionary<string, BaseModule> _modules = new Dictionary<string, BaseModule>();

                /// <summary>
        /// 检测指定的模块是否存在
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public bool HasModule(string moduleName)
        {

            if (_modules == null)
            {
                return false;
            }
            if (_modules.ContainsKey(moduleName))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据指定的模块名称注册模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RegistModule(string moduleName, object obj)
        {
            Type moduleType = ModuleConfig.GetModule(moduleName);
            if (null != moduleType)
            {
                var module = (BaseModule)Activator.CreateInstance(moduleType);
                module.SetParams(obj);
                _modules.Add(moduleName, module);
            }
        }
        public BaseModule GetBaseModule(string moduleName)
        {
            if (_modules.ContainsKey(moduleName))
            {
                return _modules[moduleName];
            }
            return null;
        }
        /// <summary>
        /// 移除功能模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveModule(string moduleName)
        {
            if (_modules.ContainsKey(moduleName))
            {
                _modules.Remove(moduleName);
            }
            //匹配队列中的模块 进行相应移除
            //QueueMoudelManager.Instance.RemoveModule(moduleName);
        }
        /// <summary>
        /// 移除所有热更功能
        /// </summary>
        public void RemoveAllModule()
        {
            if (_modules != null)
            {
                _modules.Clear();
            }
        }
        /// <summary>
        /// 获取本地功能模块
        /// </summary>
        /// <param name="moduleName">功能名字</param>
        /// <returns></returns>
        public BaseModule GetLocalModule(string moduleName)
        {
            if (_modules.ContainsKey(moduleName))
            {
                return _modules[moduleName];
            }
            return null;
        }
        /// <summary>
        /// 执行模块的初始化方法 可以携带参数
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="pms"></param>
        public void ExcuteInitFunc(string moduleName, params object[] pms)
        {
            #region 本地模块的初始化
            // switch (moduleName)
            // {
            //     case ModuleConfig.MODULE_GAME_LOGIN:
            //         (GetLocalModule(moduleName) as LoginModule).OnShow();
            //         break;
            // }
            #endregion
        }
        public void SwitchLanguage()
        {
            foreach (var item in _modules)
            {
                item.Value.SwitchLanguage();
            }
        }

        public BaseModule GetModule(string moduleName)
        {
            if (_modules.ContainsKey(moduleName))
            {
                return _modules[moduleName];
            }
            return null;
        }
    }
}