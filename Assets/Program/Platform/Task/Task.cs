using System;

namespace Platform
{
    public class Task
    {
        public int Id;
        public Action<object> callBack;
        public object param;
        public int repeat;
        public float duration;
        public float delay;
        public float passedTime;
        public bool isRemoved;
    }
}