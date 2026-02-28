using System;

namespace Newtonsoft.Json;

#nullable enable
public class EnumToNumberStringConverter<T> : JsonConverter where T : struct, Enum
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(T?);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        try
        {
            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value?.ToString();
                if (string.IsNullOrEmpty(stringValue))
                {
                    return null;
                }

                if (int.TryParse(stringValue, out var intValue))
                {
                    return (T?)Enum.ToObject(typeof(T), intValue);
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                return (T?)Enum.ToObject(typeof(T), Convert.ToInt32(reader.Value));
            }
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException($"Error converting value {reader.Value} to type '{objectType}'.", ex);
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing enum.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
        }
        else
        {
            var enumValue = (T)value;
            writer.WriteValue(Convert.ToInt32(enumValue).ToString());
        }
    }
}
#nullable disable

