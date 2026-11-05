using System;
using System.Collections.Generic;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Configures a policy.
/// </summary>
public class OperationRateLimitingPolicyBuilder
{
    protected OperationRateLimitingPolicy Policy { get; }

    internal AbpOperationRateLimitingOptions Options { get; }

    public OperationRateLimitingPolicyBuilder(
        OperationRateLimitingPolicy policy,
        AbpOperationRateLimitingOptions options)
    {
        Policy = policy;
        Options = options;
    }

    /// <summary>
    /// Adds a fixed window rule and returns its builder.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithFixedWindow(TimeSpan duration, int maxCount)
    {
        return AddRule(rule => rule.WithFixedWindow(duration, maxCount));
    }

    /// <summary>
    /// Adds a sliding window rule and returns its builder.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithSlidingWindow(TimeSpan duration, int maxCount, int segments = 10)
    {
        return AddRule(rule => rule.WithSlidingWindow(duration, maxCount, segments));
    }

    /// <summary>
    /// Adds a token bucket rule and returns its builder.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithTokenBucket(
        TimeSpan replenishmentPeriod,
        int tokenLimit,
        int tokensPerPeriod = 1)
    {
        return AddRule(rule => rule.WithTokenBucket(replenishmentPeriod, tokenLimit, tokensPerPeriod));
    }

    /// <summary>
    /// Adds a concurrency rule and returns its builder.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder WithConcurrency(int permitLimit, TimeSpan leaseDuration)
    {
        return AddRule(rule => rule.WithConcurrency(permitLimit, leaseDuration));
    }

    /// <summary>
    /// Adds a rule and configures it.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder AddRule(Action<OperationRateLimitingRuleBuilder> configureRule)
    {
        Check.NotNull(configureRule, nameof(configureRule));

        var definition = new OperationRateLimitingRuleDefinition();
        Policy.Rules.Add(definition);

        var builder = new OperationRateLimitingRuleBuilder(this, definition);
        configureRule(builder);

        return builder;
    }

    /// <summary>
    /// Adds a rule implemented by a custom <see cref="IOperationRateLimitingRule"/>.
    /// </summary>
    public virtual OperationRateLimitingRuleBuilder AddRule<TRule>(
        Action<OperationRateLimitingRuleBuilder>? configureRule = null)
        where TRule : IOperationRateLimitingRule
    {
        var algorithmName = Options.GetAlgorithmName(typeof(TRule)) ?? typeof(TRule).Name;
        Options.AddAlgorithm(algorithmName, typeof(TRule));

        var definition = new OperationRateLimitingRuleDefinition
        {
            Algorithm = algorithmName,
            RuleType = typeof(TRule)
        };
        Policy.Rules.Add(definition);

        var builder = new OperationRateLimitingRuleBuilder(this, definition);
        configureRule?.Invoke(builder);

        return builder;
    }

    /// <summary>
    /// Removes all rules.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder ClearRules()
    {
        Policy.Rules.Clear();

        return this;
    }

    /// <summary>
    /// Sets the error code used when the policy denies a request.
    /// </summary>
    public virtual OperationRateLimitingPolicyBuilder WithErrorCode(string errorCode)
    {
        Check.NotNullOrWhiteSpace(errorCode, nameof(errorCode));

        Policy.ErrorCode = errorCode;

        return this;
    }

    public virtual OperationRateLimitingPolicyBuilder WithDisplayName(string displayName)
    {
        Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

        Policy.DisplayName = displayName;

        return this;
    }

    public virtual OperationRateLimitingPolicyBuilder WithEnabled(bool enabled = true)
    {
        Policy.IsEnabled = enabled;

        return this;
    }

    /// <summary>
    /// Validates the policy and returns it.
    /// </summary>
    public virtual OperationRateLimitingPolicy Build()
    {
        if (string.IsNullOrWhiteSpace(Policy.Name))
        {
            throw new AbpException("The name of an operation rate limiting policy cannot be empty.");
        }

        if (Policy.Rules.Count == 0)
        {
            throw new AbpException($"Operation rate limiting policy '{Policy.Name}' must contain at least one rule.");
        }

        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rule in Policy.Rules)
        {
            ValidateRule(rule);

            if (!string.IsNullOrWhiteSpace(rule.Name))
            {
                if (!names.Add(rule.Name!))
                {
                    throw new AbpException($"Duplicate rule name '{rule.Name}' in operation rate limiting policy '{Policy.Name}'.");
                }
            }
            else
            {
                if (!keys.Add(rule.GetRuleKeyDescriptor()))
                {
                    throw new AbpException($"Duplicate rule definition in operation rate limiting policy '{Policy.Name}'.");
                }
            }
        }

        return Policy;
    }

    protected virtual void ValidateRule(OperationRateLimitingRuleDefinition rule)
    {
        if (rule.Duration <= TimeSpan.Zero)
        {
            throw new AbpException($"The duration of a rule in operation rate limiting policy '{Policy.Name}' must be greater than zero.");
        }

        if (rule.MaxCount < 0)
        {
            throw new AbpException($"The maximum count of a rule in operation rate limiting policy '{Policy.Name}' cannot be negative.");
        }

        if (string.Equals(rule.Algorithm, OperationRateLimitingAlgorithms.SlidingWindow, StringComparison.OrdinalIgnoreCase))
        {
            OperationRateLimitingRuleDefinitionSlidingWindowExtensions.Validate(rule, Policy.Name);
        }
        else if (string.Equals(rule.Algorithm, OperationRateLimitingAlgorithms.TokenBucket, StringComparison.OrdinalIgnoreCase))
        {
            OperationRateLimitingRuleDefinitionTokenBucketExtensions.Validate(rule, Policy.Name);
        }

        if (rule.RuleType == null && Options.GetAlgorithmType(rule.Algorithm) == null)
        {
            throw new AbpException($"Undefined operation rate limiting algorithm: {rule.Algorithm}. Register it with options.AddAlgorithm first.");
        }

        if (rule.PartitionType == OperationRateLimitingPartitionType.Custom)
        {
            if (string.IsNullOrWhiteSpace(rule.PartitionResolverName))
            {
                throw new AbpException($"A custom partition of a rule in operation rate limiting policy '{Policy.Name}' requires a resolver name.");
            }

            if (Options.GetPartitionKeyResolverOrNull(rule.PartitionResolverName!) == null)
            {
                throw new AbpException($"Undefined partition key resolver: {rule.PartitionResolverName}. Register it with options.AddPartitionKeyResolver first.");
            }
        }
    }
}
