using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using System.Collections.Generic;

namespace System.Text.Json.Serialization;

internal class MemberExternalAttributeConverter : JsonConverter<MemberExternalAttribute>
{
    public override MemberExternalAttribute? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var memberExternalAttribute = new MemberExternalAttribute();

        if (root.TryGetProperty("attrs", out var externalAttrsElement) && externalAttrsElement.ValueKind == JsonValueKind.Array)
        {
            var attrs = new List<MemberAttribute>();
            foreach (var externalAttrElement in externalAttrsElement.EnumerateArray())
            {
                if (externalAttrElement.TryGetProperty("type", out var typeElement) && typeElement.ValueKind != JsonValueKind.Null)
                {
                    var attributeType = typeElement.GetInt32();
                    if (Enum.IsDefined(typeof(AttributeType), attributeType))
                    {
                        attrs.Add(MemberAttributeDeserializeFactory.CreateExternalAttribute((AttributeType)attributeType, externalAttrElement));
                    }
                }
            }
            memberExternalAttribute.Attributes = attrs.ToArray();
        }

        return memberExternalAttribute;
    }

    public override void Write(Utf8JsonWriter writer, MemberExternalAttribute value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.Attributes != null)
        {
            writer.WritePropertyName("attrs");
            JsonSerializer.Serialize(writer, value.Attributes, value.Attributes.GetType(), options);
        }

        writer.WriteEndObject();
    }
}
