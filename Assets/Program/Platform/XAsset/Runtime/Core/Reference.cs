using System.Collections.Generic;
using UnityEngine;

namespace libx
{
    public class Reference
    {
        //延迟释放时间
        const float MinLifeTime = 5f;
        //激活时间
        float _realyTime = 0;
        private List<Object> _requires;

        public bool IsUnused()
        {
            if (_requires != null)
            {
                for (var i = 0; i < _requires.Count; i++)
                {
                    var item = _requires[i];
                    if (item != null)
                        continue;
                    Release();
                    _requires.RemoveAt(i);
                    i--;
                }
                if (_requires.Count == 0)
                    _requires = null;
            }
            return refCount <= 0 && Time.time > _realyTime + MinLifeTime;
        }

        public int refCount;

        public void Retain()
        {
            refCount++;
            _realyTime = Time.time;
        }

        public void Release()
        {
            refCount--;
        }

        private bool checkRequires
        {
            get { return _requires != null; }
        }

        public void Require(Object obj)
        {
            if (_requires == null)
                _requires = new List<Object>();

            _requires.Add(obj);
            //Retain();
        }

        public void Dequire(Object obj)
        {
            if (_requires == null)
                return;

            if (_requires.Remove(obj))
                Release();
        }
    }
}
