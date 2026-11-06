using System;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Configures one rule of a policy.
/// </summary>
/// <remarks>
/// <see cref="OperationRateLimitingRuleDefinition.Duration"/> and
/// <see cref="OperationRateLimitingRuleDefinition.MaxCount"/> are common properties.
/// Algorithm specific values are written with the extension methods of the corresponding
/// algorithm namespace (for example <c>SlidingWindow:Segments</c>).
/// The <c>PartitionBy...</c> methods return the owning <see cref="OperationRateLimitingPolicyBuilder"/>
/// so the fluent examples of the documentation keep working
/// (<c>policy.WithFixedWindow(...).PartitionByParameter().WithErrorCode(...)</c>).
/// </remarks>
public class OperationRateLimitingRuleBuilder
{
    protected OperationRateLimitingPolicyBuilder PolicyBuilder { get; }

    protected OperationRateLimitingRuleDefinition Definition { get; }

    public OperationRateLimitingRuleBuilder(
        OperationRateLimitingPolicyBuilder policyBuilder,
        OperationRateLimitingRuleDefinition definition)
    {
        PolicyBuilder = policyBuilder;
        Definition = definition;
    }

    /// <summary>
    /// Configures the rule as a fixed window rule.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithFixedWindow(TimeSpan duration, int maxCount)
    {
        Definition.Algorithm = OperationRateLimitingAlgorithms.FixedWindow;
        Definition.Duration = duration;
        Definition.MaxCount = maxCount;

        return this;
    }

    /// <summary>
    /// Configures the rule as a sliding window rule.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithSlidingWindow(TimeSpan duration, int maxCount, int segments = 10)
    {
        Definition.Algorithm = OperationRateLimitingAlgorithms.SlidingWindow;
        Definition.Duration = duration;
        Definition.MaxCount = maxCount;
        OperationRateLimitingRuleDefinitionSlidingWindowExtensions.SetSegments(Definition, segments);

        return this;
    }

    /// <summary>
    /// Configures the rule as a token bucket rule.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithTokenBucket(
        TimeSpan replenishmentPeriod,
        int tokenLimit,
        int tokensPerPeriod = 1)
    {
        Definition.Algorithm = OperationRateLimitingAlgorithms.TokenBucket;
        Definition.Duration = replenishmentPeriod;
        Definition.MaxCount = tokenLimit;
        OperationRateLimitingRuleDefinitionTokenBucketExtensions.SetTokensPerPeriod(Definition, tokensPerPeriod);

        return this;
    }

    /// <summary>
    /// Configures the rule as a concurrency rule. <paramref name="leaseDuration"/> is the time
    /// after which an acquired permit is released automatically.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithConcurrency(int permitLimit, TimeSpan leaseDuration)
    {
        Definition.Algorithm = OperationRateLimitingAlgorithms.Concurrency;
        Definition.MaxCount = permitLimit;
        Definition.Duration = leaseDuration;

        return this;
    }

    /// <summary>
    /// Uses a registered custom algorithm by name.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithAlgorithm(string algorithm)
    {
        Check.NotNullOrWhiteSpace(algorithm, nameof(algorithm));

        Definition.Algorithm = algorithm;

        return this;
    }

    /// <summary>
    /// Sets the common duration of the rule.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithDuration(TimeSpan duration)
    {
        Definition.Duration = duration;

        return this;
    }

    /// <summary>
    /// Sets the common max count of the rule.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithMaxCount(int maxCount)
    {
        Definition.MaxCount = maxCount;

        return this;
    }

    /// <summary>
    /// Sets a stable rule name. It is used as the counter key, so changing the rule parameters
    /// does not reset existing counters.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Definition.Name = name;

        return this;
    }

    /// <summary>
    /// Adds a custom property. The built-in algorithm specific values are written through the
    /// extension methods of the algorithm namespaces.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithProperty(string name, string value)
    {
        Definition.WithProperty(name, value);

        return this;
    }

    /// <summary>
    /// Includes the current tenant in the counter key.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithMultiTenancy(bool enabled = true)
    {
        Definition.IsMultiTenancyEnabled = enabled;

        return this;
    }

    /// <summary>
    /// Overrides the error code used when this rule denies a request.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithErrorCode(string errorCode)
    {
        Check.NotNullOrWhiteSpace(errorCode, nameof(errorCode));

        Definition.ErrorCode = errorCode;

        return this;
    }

    /// <summary>
    /// Uses <see cref="OperationRateLimitingContext.Parameter"/> as the partition key, as-is.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionByParameter()
    {
        return PartitionBy(OperationRateLimitingPartitionType.Parameter);
    }

    /// <summary>
    /// Uses the current authenticated user identifier as the partition key.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionByCurrentUser()
    {
        return PartitionBy(OperationRateLimitingPartitionType.CurrentUser);
    }

    /// <summary>
    /// Uses the current tenant identifier as the partition key ("host" when no tenant is active).
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionByCurrentTenant()
    {
        return PartitionBy(OperationRateLimitingPartitionType.CurrentTenant);
    }

    /// <summary>
    /// Uses the client IP address as the partition key. Requires an ASP.NET Core environment.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionByClientIp()
    {
        return PartitionBy(OperationRateLimitingPartitionType.ClientIp);
    }

    /// <summary>
    /// Uses an email address as the partition key. It is resolved from
    /// <see cref="OperationRateLimitingContext.Parameter"/> first, then from the current user,
    /// and normalized to upper case.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionByEmail()
    {
        return PartitionBy(OperationRateLimitingPartitionType.Email);
    }

    /// <summary>
    /// Uses a phone number as the partition key. It is resolved from
    /// <see cref="OperationRateLimitingContext.Parameter"/> first, then from the current user,
    /// and normalized by removing formatting characters.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionByPhoneNumber()
    {
        return PartitionBy(OperationRateLimitingPartitionType.PhoneNumber);
    }

    /// <summary>
    /// Uses a named custom partition key resolver.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionBy(string resolverName)
    {
        Check.NotNullOrWhiteSpace(resolverName, nameof(resolverName));

        if (PolicyBuilder.Options.GetPartitionKeyResolverOrNull(resolverName) == null)
        {
            throw new AbpException($"Undefined partition key resolver: {resolverName}. Register it with options.AddPartitionKeyResolver first.");
        }

        Definition.PartitionType = OperationRateLimitingPartitionType.Custom;
        Definition.PartitionResolverName = resolverName;

        return PolicyBuilder;
    }

    /// <summary>
    /// Registers (or replaces) a named custom partition key resolver and uses it.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder PartitionBy(
        string resolverName,
        Func<OperationRateLimitingContext, Task<string?>> resolver)
    {
        Check.NotNullOrWhiteSpace(resolverName, nameof(resolverName));
        Check.NotNull(resolver, nameof(resolver));

        PolicyBuilder.Options.ReplacePartitionKeyResolver(resolverName, resolver);

        Definition.PartitionType = OperationRateLimitingPartitionType.Custom;
        Definition.PartitionResolverName = resolverName;

        return PolicyBuilder;
    }

    protected virtual OperationRateLimitingPolicyBuilder PartitionBy(OperationRateLimitingPartitionType partitionType)
    {
        Definition.PartitionType = partitionType;
        Definition.PartitionResolverName = null;

        return PolicyBuilder;
    }
}
