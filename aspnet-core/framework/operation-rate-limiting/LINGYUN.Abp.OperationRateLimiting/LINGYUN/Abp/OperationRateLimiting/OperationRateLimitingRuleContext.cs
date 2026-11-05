using System;
using System.Threading;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Everything a rule needs to check one partition of one policy.
/// </summary>
public class OperationRateLimitingRuleContext
{
    public OperationRateLimitingPolicy Policy { get; }

    public OperationRateLimitingRuleDefinition Rule { get; }

    /// <summary>
    /// Fully qualified store key (policy + rule + partition + tenant).
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// True when the counter must be incremented (or a permit acquired).
    /// False for read-only pre-checks.
    /// </summary>
    public bool Consume { get; }

    public DateTimeOffset Now { get; }

    public IOperationRateLimitingStore Store { get; }

    public OperationRateLimitingContext RateLimitingContext { get; }

    public IServiceProvider ServiceProvider { get; }

    public CancellationToken CancellationToken { get; }

    public OperationRateLimitingRuleContext(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingRuleDefinition rule,
        string key,
        bool consume,
        DateTimeOffset now,
        IOperationRateLimitingStore store,
        OperationRateLimitingContext rateLimitingContext,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        Policy = policy;
        Rule = rule;
        Key = key;
        Consume = consume;
        Now = now;
        Store = store;
        RateLimitingContext = rateLimitingContext;
        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
    }
}
