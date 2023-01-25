namespace HotUpdate
{
    public class BaseVo
    {
        public short msgId;

        public string content;
        //public string status;

        public string Serialize()
        {
            return SerializeManager.Serialize(this);
        }
    }

    public class BaseResponse
    {
        /// <summary>
        /// 状态
        /// </summary>
        public byte State;

        /// <summary>
        /// 数据
        /// </summary>
        public string Data;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg;

        /// <summary>
        /// 总共条数
        /// </summary>
        public int allCount;

        public BaseResponse()
        {
            State = 1;
        }

        public bool Success()
        {
            return State == 0;
        }
    }
}