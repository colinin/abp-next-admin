using System;

namespace Newtonsoft.Json
{
    public class HexLongConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long v = value is ulong ? (long)(ulong)value : (long)value;
            writer.WriteValue(v.ToString());
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = reader.Value as string;
            long lValue = long.Parse(value);
            return typeof(ulong) == objectType ? (object)(ulong)lValue : lValue;
        }
        public override bool CanConvert(Type objectType)
        {
            switch (objectType.FullName)
            {
                case "System.Int64":
                case "System.UInt64":
                    return true;
                default:
                    return false;
            }
        }
    }
}
