using System.Collections;
using HotLogic;
using libx;
using Platform;
using UnityEngine;

namespace GameCore
{
    public class GameLogicManager: TMonoSingleton<GameLogicManager>
    {
        //标记是否进行热更新
        private bool _isHotUp = false;

        public override void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            // if (Assets.HasFile("HotUpdateScripts.dll.bytes", ABResources.MatchMode.Dll))
            // {
            //     _isHotUp = true;
            // }
            
            if (_isHotUp)
            {
                GameHotUpdateManager.Instance.InitHotUpdate();
                // StartCoroutine(CheckHotUpdate());
            }
            
            //进入游戏逻辑
            InitGameLogic();
        }
        
        
        /// <summary>
        /// 初始化游戏逻辑层
        /// </summary>
        public void InitGameLogic()
        {
            if (_isHotUp)
            {
                GameHotUpdateManager.Instance.HotUpdateAdapter.InitGameLogic();
            }
            else
            {
                GameHotUpdateLogicManager.Instance.InitGameLogic();
            }
        }

        public bool ProcessWebsocketMessage(string message)
        {
            if (_isHotUp)
            {
                return GameHotUpdateManager.Instance.HotUpdateAdapter.ProcessWebsocketMessage(message);
            }
            else
            {
                return GameHotUpdateLogicManager.Instance.ProcessWebsocketMessage(message);
            }
        }
        
        public bool ProcessTcpMessage(TcpMessage message)
        {
            if (_isHotUp)
            {
                return GameHotUpdateManager.Instance.HotUpdateAdapter.ProcessTcpMessage(message);
            }
            else
            {
                return GameHotUpdateLogicManager.Instance.ProcessTcpMessage(message);
            }
        }
        
        
        public void RegistModule(string moduleName,object obj = null)
        {
            if (_isHotUp)
            {
                GameHotUpdateManager.Instance.HotUpdateAdapter.RegistModule(moduleName,obj);
            }
            else
            {
                GameHotUpdateLogicManager.Instance.RegistModule(moduleName,obj);
            }
        }
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public BaseModule GetBaseModule(string moduleName)
        {
            if (_isHotUp)
            {
                return (BaseModule)  GameHotUpdateManager.Instance.HotUpdateAdapter.GetBaseModule(moduleName);
            }
            else
            {
                return   GameHotUpdateLogicManager.Instance.GetBaseModule(moduleName);
            }
      
        }
        /// <summary>
        /// 移除所有UI功能模块
        /// </summary>
        public void RemoveAllModule()
        {
            if (_isHotUp)
            {
                GameHotUpdateManager.Instance.HotUpdateAdapter.RemoveAllModule();
            }
            else
            {
                GameHotUpdateLogicManager.Instance.RemoveAllModule();
            }
        }
        /// <summary>
        /// 移除UI功能模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveModule(string moduleName)
        {
            if (_isHotUp)
            {
                GameHotUpdateManager.Instance.HotUpdateAdapter.RemoveModule(moduleName);
            }
            else
            {
                GameHotUpdateLogicManager.Instance.RemoveModule(moduleName);
            }
        }

        
    }
    
}
