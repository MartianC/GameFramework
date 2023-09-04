using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameCore;
using libx;

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
        private const int HEAD_LENTH = 2;
        //private const int RECV_LENTH = 8192;
        
        private ETcpState _state;
        private Socket _socket = null;

        /// <summary>
        /// 消息接收线程
        /// </summary>
        private Thread _receiveThread = null;

        private byte[] _recvHead = new byte[HEAD_LENTH];
        private byte[] _recvBody;
        
        public Queue<TcpMessage> MessageQueue = new Queue<TcpMessage>();

        void Start()
        {
            this._state = ETcpState.Disconnect;
            this.Connect(GameConfig.TcpServerUrl, GameConfig.TcpServerPort);
        }

        private void Update()
        {
            this.Connect(GameConfig.TcpServerUrl, GameConfig.TcpServerPort);
            this.ProcessMessage();
        }

        private void OnDestroy()
        {
            this.Disconnect();
        }

        private void OnApplicationQuit()
        {
            this.Disconnect();
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
                OnConnectError(e.ToString());
            }
        }

        private void Disconnect()
        {
            this._state = ETcpState.Disconnect;
            try
            {
                if (_socket.Connected)
                {
                    _socket.Disconnect(true);
                    _socket.Close();
                }
            }
            catch (Exception e)
            {
                GameDebug.LogError("disconnect error: " + e.ToString());
            }
            finally
            {
                _socket = null;
            }
        }

        private void OnConnected(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket) ar.AsyncState;
                socket.EndConnect(ar);

                this._state = ETcpState.Connected;
                this._receiveThread = new Thread(new ThreadStart(this.ReceiveThreadWorker));
                this._receiveThread.IsBackground = true;
                this._receiveThread.Start();
                GameDebug.Log("connect success to server: " + GameConfig.TcpServerUrl + ":" + GameConfig.TcpServerPort);
            }
            catch (Exception e)
            {
                this.OnConnectError(e.ToString());
            }
        }
        

        private void OnConnectError(string error)
        {
            GameDebug.Log(error);
            this._state = ETcpState.Disconnect;
        }

        private void ReceiveThreadWorker()
        {
            while (this._state == ETcpState.Connected)
            {
                if (!this._socket.Connected)
                {
                    break;
                }
                try
                {
                    int recvLen = 0;
                    while (recvLen < HEAD_LENTH)
                    {
                        recvLen += _socket.Receive(_recvHead, recvLen, HEAD_LENTH - recvLen, SocketFlags.None);
                    }

                    int bodySize = (_recvHead[0] << 8 | _recvHead[1]);
                    bodySize -= HEAD_LENTH;
                    byte[] _buffer = GetBodyBuffer(bodySize);
                    
                    recvLen = 0;
                    while (recvLen < bodySize)
                    {
                        recvLen += _socket.Receive(_buffer, recvLen, bodySize - recvLen, SocketFlags.None);
                    }
                    _recvBody = _buffer;
                    
                    MessageQueue.Enqueue(TcpMessage.Create(_recvBody));
                }
                catch (Exception e)
                {
                    this.OnConnectError(e.ToString());
                }
            }
        }

        private void ProcessMessage()
        {
            if (this._state == ETcpState.Connected)
            {
                while (this.MessageQueue.Count > 0)
                {
                    TcpMessage message = this.MessageQueue.Dequeue();
                    GameLogicManager.Instance.ProcessTcpMessage(message);
                }
            }
        }

        private byte[] GetBodyBuffer(int size)
        {
            if (_recvBody.Length < size)
            {
                _recvBody = new byte[size];
            }
            Array.Clear(_recvBody, 0, _recvBody.Length);
            return _recvBody;
        }
    }
}