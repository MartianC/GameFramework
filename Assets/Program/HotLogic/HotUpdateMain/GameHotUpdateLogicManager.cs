using Platform;

namespace HotLogic
{
    public class GameHotUpdateLogicManager : TSingleton<GameHotUpdateLogicManager>
    {
        /// <summary>
        /// 开始游戏逻辑 
        /// </summary>
        public void InitGameLogic()
        {
            GameDebug.Log("GameHotUpdateLogicManager: InitGameLogic");
        }

        public bool ProcessWebsocketMessage(string message)
        {
            var bVo = SerializeManager.DeSerialize<BaseVo>(message);
            var bData = SerializeManager.DeSerialize<BaseResponse>(bVo.content);

            return MessageDispatchManager.Instance.ProcessWebsocketMessage(bVo.msgId, bData);
        }
    }
}