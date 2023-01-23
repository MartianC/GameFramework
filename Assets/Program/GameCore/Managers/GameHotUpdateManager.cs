using System;
using System.IO;
using ILRuntime.CLR.TypeSystem;
using Platform;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class GameHotUpdateManager : TSingleton<GameHotUpdateManager>
    {
        //全局热更新唯一实例
        private ILRuntime.Runtime.Enviorment.AppDomain _mAppdomain;
        private MemoryStream _msDll = null;
        private MemoryStream _msPdb = null;

        private HotUpdateAdapterInner _hotUpdateAdapter;

        public HotUpdateAdapterInner HotUpdateAdapter
        {
            get => _hotUpdateAdapter;
        }


        public void InitHotUpdate()
        {
            byte[] dllbyte = LoadHotUpdateDll();
            byte[] pdbbyte = LoadHotUpdatePdb();

            if (dllbyte == null)
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError("初始化热更新的dll文件无法就加载");
                }

                return;
            }

            _msDll = new System.IO.MemoryStream(dllbyte);
            _msPdb = new System.IO.MemoryStream(pdbbyte);

            _mAppdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            //当正式发布不需要调试信息的时候可以不加载pbd
            _mAppdomain.LoadAssembly(_msDll, _msPdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#if UNITY_EDITOR
            _mAppdomain.DebugService.StartDebugService(56000);
#endif

            InitializeILRuntimeMethodDelegate();
            InitializeILRuntime();
            //项目发布需要开启绑定 该绑定由工具(Tools/ILRuntime/Generate CLR Binding Code by Analysis)自动分析dll后生成 目录在Assets/ThirdPartTools/IL/Generated 发布时只需要打开注释就可以
            //ILRuntime.Runtime.Generated.CLRBindings.Initialize(_mAppdomain); 

            InitAdapter();
        }
        
        private void InitAdapter()
        {
            IType type = _mAppdomain.LoadedTypes["HotUpdate.HotUpdateAdaperOuter"];
            object obj1 = ((ILType)type).Instantiate();
            _hotUpdateAdapter = new HotUpdateAdapterInner(_mAppdomain, type, obj1);
        }

        /// <summary>
        /// 加载dll
        /// </summary>
        /// <returns></returns>
        private byte[] LoadHotUpdateDll()
        {
            byte[] bytes = null;
            ABResources.LoadRes<TextAsset>("HotUpdateScripts.dll.bytes", (data, request) =>
            {
                bytes = data.bytes;
                request.Release();
            }, ABResources.MatchMode.Dll);
            return bytes;
        }

        /// <summary>
        /// 加载pbd
        /// </summary>
        /// <returns></returns>
        private byte[] LoadHotUpdatePdb()
        {
            byte[] bytes = null;
            ABResources.LoadRes<TextAsset>("HotUpdateScripts.pdb.bytes", (data, request) =>
            {
                bytes = data.bytes;
                request.Release();
            }, ABResources.MatchMode.Dll);
            return bytes;
        }
        
        private void InitializeILRuntimeMethodDelegate()
        {
            #region 对委托的注册的解释

            //在这里进行委托的适配器的注册
            //完全在热更DLL内部使用的委托，直接可用，不需要做任何处理。 应该尽量减少不必要的跨域委托调用，如果委托只在热更DLL中用，是不需要进行任何注册的
            //如果需要跨域调用委托（将热更DLL里面的委托实例传到Unity主工程用）, 就需要注册适配器
            //因为IL2CPP模式下，不能动态生成类型，为了避免出现不可预知的问题，没有通过反射的方式创建委托实例，因此需要手动进行一些注册
            //委托的使用中尽量使用万用型 Action Func 这样可以减少转化器的编写 
            //如果没有注册委托适配器，运行时会报错并提示需要的注册代码，直接复制粘贴到ILRuntime初始化的地方
            //例如 ：Action<string> 的参数为一个string 需要 _mAppdomain.DelegateManager.RegisterMethodDelegate<string>();
            //       Action<string,int> 需要 _mAppdomain.DelegateManager.RegisterMethodDelegate<string,int>();
            //       Func<int, string, bool>需要 _mAppdomain.DelegateManager.RegisterFunctionDelegate<int, string, bool>();
            //当自己定义的委托需要进行转换器转换
            //例如 ： delegate void myDelegateMethod(int a); 除了 _mAppdomain.DelegateManager.RegisterMethodDelegate<int>(); 还需要注册转换器
            //        _mAppdomain.DelegateManager.RegisterDelegateConvertor<myDelegateMethod>((action) =>
            //        {
            //                    //转换器的目的是把Action或者Func转换成正确的类型，这里则是把Action<int>转换成myDelegateMethod
            //            return new myDelegateMethod((a) =>
            //            {
            //                        //调用委托实例
            //                         ((System.Action<int>)action)(a);
            //            });
            //         });
            //        
            //         delegate string myDelegateFunction(int a); 除了appdomain.DelegateManager.RegisterFunctionDelegate<int, string>(); 还需要注册
            //        _mAppdomain.DelegateManager.RegisterDelegateConvertor<myDelegateFunction>((action) =>
            //        {
            //            return new myDelegateFunction((a) =>
            //            {
            //                return ((System.Func<int, string>)action)(a);
            //            });
            //        });
            //UGUI中常用的委托的转换
            //例如：UnityAction<float>
            //        _mAppdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<float>>((action) =>
            //        {
            //            return new UnityEngine.Events.UnityAction<float>((a) =>
            //            {
            //                ((System.Action<float>)action)(a);
            //            });
            //        });

            #endregion

            //用到的跨域委托在这里进行注册
            _mAppdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();

            _mAppdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) => { return new UnityAction(() => { ((Action)action)(); }); });
            _mAppdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            // _mAppdomain.DelegateManager.RegisterDelegateConvertor<EventTriggerListener.PointDataDelegate>((action) => { return new EventTriggerListener.PointDataDelegate((a) => { ((Action<UnityEngine.EventSystems.PointerEventData>)action)(a); }); });
            _mAppdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
            // _mAppdomain.DelegateManager.RegisterFunctionDelegate<EnhancedUI.EnhancedScroller.EnhancedScroller, int>();
            // _mAppdomain.DelegateManager.RegisterDelegateConvertor<EnhancedUI.EnhancedScroller.ScrollerGetNumberOfCellsDelegate>((act) =>
            // {
            //     return new EnhancedUI.EnhancedScroller.ScrollerGetNumberOfCellsDelegate((scroller) => { return ((Func<EnhancedUI.EnhancedScroller.EnhancedScroller, int>)act)(scroller); });
            // });
            // _mAppdomain.DelegateManager.RegisterFunctionDelegate<EnhancedUI.EnhancedScroller.EnhancedScroller, int, float>();
            // _mAppdomain.DelegateManager.RegisterDelegateConvertor<EnhancedUI.EnhancedScroller.ScrollerGetCellViewSizeDelegate>((act) =>
            // {
            //     return new EnhancedUI.EnhancedScroller.ScrollerGetCellViewSizeDelegate((scroller, dataIndex) => { return ((Func<EnhancedUI.EnhancedScroller.EnhancedScroller, System.Int32, System.Single>)act)(scroller, dataIndex); });
            // });
            // _mAppdomain.DelegateManager.RegisterFunctionDelegate<EnhancedUI.EnhancedScroller.EnhancedScroller, System.Int32, System.Int32, EnhancedUI.EnhancedScroller.EnhancedScrollerCellView>();
            // _mAppdomain.DelegateManager.RegisterDelegateConvertor<EnhancedUI.EnhancedScroller.ScrollerGetCellViewDelegate>((act) =>
            // {
            //     return new EnhancedUI.EnhancedScroller.ScrollerGetCellViewDelegate((scroller, dataIndex, cellIndex) =>
            //     {
            //         return ((Func<EnhancedUI.EnhancedScroller.EnhancedScroller, System.Int32, System.Int32, EnhancedUI.EnhancedScroller.EnhancedScrollerCellView>)act)(scroller, dataIndex, cellIndex);
            //     });
            // });
            // _mAppdomain.DelegateManager.RegisterDelegateConvertor<EnhancedUI.EnhancedScroller.ScrollerInitCompletedDelegate>((act) => { return new EnhancedUI.EnhancedScroller.ScrollerInitCompletedDelegate(() => { ((Action)act)(); }); });
            _mAppdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Texture2D>();
            _mAppdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.AudioClip>();
            _mAppdomain.DelegateManager.RegisterMethodDelegate<System.String>();
            _mAppdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
        }

        private void InitializeILRuntime()
        {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            _mAppdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            //注册跨域使用的实现或着跨域的继承（！！！开发规范限定联允许进行跨域的继承 为减少复杂度和开销）这里主要注册常用的协程 方便UI开发中的使用
            _mAppdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());


            //这里注册值类型Binder 可以在dll中使用主工程中的值类型的时候减少性能消耗和GC Alloc
            _mAppdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            _mAppdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            _mAppdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());


            //这里对LitJson进行注册 方便在热更功能中使用json
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(_mAppdomain);
            //这里对debug.log 方法进行重定向 使在热更新中使用debug.log的时候可以显示热更新中的调用堆栈
            // DebugLog.RegisterILRuntimeCLRRedirection(_mAppdomain);
        }
        
    }
}