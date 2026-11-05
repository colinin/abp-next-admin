using System;
using System.Globalization;
using System.Linq;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Shared parsing/formatting for the algorithm rule definition extensions.
/// </summary>
internal static class OperationRateLimitingRuleDefinitionValueHelper
{
    public static int GetInt(OperationRateLimitingRuleDefinition definition, string propertyName, int defaultValue)
    {
        var value = definition.GetPropertyOrNull(propertyName);

        return value == null ? defaultValue : int.Parse(value, CultureInfo.InvariantCulture);
    }

    public static string FormatInt(int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static string BuildKeyDescriptor(OperationRateLimitingRuleDefinition definition)
    {
        if (!string.IsNullOrWhiteSpace(definition.Name))
        {
            return definition.Name!;
        }

        var descriptor = $"{definition.Algorithm}:{definition.Duration.TotalSeconds}:{definition.MaxCount}:{definition.PartitionType}";

        if (definition.IsMultiTenancyEnabled)
        {
            descriptor += ":tenant";
        }

        foreach (var property in definition.Properties.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            descriptor += $":{property.Key}={property.Value}";
        }

        return descriptor;
    }
}
