using System;
using System.IO;
using UnityEngine;

namespace GameCore
{
    public class GameHotUpdateManager:TSingleton<GameHotUpdateManager>
    {
        //全局热更新唯一实例
        private ILRuntime.Runtime.Enviorment.AppDomain _mAppdomain;
        private MemoryStream _msDll = null;
        private MemoryStream _msPdb = null;

        private HotUpdateAdapterInner _hotUpdateAdapter;

        public HotUpdateAdapterInner HotUpdateAdapter { get => _hotUpdateAdapter; }

        
        public void Init()
        {
            _mAppdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        }
        
        
        public void LoadHotFixAssembly(byte[] dll, byte[] pdb)
        {
            _msDll = new MemoryStream(dll);

            if (pdb != null)
            {
                _msPdb = new MemoryStream(pdb);
            }
            
            try
            {
                _mAppdomain.LoadAssembly(_msDll, _msPdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch (Exception e)
            {
                Debug.LogError("加载热更DLL失败，请确保已经通过VS打开Assets/Samples/ILRuntime/1.6/Demo/HotFix_Project/HotFix_Project.sln编译过热更DLL");
            }
            
            
            InitializeILRuntime();
        }
        
        void InitializeILRuntime()
        {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            _mAppdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
        }


        public void OnHotFixLoaded()
        {
            //HelloWorld，第一次方法调用
            //_mAppdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest", null, null);
            _mAppdomain.Invoke("HotLogic.Main","Init",null,null);
        }
    }
}