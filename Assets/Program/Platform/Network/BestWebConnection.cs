using System;
using UnityEngine;
using BestHTTP.WebSocket;
using GameCore;


namespace Platform
{
    public class BestWebConnection : TMonoSingleton<BestWebConnection>
    {
        WebSocket websocket;

        float mTickTime;
        bool mStartReconnect = false;
        bool connected = false;
        
        void Start()
        {
            Reconnect();
        }


        void Update()
        {
            if (mStartReconnect)
            {
                mTickTime += Time.deltaTime;
                if (mTickTime >= 5)
                {
                    mTickTime = 0;
                    mStartReconnect = false;
                    Reconnect();
                }
            }
        }

        void Reconnect()
        {
            websocket = new WebSocket(new System.Uri(GameConfig.WebSocketUrl));
            websocket.OnOpen = OnOpen;
            websocket.OnMessage = OnMessageRecieve;
            websocket.OnBinary = OnBinaryRecieve;
            websocket.OnClosed = OnClose;
            websocket.OnError = OnError;
            websocket.Open();
        }

        void OnOpen(WebSocket socket)
        {
            GameDebug.Log("Connection open!");
            connected = true;
            mStartReconnect = false;
            mTickTime = 0;
        }

        void OnMessageRecieve(WebSocket socket, string message)
        {
            GameDebug.Log("Received OnMessage!" + message);
            GameLogicManager.Instance.ProcessWebsocketMessage(message);
        }

        void OnBinaryRecieve(WebSocket socket, byte[] data)
        {
        }

        void OnClose(WebSocket socket, UInt16 code, string message)
        {
            GameDebug.Log("Connection closed!" + message);
            socket.Close();
            socket = null;
            connected = false;
            mTickTime = 0;
            mStartReconnect = true;
        }

        void OnError(WebSocket socket, string reason)
        {
            GameDebug.Log(reason);
            socket.Close();
            socket = null;
            connected = false;
            mTickTime = 0;
            mStartReconnect = true;
        }

        public bool CanSend
        {
            get { return connected; }
        }

        void OnApplicationQuit()
        {
            if (websocket != null)
            {
                websocket.Close();
                mStartReconnect = false;
            }
        }

        #region 消息发送

        public void SendMsg(string msg)
        {
            GameDebug.Log(websocket.State + msg);
            websocket.Send(msg);
        }

        #endregion
    }
}