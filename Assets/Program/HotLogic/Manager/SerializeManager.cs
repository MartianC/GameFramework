using Newtonsoft.Json;

namespace HotLogic
{
    public static class SerializeManager
    {
        public static string Serialize(object obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return content;
        }
        
        public static T DeSerialize<T>(string content)
        {
            T obj = JsonConvert.DeserializeObject<T>(content);
            return obj;
        }
    }
}