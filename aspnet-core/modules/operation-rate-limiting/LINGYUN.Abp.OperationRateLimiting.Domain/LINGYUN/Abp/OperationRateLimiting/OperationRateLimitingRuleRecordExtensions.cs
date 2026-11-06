using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingRuleRecordExtensions
{
    public static OperationRateLimitingRuleDefinition ToDefinition(this OperationRateLimitingRuleRecord record)
    {
        return new OperationRateLimitingRuleDefinition(record.Name!, record.PartitionType)
        {
            Algorithm = record.Algorithm,
            Duration = record.Duration,
            MaxCount = record.MaxCount,
            PartitionResolverName = record.PartitionResolverName,
            IsMultiTenancyEnabled = record.IsMultiTenancyEnabled,
            ErrorCode = record.ErrorCode,
            Properties = new Dictionary<string, string>(record.Properties, StringComparer.Ordinal)
        };
    }

    public static OperationRateLimitingRuleRecord ToRecord(
        this OperationRateLimitingRuleDefinition definition,
        Guid id,
        Guid policyId,
        int sortOrder)
    {
        return new OperationRateLimitingRuleRecord(id, policyId, definition.Name, definition.Algorithm)
        {
            Duration = definition.Duration,
            MaxCount = definition.MaxCount,
            SortOrder = sortOrder,
            PartitionType = definition.PartitionType,
            PartitionResolverName = definition.PartitionResolverName,
            IsMultiTenancyEnabled = definition.IsMultiTenancyEnabled,
            ErrorCode = definition.ErrorCode,
            Properties = new Dictionary<string, string>(definition.Properties, StringComparer.Ordinal)
        };
    }
}
