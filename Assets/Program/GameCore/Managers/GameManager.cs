using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// 游戏世界管理对象
    /// 引擎启动类，与Unity进行结合
    /// </summary>
    public sealed class GameManager : TSingleton<GameManager>
    {
        /// <summary>
        /// 游戏启动
        /// </summary>
        public void Startup()
        {
            //初始化逻辑管理器
            GameLogicManager.Instance.Init();
        }
    }

}
