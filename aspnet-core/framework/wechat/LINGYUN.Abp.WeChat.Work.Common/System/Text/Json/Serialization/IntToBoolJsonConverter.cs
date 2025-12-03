namespace System.Text.Json.Serialization;

public class IntToBoolJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                var intValue = reader.GetInt64();
                return intValue == 1;
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
        }

        throw new JsonException($"Cannot convert {reader.TokenType} to bool.");
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}

