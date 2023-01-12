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
    
}