using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Context passed to the rate limiting checker.
/// </summary>
public class OperationRateLimitingContext
{
    /// <summary>
    /// The value used by <see cref="OperationRateLimitingPartitionType.Parameter"/>,
    /// <see cref="OperationRateLimitingPartitionType.Email"/> and
    /// <see cref="OperationRateLimitingPartitionType.PhoneNumber"/> partitions.
    /// </summary>
    public string? Parameter { get; set; }

    /// <summary>
    /// Additional values available to custom partition resolvers and copied to the exception data.
    /// Do not put secrets here: they may reach an error response.
    /// </summary>
    public Dictionary<string, object?> Properties { get; set; } = new();
}
