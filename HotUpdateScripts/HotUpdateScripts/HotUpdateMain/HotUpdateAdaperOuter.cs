using Platform;

namespace HotUpdate
{
    public class HotUpdateAdaperOuter
    {
        public GameHotUpdateLogicManager gameHotUpdateLogicManager;

        /// <summary>
        /// 开启游戏逻辑
        /// </summary>
        public void InitGameLogic()
        {
            gameHotUpdateLogicManager = GameHotUpdateLogicManager.Instance;
            gameHotUpdateLogicManager.InitGameLogic();
        }

        public bool ProcessWebsocketMessage(string message)
        {
            return gameHotUpdateLogicManager.ProcessWebsocketMessage(message);
        }
    }
}