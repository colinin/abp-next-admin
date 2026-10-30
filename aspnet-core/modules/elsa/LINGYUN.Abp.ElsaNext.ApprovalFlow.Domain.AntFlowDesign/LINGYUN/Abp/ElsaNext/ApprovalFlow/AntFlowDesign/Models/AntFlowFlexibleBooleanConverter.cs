using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

internal sealed class AntFlowFlexibleBooleanConverter : JsonConverter<bool>
{
    public override bool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Number:
                return !reader.TryGetInt32(out var number) || number != 0;
            case JsonTokenType.String:
                var text = reader.GetString();
                if (bool.TryParse(text, out var boolean))
                {
                    return boolean;
                }
                if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numeric))
                {
                    return numeric != 0;
                }
                return string.Equals(text, "且", StringComparison.Ordinal) ||
                       string.Equals(text, "and", StringComparison.OrdinalIgnoreCase);
            case JsonTokenType.Null:
                return false;
        }

        reader.Skip();
        return false;
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
