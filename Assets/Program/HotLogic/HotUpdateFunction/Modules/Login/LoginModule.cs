using GameCore;
using Platform;

namespace HotLogic
{
    public class LoginModule: BaseModule
    {
        private LoginModuleView _LoginModuleView;
        
        /// <summary>
        /// 构造函数里进行通用的操作
        /// 1.实例化Prefab
        /// 2.初始化视图层
        /// 3.进行通用委托的注册
        /// </summary>
        public LoginModule()
        {
            //使用同步或异步加载方式
            UIManager.Instance.OpenCommonUIAsync(UIPrefabPathDefine.LoginUI, ModuleConfig.MODULE_GAME_LOGIN, EUIState.Dynamic, (go) =>
            {
                _LoginModuleView = new LoginModuleView();
                InitPre(_LoginModuleView, go.GetComponent<UIAbstractViewObject>());
            }, TweenCompleted, true, false);
            //根据需要注册 通用方法 
            //_mScript.RegistCommAction(new Dictionary<string, System.Action> { { FUNCTION_NAME_UPDATE, Update }, { FUNCTION_NAME_ONDESTROY, OnDestroy }});
        }

        #region override

        public override void RegistEvent()
        {
        }

        public override void EventBinding()
        {
            _LoginModuleView._btnLogin.onClick.RemoveAllListeners();
            _LoginModuleView._btnLogin.onClick.AddListener(OnBtnLogin);
        }

        public override void TweenCompleted()
        {
        }

        public override void Release()
        {
        }

        public override void SwitchLanguage()
        {
        }

        public override void OnInit()
        {
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        public override void OnDestroy()
        {
        }

        #endregion

        #region Event

        void OnBtnLogin()
        {
            MessageManager.SendMessage_Login();
        }

        #endregion
    }
}