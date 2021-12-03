using Newtonsoft.Json;

namespace System
{
    public static class ObjectSerializerExtensions
    {
        private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        public static string SerializeObject(this object obj)
        {
            return JsonConvert.SerializeObject(obj, SerializerSettings);
        }

        public static object DeserializeObject(this string str)
        {
            return JsonConvert.DeserializeObject(str, SerializerSettings);
        } 
    }
}
