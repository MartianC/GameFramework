using System;
using System.Collections.Generic;

namespace HotLogic
{
    public class GameDataManager:TSingleton<GameDataManager>
    {
        public Dictionary<GameDataType, BaseGameDataAll> DataDict = new Dictionary<GameDataType, BaseGameDataAll>();
        
        public void Init()
        {
            var dataTypes = Enum.GetValues(typeof(GameDataType));
            foreach (var dataType in dataTypes)
            {
                //根据DataType获取对应的数据类型
                var type = Type.GetType("HotLogic." + dataType.ToString() + "DataAll");
                //根据数据类型创建对应的实例
                var instance = Activator.CreateInstance(type);
                //调用初始化方法
                var method = type.GetMethod("Init");
                method.Invoke(instance, null);
                DataDict[(GameDataType)dataType] = instance as BaseGameDataAll;
            }
        }
        
        public BaseGameDataAll GetGameData(GameDataType dataType)
        {
            if (DataDict.ContainsKey(dataType))
            {
                return DataDict[dataType];
            }
            return null;
        }
    }
}