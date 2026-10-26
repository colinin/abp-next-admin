using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// Reads <c>nodeProperty</c> whether the designer wrote it as an object or as a serialised JSON string.
/// </summary>
public sealed class AntFlowNodePropertyConverter : JsonConverter<AntFlowNodeProperty>
{
    public override AntFlowNodeProperty? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.String:
                var raw = reader.GetString();
                if (string.IsNullOrWhiteSpace(raw))
                {
                    return new AntFlowNodeProperty();
                }

                try
                {
                    using var stringDocument = JsonDocument.Parse(raw);
                    return stringDocument.RootElement.ValueKind == JsonValueKind.Object
                        ? stringDocument.RootElement.Deserialize<AntFlowNodeProperty>(options) ?? new AntFlowNodeProperty()
                        : new AntFlowNodeProperty();
                }
                catch (JsonException)
                {
                    // An unparsable nodeProperty is treated as an empty node property so the rest of
                    // the design can still be compiled.
                    return new AntFlowNodeProperty();
                }
            case JsonTokenType.StartObject:
                using (var document = JsonDocument.ParseValue(ref reader))
                {
                    return document.RootElement.Deserialize<AntFlowNodeProperty>(options) ?? new AntFlowNodeProperty();
                }
            default:
                reader.Skip();
                return new AntFlowNodeProperty();
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        AntFlowNodeProperty value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
