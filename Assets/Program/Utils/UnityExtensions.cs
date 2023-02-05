using UnityEngine;

namespace Utils
{
    public static class UnityExtensions
    {
        static public T GetOrAddComponent<T>(this GameObject child, bool set_enable = false) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }
            var bcomp = result as Behaviour;
            if (set_enable)
            {
                if (bcomp != null) bcomp.enabled = true;
            }
            return result;
        }

    }
}