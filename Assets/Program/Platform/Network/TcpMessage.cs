using System;

namespace Platform
{
    public class TcpMessage
    {
        public short MsgId;
        public byte[] Data;
        
        public static TcpMessage Create(byte[] body)
        {
            TcpMessage message = new TcpMessage();
            message.MsgId = BitConverter.ToInt16(body, 0);
            message.Data = new byte[body.Length - 2];
            Array.Copy(body, 2, message.Data, 0, message.Data.Length);
            return message;
        }
    }
}