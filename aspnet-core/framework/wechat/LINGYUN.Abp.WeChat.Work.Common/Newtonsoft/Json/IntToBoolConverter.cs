using System;

namespace Newtonsoft.Json;

public class IntToBoolConverter : JsonConverter<bool>
{
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Integer:
                return reader.ReadAsInt32() == 1;
            case JsonToken.Boolean:
                return reader.ReadAsBoolean() == true;
            default:
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing bool.");
        }
    }

    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
    {
        writer.WriteValue(value ? 1 : 0);
    }
}
