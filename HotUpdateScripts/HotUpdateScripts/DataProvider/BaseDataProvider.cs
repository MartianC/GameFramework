namespace HotUpdate
{
    public class BaseDataProvider
    {
        protected string providerName = null;
        public BaseDataProvider(string pName)
        {
            providerName = pName;
        }
        public string ProviderName
        {
            get => providerName;
        }

        // public virtual bool ProcessTCPmessage(int msgId, Platform.TcpMessage message)
        // {
        //     return false;
        // }
        public virtual bool ProcessWebsocketMessage(short msgId , BaseResponse message)
        {
            return false;
        }
    }
}