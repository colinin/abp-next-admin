namespace System.Text.Json.Serialization;

public class EnumToStringConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default(T);
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return default(T);
            }
            if (int.TryParse(stringValue, out var intValue))
            {
                return (T)Enum.ToObject(typeof(T), intValue);
            }
            if (Enum.TryParse<T>(stringValue, out var enumValue))
            {
                return enumValue;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return (T)Enum.ToObject(typeof(T), reader.GetInt32());
        }

        throw new JsonException($"Unable to convert value to enum {typeof(T).Name}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}