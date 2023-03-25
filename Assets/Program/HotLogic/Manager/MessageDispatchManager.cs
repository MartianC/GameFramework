using System.Collections.Generic;
using GameCore;
using Platform;

namespace HotLogic
{
    public class MessageDispatchManager : TSingleton<MessageDispatchManager>
    {
        /// <summary>
        /// 消息注册的Dic
        /// </summary>
        private Dictionary<int, BaseDataProvider> _messageRegistDic = new Dictionary<int, BaseDataProvider>();

        /// <summary>
        /// 数据供给器的注册Dic
        /// </summary>
        private Dictionary<string, BaseDataProvider> _dataProviderDic = new Dictionary<string, BaseDataProvider>();

        /// <summary>
        /// 统一注册
        /// </summary>
        public void Init()
        {
            RegistProvider(new CommonDataProvider("Provider_CommonData"));
        }

        /// <summary>
        /// 注册数据供给器
        /// </summary>
        /// <param name="provider"></param>
        public void RegistProvider(BaseDataProvider provider)
        {
            if (_dataProviderDic.ContainsKey(provider.ProviderName))
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("重复注册模块的数据供给器 name = " + provider.ProviderName);
                }
            }
            else
            {
                _dataProviderDic.Add(provider.ProviderName, provider);
            }
        }

        /// <summary>
        /// 移除数据供给器
        /// </summary>
        /// <param name="providerName"></param>
        public void RemoveProvider(string providerName)
        {
            if (_dataProviderDic.ContainsKey(providerName))
            {
                _dataProviderDic.Remove(providerName);
            }
            else
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("移除模块的数据供给器不存在 name = " + providerName);
                }
            }
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="provider"></param>
        public void RegistMsg(short msgId, BaseDataProvider provider)
        {
            if (_messageRegistDic.ContainsKey(msgId))
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("模块中重复注册消息 msgId = " + msgId + " 供给器name = " + provider.ProviderName);
                }
            }
            else
            {
                _messageRegistDic.Add(msgId, provider);
            }
        }

        /// <summary>
        /// 移除消息注册
        /// </summary>
        /// <param name="msgId"></param>
        public void RemoveMsg(short msgId)
        {
            if (_messageRegistDic.ContainsKey(msgId))
            {
                _messageRegistDic.Remove(msgId);
            }
            else
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("移除的消息不存在 msgId = " + msgId);
                }
            }
        }

        /// <summary>
        /// 消息分发
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ProcessWebsocketMessage(short msgId, BaseResponse message)
        {
            if (_messageRegistDic.ContainsKey(msgId))
            {
                return _messageRegistDic[msgId].ProcessWebsocketMessage(msgId, message);
            }
            else
            {
                return false;
            }
        }

        public bool ProcessTCPmessage(TcpMessage message)
        {
            if (_messageRegistDic.ContainsKey(message.MsgId))
            {
                return _messageRegistDic[message.MsgId].ProcessTcpMessage(message);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据供给器的名字获取该供给器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public T GetProvider<T>(string providerName) where T : BaseDataProvider
        {
            if (_dataProviderDic.ContainsKey(providerName))
            {
                return (T)_dataProviderDic[providerName];
            }
            else
            {
                return null;
            }
        }
    }
}