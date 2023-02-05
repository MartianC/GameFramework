using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using UnityEditor;

namespace GameCore
{
    public class HotUpdateAdapterInner
    {
        //热更新的唯一全局实例
        AppDomain mAppdomain;
        IType mType;
        object mInstance;

        public HotUpdateAdapterInner(AppDomain rAppdomain, IType rType, object rInstance)
        {
            mAppdomain = rAppdomain;
            mType = rType;
            mInstance = rInstance;
        }

        /// <summary>
        /// 热更新的处理消息的方法
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        // public bool ProcessTCPmessage(short msgId, TcpMessage message)
        // {
        //     List<IType> paramList = new List<IType>();
        //     paramList.Add(mAppdomain.GetType(typeof(int)));
        //     paramList.Add(mAppdomain.GetType(typeof(TcpMessage)));
        //     IMethod method = mType.GetMethod("ProcessTCPmessage", paramList, null);
        //     object obj = mAppdomain.Invoke(method, mInstance, msgId, message);
        //     var flag = (bool)obj;
        //     return flag;
        // }

        public bool ProcessWebsocketMessage(string message)
        {
            List<IType> paramList = new List<IType>();
            paramList.Add(mAppdomain.GetType(typeof(string)));
            IMethod method = mType.GetMethod("ProcessWebsocketMessage", paramList, null);
            object obj = mAppdomain.Invoke(method, mInstance, message);
            var flag = (bool)obj;
            return flag;
        }

        
        
        /// <summary>
        /// 开启游戏逻辑
        /// </summary>
        public void InitGameLogic()
        {
            IMethod method = mType.GetMethod("InitGameLogic", 0);
            mAppdomain.Invoke(method, mInstance, null);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RegistModule(string moduleName, object obj)
        {
            List<IType> paramList = new List<IType>();
            paramList.Add(mAppdomain.GetType(typeof(string)));
            IMethod method = mType.GetMethod("RegistModule", paramList, null);
            mAppdomain.Invoke(method, mInstance, moduleName);
        }

        /// <summary>
        /// 移除所有UI功能模块
        /// </summary>
        public void RemoveAllModule()
        {
            IMethod method = mType.GetMethod("RemoveAllModule", 0);
            mAppdomain.Invoke(method, mInstance, null);
        }

        /// <summary>
        /// 移除所有UI功能模块
        /// </summary>
        public object GetBaseModule(string moduleName)
        {
            List<IType> paramList = new List<IType>();
            paramList.Add(mAppdomain.GetType(typeof(string)));
            IMethod method = mType.GetMethod("GetBaseModule", paramList, null);
            object mo = mAppdomain.Invoke(method, mInstance, moduleName);
            return mo;
        }

        /// <summary>
        /// 移除UI功能模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveModule(string moduleName)
        {
            List<IType> paramList = new List<IType>();
            paramList.Add(mAppdomain.GetType(typeof(string)));
            IMethod method = mType.GetMethod("RemoveModule", paramList, null);
            mAppdomain.Invoke(method, mInstance, moduleName);
        }

        public void Release()
        {
            IMethod method = mType.GetMethod("Release", 0);
            mAppdomain.Invoke(method, mInstance, null);
        }

        /// <summary>
        /// 通用的热更新执行方法
        /// </summary>
        /// <param name="funName"></param>
        /// <param name="parmList"></param>
        public void ExcuteFuction(string funName, object[] parmList)
        {
            if (parmList == null)
            {
                //按参数数量获取方法  仅确保在同一方法名下 不同参数数量可用
                IMethod method = mType.GetMethod(funName, 0);
                mAppdomain.Invoke(method, mInstance, null);
            }
            else
            {
                //按参数类型获取方法
                List<IType> pList = new List<IType>();
                for (int i = 0; i < parmList.Length; i++)
                {
                    pList.Add(mAppdomain.GetType(parmList[i].GetType()));
                }

                IMethod method = mType.GetMethod(funName, pList, null);
                mAppdomain.Invoke(method, mInstance, parmList);
            }
        }

        public void Update(float deltaTime)
        {
            List<IType> paramList = new List<IType>();
            paramList.Add(mAppdomain.GetType(typeof(float)));
            IMethod method = mType.GetMethod("Update", paramList, null);
            mAppdomain.Invoke(method, mInstance, deltaTime);
        }

        public void SwitchLanguage()
        {
            IMethod method = mType.GetMethod("SwitchLanguage", 0);
            mAppdomain.Invoke(method, mInstance, null);
        }

    }
}