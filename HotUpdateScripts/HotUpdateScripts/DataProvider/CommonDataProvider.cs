using Platform;

namespace HotUpdate
{
    public class CommonDataProvider : BaseDataProvider
    {
        public CommonDataProvider(string pName) : base(pName)
        {
            MessageDispatchManager.Instance.RegistMsg(MessageCommands.LOGIN_GAME , this);
        }

        public override bool ProcessWebsocketMessage(short msgId , BaseResponse message)
        {
            switch (msgId)
            {
                case MessageCommands.LOGIN_GAME:
                {
                    if (message.Success())
                    {
                        //解析消息
                        //var playerInfo = SerializeManager.DeSerialize<PlayerInfo>(message.Data);
                        //Messenger<PlayerInfo>.Broadcast(MessengerEventDef.Event_LoginGame, playerInfo);
                        Messenger.Broadcast(MessengerEventDef.Event_LoginGame);
                    }
                    else
                    {
                        GameDebug.Log(message.Msg);
                    }
                    return true;
                }
            }

            return base.ProcessWebsocketMessage(msgId , message);
        }
    }
}