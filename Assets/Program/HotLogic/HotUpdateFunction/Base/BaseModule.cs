using GameCore;

namespace HotLogic
{
    public abstract class BaseModule
    {
        protected object Cmd;
        public void SetParams(object cmd)
        {
            Cmd = cmd;
        }
        /// <summary>
        /// 模块的类型
        /// </summary>
        protected EModuleType _moduleType = EModuleType.Common;
        /// <summary>
        /// 模块名字
        /// </summary>
        protected string _moduleName = "";
        /// <summary>
        /// 模块的视图
        /// </summary>
        protected BaseView _view;
        /// <summary>
        /// Unity通用功能桥接脚本
        /// </summary>
        protected UIAbstractViewObject _mScript;
        /// <summary>
        /// 设置视图并初始化 构造方法中调用
        /// </summary>
        /// <param name="view"></param>
        /// <param name="mScript"></param>
        public void InitPre(BaseView view, UIAbstractViewObject mScript)
        {
            _view = view;
            _mScript = mScript;
            _view.GetObjects(_mScript);
            EventBinding();
            RegistEvent();
            OnInit();
            OnShow();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public abstract void OnInit();
        /// <summary>
        /// 显示界面
        /// </summary>
        public abstract void OnShow();
        /// <summary>
        /// UI动画播完 或者 UI准备好的回调
        /// </summary>
        public abstract void TweenCompleted();
        /// <summary>
        /// 事件注册
        /// </summary>
        public abstract void RegistEvent();
        /// <summary>
        /// UI事件绑定
        /// </summary>
        public abstract void EventBinding();
        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Release();
        /// <summary>
        /// 多语言切换
        /// </summary>
        public abstract void SwitchLanguage();
        /// <summary> 
        /// 隐藏界面
        /// </summary>
        public abstract void OnHide();
        /// <summary>
        /// 销毁界面
        /// </summary>
        public abstract void OnDestroy();

    }
}