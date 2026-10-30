using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

internal sealed class AntFlowFlexibleInt32Converter : JsonConverter<int>
{
    public override int Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                if (reader.TryGetInt32(out var number))
                {
                    return number;
                }
                if (reader.TryGetDouble(out var real))
                {
                    return (int)real;
                }
                break;
            case JsonTokenType.String:
                var text = reader.GetString();
                if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                {
                    return parsed;
                }
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedReal))
                {
                    return (int)parsedReal;
                }
                break;
            case JsonTokenType.True:
                return 1;
            case JsonTokenType.False:
                return 0;
            case JsonTokenType.Null:
                return 0;
        }

        reader.Skip();
        return 0;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
