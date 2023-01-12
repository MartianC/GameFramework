namespace GameCore
{
    public class GameConfig
    {
        /// <summary>
        /// Debug标记
        /// </summary>
        public static EDebugLevel DebugLevel
        {
            get
            {
#if ENABLE_DEBUG
                return EDebugLevel.Dev;
#endif
                return EDebugLevel.Prod;
            }
        }

    }
}