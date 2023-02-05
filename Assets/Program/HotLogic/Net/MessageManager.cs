using Platform;

namespace HotLogic
{
    public class MessageManager
    {
        public static void SendMessage_Login()
        {
            LoginReq req = new LoginReq(){account = "test"};
            BaseVo baseVo = new BaseVo(){msgId = MessageCommands.LOGIN_GAME, content = SerializeManager.Serialize(req)};
            BestWebConnection.Instance.SendMsg(baseVo.Serialize());
        }
    }
}