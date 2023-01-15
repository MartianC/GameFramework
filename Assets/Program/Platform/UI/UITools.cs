using UnityEngine;
using GameCore;

namespace Platform
{
    public static class UITools
    {
        /// <summary>
        /// 向一个UI结点添加子结点
        /// </summary>
        /// <param name="rParent"></param>
        /// <param name="rPrefabGo"></param>
        /// <param name="rLayerName"></param>
        /// <returns></returns>
        public static GameObject AddChild(this Transform rParent, GameObject rPrefabGo, ELayer rLayerName = ELayer.UI)
        {
            if (rParent == null || rPrefabGo == null) return null;

            GameObject rTargetGo = rPrefabGo; //GameObject.Instantiate(rPrefabGo);  //需要已实例化的Object
            if (rTargetGo.GetComponent<RectTransform>() == null)
            {
                var rect = rTargetGo.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
            rTargetGo.name = rPrefabGo.name;
            rTargetGo.transform.SetParent(rParent, false);
            rTargetGo.SetLayer(rLayerName);

            return rTargetGo;
        }
        /// <summary>
        /// 向一个UI结点添加子结点 并获取指定的Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rParent"></param>
        /// <param name="rPrefabGo"></param>
        /// <param name="rLayerName"></param>
        /// <returns></returns>
        public static T AddChild<T>(this Transform rParent, GameObject rPrefabGo, ELayer rLayerName = ELayer.UI)
        {
            GameObject preGo = AddChild(rParent, rPrefabGo, rLayerName);
            if (preGo == null)
            {
                return default(T);
            }
            return preGo.GetComponent<T>();
        }
        /// <summary>
        /// 递归设置一个节点的层
        /// </summary>
        /// <param name="rGo"></param>
        /// <param name="rLayerName"></param>
        private static void SetLayer(this GameObject rGo, ELayer rLayerName)
        {
            if (rGo == null) return;

            SetLayer(rGo.transform, rLayerName);
        }
        /// <summary>
        /// 设置一个Transform下的所有的GameObject的层
        /// </summary>
        /// <param name="rParent"></param>
        /// <param name="rLayerName"></param>
        private static void SetLayer(Transform rParent, ELayer rLayerName)
        {
            if (rParent == null) return;

            rParent.gameObject.layer = LayerMask.NameToLayer(rLayerName.ToString());

            for (int i = 0; i < rParent.transform.childCount; i++)
            {
                var rTrans = rParent.transform.GetChild(i);
                SetLayer(rTrans, rLayerName);
            }
        }
    }
}
