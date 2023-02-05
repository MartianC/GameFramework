using System.Collections.Generic;
using GameCore;
using Platform;
using UnityEngine;

namespace HotLogic
{
    public abstract class BaseView
    {
        public Transform TransRoot;
        protected Dictionary<string, GameObject> _ObjectDict = null;
        /// <summary>
        /// 根据名字获取UI对象
        /// </summary>
        /// <param name="mScript"></param>
        public virtual void GetObjects(UIAbstractViewObject mScript)
        {
            TransRoot = mScript.transform;
        }
        /// <summary>
        /// 获取指定物体上的指定类型组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetObject<T>(string name)
        {
            if (_ObjectDict == null)
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("gameobject的对应字典没有初始化 请实现视图的GetObjects()方法！");
                }
                return default(T);
            }
            GameObject go;
            if (_ObjectDict.TryGetValue(name, out go))
            {
                T tmp = go.GetComponent<T>();
                if (tmp == null)
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogErrorFormat("found GameObject[{0}], but not find Component[{1}]", name, typeof(T));
                    }
                }
                return tmp;
            }
            else
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogErrorFormat("not find GameObject[{0}]", name);
                }
                return default(T);
            }
        }
        public void Release()
        {
            _ObjectDict.Clear();
        }
    }
}