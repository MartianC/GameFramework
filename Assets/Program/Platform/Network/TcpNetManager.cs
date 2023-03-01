using System;
using System.Net;
using System.Net.Sockets;
using GameCore;

namespace Platform
{
    enum ETcpState
    {
        Disconnect,
        Connecting,
        Connected,
    } 
    
    public class TcpNetManager:TMonoSingleton<TcpNetManager>
    {
        private ETcpState _state;
        private Socket _socket = null;

        public override void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            this._state = ETcpState.Disconnect;
            Connect(GameConfig.TcpServerUrl, GameConfig.TcpServerPort);
        }

        void Connect(string url, int port)
        {
            if (this._state != ETcpState.Disconnect)
            {
                return;
            }
            this._state = ETcpState.Connecting;

            IPAddress ip;
            try
            {
                IPAddress[] addressList = Dns.GetHostAddresses(url);
                ip = addressList[0];
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                AddressFamily family = AddressFamily.InterNetwork;
                if (endPoint.AddressFamily.ToString() == ProtocolFamily.InterNetworkV6.ToString())
                {
                    family = AddressFamily.InterNetworkV6;
                }
                _socket = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
                _socket.BeginConnect(endPoint, new AsyncCallback(OnConnected), _socket);
            }
            catch (Exception e)
            {
                GameDebug.Log(e.ToString());
                OnConnectError();
            }
        }

        private void OnConnected(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }
        
        
        private void OnConnectError()
        {
            throw new NotImplementedException();
        }

    }
}