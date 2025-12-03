using System;

namespace Newtonsoft.Json;

public class IntToBoolConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Integer:
                var value = (long)reader.Value;
                return value == 1;
            case JsonToken.Boolean:
                return (bool)reader.Value;
            default:
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing bool.");
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var boolValue = (bool)value;
        writer.WriteValue(boolValue ? 1 : 0);
    }
}
