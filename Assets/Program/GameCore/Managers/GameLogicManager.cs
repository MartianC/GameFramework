using System.Collections;
using HotLogic;
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
            if (Assets.HasFile("HotUpdateScripts.dll.bytes", ABResources.MatchMode.Dll))
            {
                _isHotUp = true;
            }
            
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
        
    }
    
}
