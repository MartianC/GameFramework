namespace GameCore
{
    public enum EDebugLevel
    {
        /// <summary>
        /// 生产环境
        /// </summary>
        Prod,
        /// <summary>
        /// 开发环境
        /// </summary>
        Dev,
    }
    /// <summary>
    /// 宏定义类型
    /// </summary>
    public enum EDefineType
    {
        UNITY_EDITOR,
        UNITY_STANDALONE_WIN,
        UNITY_IPHONE,
        UNITY_ANDROID,
        UNITY_WEBGL,
        DEBUG,
    }
    /// <summary>
    /// UI节点
    /// </summary>
    public enum EUIState
    {
        /// <summary>
        /// 预加载资源节点
        /// </summary>
        PreLoadRoot,
        /// <summary>
        /// 动态功能
        /// </summary>
        Dynamic,
        /// <summary>
        /// 公共功能
        /// </summary>
        GlobaRoot,
        /// <summary>
        /// 交互Loading
        /// </summary>
        DisplayLoadingRoot,
        /// <summary>
        /// 通知
        /// </summary>
        Notice,
        /// <summary>
        /// 最上层
        /// </summary>
        TopSideRoot,
        MAX_,
    }
    /// <summary>
    /// Layer
    /// </summary>
    public enum ELayer
    {
        /// <summary>
        /// 默认3D层
        /// </summary>
        Default,
        /// <summary>
        /// UI层
        /// </summary>
        UI,
    }
    public enum EModuleType
    {
        /// <summary>
        /// 通用
        /// </summary>
        Common,
        /// <summary>
        /// 队列类型
        /// </summary>
        Queue,
    }

}