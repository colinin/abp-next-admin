using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Describes one atomic operation of a rate limiting algorithm.
/// </summary>
/// <remarks>
/// The request contains everything a storage implementation needs to execute the algorithm
/// atomically (for example in a single Redis Lua script), so the store does not have to run
/// user code while holding a lock.
/// </remarks>
public class OperationRateLimitingStoreRequest
{
    /// <summary>
    /// Fully qualified counter key (already includes the partition and the tenant).
    /// </summary>
    public string Key { get; set; } = default!;

    /// <summary>
    /// Name of the algorithm, see <see cref="OperationRateLimitingAlgorithms"/>.
    /// </summary>
    public string Algorithm { get; set; } = OperationRateLimitingAlgorithms.FixedWindow;

    public TimeSpan Duration { get; set; }

    public int MaxCount { get; set; }

    public int Segments { get; set; } = 10;

    public int TokensPerPeriod { get; set; } = 1;

    /// <summary>
    /// True to increment (or acquire a permit); false for a read-only pre-check.
    /// </summary>
    public bool Consume { get; set; }

    public DateTimeOffset Now { get; set; }

    /// <summary>
    /// Algorithm specific values (not used by the built-in algorithms).
    /// </summary>
    public Dictionary<string, string> Properties { get; set; } = new();
}
