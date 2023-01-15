

using System.Collections;
using UnityEngine;

//下面这行为了取消使用WWW的警告，Unity2018以后推荐使用UnityWebRequest，处于兼容性考虑Demo依然使用WWW
#pragma warning disable CS0618

namespace GameCore
{
    public class GameLogicManager: TMonoSingleton<GameLogicManager>
    {
        //标记是否进行热更新
        private bool _isHotUp = true;

        private GameHotUpdateManager _gameHotUpdateManager = new GameHotUpdateManager();

        public GameHotUpdateManager GameHotUpdateManager { get => _gameHotUpdateManager; }

        public override void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            if (_isHotUp)
            {
                GameHotUpdateManager.Instance.Init();
                StartCoroutine(CheckHotUpdate());
            }
        }
        
        
        IEnumerator CheckHotUpdate()
        {
            //检查热更资源
            
            
            //检查热更代码
            yield return CheckHotFix();
            
            //进入热更逻辑
            //GameHotUpdateManager.HotUpdateAdapter.InitGameLogic();
            GameHotUpdateManager.Instance.OnHotFixLoaded();
        }

        IEnumerator CheckHotFix()
        {
            //正常项目中应该是自行从其他地方下载dll，或者打包在AssetBundle中读取，平时开发以及为了演示方便直接从StreammingAssets中读取，
            //正式发布的时候需要大家自行从其他地方读取dll

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //这个DLL文件是直接编译HotFix_Project.sln生成的，已经在项目中设置好输出目录为StreamingAssets，在VS里直接编译即可生成到对应目录，无需手动拷贝
            //工程目录在Assets\Samples\ILRuntime\1.6\Demo\HotFix_Project~
            //以下加载写法只为演示，并没有处理在编辑器切换到Android平台的读取，需要自行修改
#if UNITY_ANDROID
        WWW www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
            WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/HotFix_Project.dll");
#endif
            while (!www.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(www.error))
                UnityEngine.Debug.LogError(www.error);
            byte[] dll = www.bytes;
            www.Dispose();

            //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
#if UNITY_ANDROID
        www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
            www = new WWW("file:///" + Application.streamingAssetsPath + "/HotFix_Project.pdb");
#endif
            while (!www.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(www.error))
                UnityEngine.Debug.LogError(www.error);
            byte[] pdb = www.bytes;
            www.Dispose();

            GameHotUpdateManager.Instance.LoadHotFixAssembly(dll, pdb);
        }
    }
    
}
