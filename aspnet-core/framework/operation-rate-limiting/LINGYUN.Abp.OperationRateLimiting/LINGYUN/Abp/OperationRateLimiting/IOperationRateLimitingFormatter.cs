using System;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Formats durations shown in error messages and exception data.
/// </summary>
public interface IOperationRateLimitingFormatter
{
    /// <summary>
    /// Formats a window/lease duration (e.g. "5 minutes", "2 hours 30 minutes").
    /// </summary>
    string FormatDuration(TimeSpan duration);

    /// <summary>
    /// Formats a retry-after duration. Returns an empty string when the request is permanently denied.
    /// </summary>
    string FormatRetryAfter(TimeSpan? retryAfter);
}
