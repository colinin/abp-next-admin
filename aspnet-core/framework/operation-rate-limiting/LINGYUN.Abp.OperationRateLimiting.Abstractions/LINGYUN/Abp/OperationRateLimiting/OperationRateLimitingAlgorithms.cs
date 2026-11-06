namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Names of the rate limiting algorithms built into the module.
/// </summary>
/// <remarks>
/// An algorithm name is stored in <see cref="OperationRateLimitingRuleDefinition.Algorithm"/>,
/// so it can be persisted (database, configuration) and re-bound to an implementation at runtime.
/// Use <see cref="AbpOperationRateLimitingOptions.AddAlgorithm{TAlgorithm}"/> to add your own.
/// </remarks>
public static class OperationRateLimitingAlgorithms
{
    /// <summary>
    /// Allows <c>maxCount</c> requests per fixed time window.
    /// </summary>
    public const string FixedWindow = "FixedWindow";

    /// <summary>
    /// Allows <c>maxCount</c> requests over a sliding time window.
    /// </summary>
    public const string SlidingWindow = "SlidingWindow";

    /// <summary>
    /// Allows bursts up to <c>maxCount</c> tokens, replenishing <c>tokensPerPeriod</c> tokens per period.
    /// </summary>
    public const string TokenBucket = "TokenBucket";

    /// <summary>
    /// Allows at most <c>maxCount</c> concurrent (in-flight) operations.
    /// </summary>
    public const string Concurrency = "Concurrency";
}
