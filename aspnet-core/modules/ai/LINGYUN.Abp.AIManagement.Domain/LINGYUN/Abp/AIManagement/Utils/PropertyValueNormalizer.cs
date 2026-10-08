using System.Collections.Generic;
using System.Text.Json;

namespace LINGYUN.Abp.AIManagement.Utils;

internal static class PropertyValueNormalizer
{
    public static object? Normalize(object? value)
    {
        return value switch
        {
            null => null,
            JsonElement element => NormalizeJsonElement(element),
            JsonDocument document => NormalizeJsonElement(document.RootElement),
            IDictionary<string, string> stringDictionary => new Dictionary<string, string>(stringDictionary),
            IDictionary<string, object?> dictionary => NormalizeDictionary(dictionary),
            IEnumerable<KeyValuePair<string, object?>> pairs => NormalizeDictionary(pairs),
            _ => value,
        };
    }

    private static object? NormalizeJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                if (element.TryGetInt64(out var int64))
                {
                    return int64;
                }
                if (element.TryGetDecimal(out var dec))
                {
                    return dec;
                }
                return element.GetDouble();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
                return null;
            case JsonValueKind.Object:
                return NormalizeJsonObject(element);
            case JsonValueKind.Array:
                var list = new List<object?>();
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(NormalizeJsonElement(item));
                }
                return list;
            default:
                return null;
        }
    }

    private static object NormalizeJsonObject(JsonElement element)
    {
        var allScalar = true;
        foreach (var property in element.EnumerateObject())
        {
            if (property.Value.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
            {
                allScalar = false;
                break;
            }
        }

        if (allScalar)
        {
            var stringDictionary = new Dictionary<string, string>();
            foreach (var property in element.EnumerateObject())
            {
                stringDictionary[property.Name] = ToStringValue(property.Value);
            }
            return stringDictionary;
        }

        var dictionary = new Dictionary<string, object?>();
        foreach (var property in element.EnumerateObject())
        {
            dictionary[property.Name] = NormalizeJsonElement(property.Value);
        }
        return dictionary;
    }

    private static string ToStringValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Null or JsonValueKind.Undefined => string.Empty,
            JsonValueKind.True => bool.TrueString,
            JsonValueKind.False => bool.FalseString,
            _ => element.GetRawText(),
        };
    }

    private static Dictionary<string, object?> NormalizeDictionary(IEnumerable<KeyValuePair<string, object?>> source)
    {
        var dictionary = new Dictionary<string, object?>();
        foreach (var (key, value) in source)
        {
            dictionary[key] = Normalize(value);
        }
        return dictionary;
    }
}
