using Platform;

namespace HotLogic
{
    public class GameHotUpdateLogicManager : TSingleton<GameHotUpdateLogicManager>
    {
        public ModuleManager moduleManager = new ModuleManager();
        
        /// <summary>
        /// 开始游戏逻辑 
        /// </summary>
        public void InitGameLogic()
        {
            RegistModule(ModuleConfig.MODULE_GAME_LOGIN, null);
        }

        public bool ProcessWebsocketMessage(string message)
        {
            var bVo = SerializeManager.DeSerialize<BaseVo>(message);
            var bData = SerializeManager.DeSerialize<BaseResponse>(bVo.content);

            return MessageDispatchManager.Instance.ProcessWebsocketMessage(bVo.msgId, bData);
        }
        
        /// <summary>
        /// 移除所有UI功能模块
        /// </summary>
        public void RemoveAllModule()
        {
            moduleManager.RemoveAllModule();
        }
        /// <summary>
        /// 移除UI功能模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveModule(string moduleName)
        {
            //移除模块需要统一执行 OnHide
            //若直接销毁物体需要调用 OnDestroy
            var module = moduleManager.GetBaseModule(moduleName);
            if (module != null)
            {
                module.OnHide();
                module.OnDestroy();
            }
            moduleManager.RemoveModule(moduleName);
        }
        public BaseModule GetBaseModule(string moduleName)
        {
            var module = moduleManager.GetBaseModule(moduleName);
            return module;
        }
        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RegistModule(string moduleName, object obj)
        {
            moduleManager.RegistModule(moduleName, obj);
        }

    }
}