namespace System.Text.Json.Serialization;

#nullable enable
public class EnumToNumberStringConverter<T> : JsonConverter<T?> where T : struct, Enum
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }
            if (int.TryParse(stringValue, out var intValue))
            {
                return (T)Enum.ToObject(typeof(T), intValue);
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return (T)Enum.ToObject(typeof(T), reader.GetInt32());
        }

        throw new JsonException($"Unable to convert value to enum {typeof(T).Name}");
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(Convert.ToInt32(value).ToString());
        }
    }
}
#nullable disable
