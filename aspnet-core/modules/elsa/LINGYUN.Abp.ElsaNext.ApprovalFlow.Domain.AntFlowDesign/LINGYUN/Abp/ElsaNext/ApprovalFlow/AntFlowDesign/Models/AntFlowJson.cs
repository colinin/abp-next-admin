using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// Serializer options for the AntFlow contract. AntFlow payloads are loosely typed in the
/// designer (numbers stored as strings and booleans stored as 0/1), so the options are lenient.
/// </summary>
public static class AntFlowJson
{
    public static readonly JsonSerializerOptions Options = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        options.Converters.Add(new AntFlowFlexibleStringConverter());
        options.Converters.Add(new AntFlowFlexibleInt32Converter());
        options.Converters.Add(new AntFlowFlexibleBooleanConverter());

        return options;
    }
}
