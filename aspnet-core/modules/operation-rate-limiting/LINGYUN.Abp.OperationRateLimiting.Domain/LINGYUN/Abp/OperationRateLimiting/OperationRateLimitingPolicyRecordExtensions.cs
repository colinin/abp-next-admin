using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingPolicyRecordExtensions
{
    /// <summary>
    /// Maps a persisted policy record (with its rules) to the runtime policy definition.
    /// </summary>
    public static OperationRateLimitingPolicy ToPolicy(this OperationRateLimitingPolicyRecord record)
    {
        return new OperationRateLimitingPolicy(record.Name)
        {
            DisplayName = record.DisplayName,
            ErrorCode = record.ErrorCode,
            IsEnabled = record.IsEnabled,
            Rules = record.Rules
                .OrderBy(rule => rule.SortOrder)
                .Select(rule => rule.ToDefinition())
                .ToList()
        };
    }
}
