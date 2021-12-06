using Newtonsoft.Json;

namespace System
{
    public static class ObjectSerializerExtensions
    {
        private readonly static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        public static string SerializeObject(this object obj, bool handlingString = false, JsonSerializerSettings serializerSettings = null)
        {
            if (obj is string objStr && !handlingString)
            {
                return objStr;
            }
            return JsonConvert.SerializeObject(obj, serializerSettings ?? SerializerSettings);
        }

        public static object DeserializeObject(this string str, JsonSerializerSettings serializerSettings = null)
        {
            return JsonConvert.DeserializeObject(str ?? string.Empty, serializerSettings ?? SerializerSettings);
        } 
    }
}
