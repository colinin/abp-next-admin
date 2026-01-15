using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging;
public class EntityChangeTypeConverter : JsonConverter<EntityChangeType>
{
    public override EntityChangeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (int.TryParse(stringValue, out var intValue))
            {
                return (EntityChangeType)Enum.ToObject(typeof(EntityChangeType), intValue);
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return (EntityChangeType)Enum.ToObject(typeof(EntityChangeType), reader.GetInt32());
        }

        throw new JsonException($"Unable to convert value to enum {typeof(EntityChangeType).Name}");
    }

    public override void Write(Utf8JsonWriter writer, EntityChangeType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Convert.ToInt32(value).ToString());
    }
}
