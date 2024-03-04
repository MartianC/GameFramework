using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class UIViewObjectBase : MonoBehaviour
    {
        /// <summary>
        /// 委托执行的Update方法名 
        /// </summary>
        public static string FUNCTION_NAME_UPDATE = "Update";
        /// <summary>
        /// 委托执行的FixedUpdate方法名 
        /// </summary>
        public static string FUNCTION_NAME_FIXEDUPDATE = "FixedUpdate";
        /// <summary>
        /// 委托执行的OnDestroy方法名
        /// </summary>
        public static string FUNCTION_NAME_ONDESTROY = "OnDestroy";
        /// <summary>
        /// 委托执行的OnEnable方法名
        /// </summary>
        public static string FUNCTION_NAME_ONENABLE = "OnEnable";
        /// <summary>
        /// 委托执行的OnDisable方法名
        /// </summary>
        public static string FUNCTION_NAME_ONDISABLE = "OnDisable";

        private Dictionary<string, Action> _action;

        /// <summary>
        /// 注册需要使用的通用委托
        /// </summary>
        /// <param name="action">执行的委托方法组 Key =>继承自MonoBehaviour的方法,Value =>委托方法</param>
        public void RegistCommAction(Dictionary<string, Action> action)
        {
            this._action = action;
        }

        /// <summary>
        /// 查找对应名字的物体名称
        /// </summary>
        /// <param name="objNames"></param>

        /// <returns></returns>
        public Dictionary<string, GameObject> FindGameObject(HashSet<string> objNames)
        {
            return Utils.UnityUtilTools.GetGameObjectName(gameObject, objNames);
        }

        /// <summary>
        /// 查找对应名字的物体名称
        /// </summary>
        /// <param name="objDict"></param>
        /// <returns></returns>
        public Dictionary<string, GameObject> FindGameObject(Dictionary<string, GameObject> objDict)
        {
            return Utils.UnityUtilTools.GetGameObjectName(gameObject, objDict);
        }

        /// <summary>
        /// 获取单个GameObject
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject FindSingleObject(string name)
        {
            return Utils.UnityUtilTools.GetSingleObject(gameObject, name);
        }

        /// <summary>
        /// 获取指定类型的Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T FindSingleObject<T>(string name)
        {
            return Utils.UnityUtilTools.GetSingleObject<T>(gameObject, name);
        }

        /// <summary>
        /// 为热更新模块准备的携程调用方法 需要在热更新模块管理器里进行适配器的注册
        /// </summary>
        /// <param name="coroutine"></param>
        public void DoCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        void OnEnable()
        {
            if (_action != null && _action.Count > 0 && _action.ContainsKey(FUNCTION_NAME_ONENABLE))
            {
                if (_action[FUNCTION_NAME_ONENABLE] != null)
                {
                    _action[FUNCTION_NAME_ONENABLE]();
                }
            }
        }

        void Update()
        {
            if (_action != null && _action.Count > 0 && _action.ContainsKey(FUNCTION_NAME_UPDATE))
            {
                if (_action[FUNCTION_NAME_UPDATE] != null)
                {
                    _action[FUNCTION_NAME_UPDATE]();
                }
            }
        }

        void FixedUpdate()
        {
            if (_action != null && _action.Count > 0 && _action.ContainsKey(FUNCTION_NAME_FIXEDUPDATE))
            {
                if (_action[FUNCTION_NAME_FIXEDUPDATE] != null)
                {
                    _action[FUNCTION_NAME_FIXEDUPDATE]();
                }
            }
        }

        void OnDisable()
        {
            if (_action != null && _action.Count > 0 && _action.ContainsKey(FUNCTION_NAME_ONDISABLE))
            {
                if (_action[FUNCTION_NAME_ONDISABLE] != null)
                {
                    _action[FUNCTION_NAME_ONDISABLE]();
                }
            }
        }

        void OnDestroy()
        {
            if (_action != null && _action.Count > 0 && _action.ContainsKey(FUNCTION_NAME_ONDESTROY))
            {
                if (_action[FUNCTION_NAME_ONDESTROY] != null)
                {
                    _action[FUNCTION_NAME_ONDESTROY]();
                }
            }
        }
    }

}