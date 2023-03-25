namespace GameCore
{
    public class GameConfig
    {
        /// <summary>
        /// Debug标记
        /// </summary>
        public static EDebugLevel DebugLevel = EDebugLevel.Dev;

        public static string WebSocketUrl = "ws://127.0.0.1:4200";
        
        public static string TcpServerUrl = "127.0.0.1";
        //public static string TcpServerUrl = "192.168.3.44";
        public static int TcpServerPort = 4300;

        public static bool GetDefineStatus(EDefineType type)
        {
            switch (type)
            {
                case EDefineType.UNITY_EDITOR:
#if UNITY_EDITOR
                    return true;
#else
                    return false;
#endif
                case EDefineType.UNITY_STANDALONE_WIN:
#if UNITY_STANDALONE_WIN
                    return true;
#else
                    return false;
#endif
                case EDefineType.UNITY_IPHONE:
#if UNITY_IPHONE
                    return true;
#else
                    return false;
#endif
                case EDefineType.UNITY_ANDROID:
#if UNITY_ANDROID
                    return true;
#else
                    return false;
#endif
                case EDefineType.UNITY_WEBGL:
#if UNITY_WEBGL
                    return true;
#else
                    return false;
#endif

                case EDefineType.DEBUG:
#if ENABLE_DEBUG
                return true;
#else
                    return false;
#endif
                default:
                    return false;
            }
        }

    }
}