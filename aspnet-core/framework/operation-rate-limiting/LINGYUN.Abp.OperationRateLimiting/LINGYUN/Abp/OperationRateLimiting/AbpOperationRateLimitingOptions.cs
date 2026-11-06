using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Main options of the operation rate limiting module.
/// </summary>
public class AbpOperationRateLimitingOptions
{
    /// <summary>
    /// Global switch. When false, all checks pass through without touching any counter.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Timeout used when acquiring the distributed lock that guards a counter update.
    /// </summary>
    public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Statically defined policies, keyed by name (case-insensitive).
    /// </summary>
    public IDictionary<string, OperationRateLimitingPolicy> Policies { get; }

    /// <summary>
    /// Named custom partition key resolvers.
    /// </summary>
    public IDictionary<string, Func<OperationRateLimitingContext, Task<string?>>> PartitionKeyResolvers { get; }

    /// <summary>
    /// Registered algorithms, keyed by name (case-insensitive).
    /// </summary>
    public IDictionary<string, Type> Algorithms { get; }

    public AbpOperationRateLimitingOptions()
    {
        Policies = new Dictionary<string, OperationRateLimitingPolicy>(StringComparer.OrdinalIgnoreCase);
        PartitionKeyResolvers = new Dictionary<string, Func<OperationRateLimitingContext, Task<string?>>>(StringComparer.OrdinalIgnoreCase);
        Algorithms = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Adds a policy. Calling it again with the same name fully replaces the previous policy.
    /// </summary>
    public virtual void AddPolicy(string name, Action<OperationRateLimitingPolicyBuilder> configurePolicy)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(configurePolicy, nameof(configurePolicy));

        var policy = new OperationRateLimitingPolicy(name);
        var builder = new OperationRateLimitingPolicyBuilder(policy, this);

        configurePolicy(builder);
        builder.Build();

        Policies[name] = policy;
    }

    /// <summary>
    /// Adds a pre-built policy (for example loaded from a database or a configuration file).
    /// The builder validations are not executed.
    /// </summary>
    public virtual void AddPolicy(OperationRateLimitingPolicy policy)
    {
        Check.NotNull(policy, nameof(policy));
        Check.NotNullOrWhiteSpace(policy.Name, nameof(policy.Name));

        Policies[policy.Name] = policy;
    }

    /// <summary>
    /// Modifies an existing policy. The builder is pre-populated with the current rules.
    /// </summary>
    public virtual AbpOperationRateLimitingOptions ConfigurePolicy(
        string name,
        Action<OperationRateLimitingPolicyBuilder> configurePolicy)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(configurePolicy, nameof(configurePolicy));

        if (!Policies.TryGetValue(name, out var policy))
        {
            throw new AbpException($"Undefined operation rate limiting policy: {name}. Use AddPolicy before ConfigurePolicy.");
        }

        var clone = policy.Clone();
        var builder = new OperationRateLimitingPolicyBuilder(clone, this);

        configurePolicy(builder);
        builder.Build();

        Policies[name] = clone;

        return this;
    }

    /// <summary>
    /// Removes a policy if it exists.
    /// </summary>
    public virtual void RemovePolicy(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Policies.Remove(name);
    }

    public virtual OperationRateLimitingPolicy? GetPolicyOrNull(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        return Policies.TryGetValue(name, out var policy) ? policy : null;
    }

    public virtual OperationRateLimitingPolicy GetPolicy(string name)
    {
        return GetPolicyOrNull(name) ?? throw new AbpException($"Undefined operation rate limiting policy: {name}");
    }

    /// <summary>
    /// Registers a named custom partition key resolver.
    /// </summary>
    public virtual void AddPartitionKeyResolver(
        string name,
        Func<OperationRateLimitingContext, Task<string?>> resolver)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(resolver, nameof(resolver));

        if (PartitionKeyResolvers.ContainsKey(name))
        {
            throw new AbpException($"A partition key resolver named '{name}' is already registered. Use ReplacePartitionKeyResolver to replace it.");
        }

        PartitionKeyResolvers[name] = resolver;
    }

    /// <summary>
    /// Registers or replaces a named custom partition key resolver.
    /// </summary>
    public virtual void ReplacePartitionKeyResolver(
        string name,
        Func<OperationRateLimitingContext, Task<string?>> resolver)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(resolver, nameof(resolver));

        PartitionKeyResolvers[name] = resolver;
    }

    public virtual Func<OperationRateLimitingContext, Task<string?>>? GetPartitionKeyResolverOrNull(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        return PartitionKeyResolvers.TryGetValue(name, out var resolver) ? resolver : null;
    }

    /// <summary>
    /// Registers a custom algorithm. The name defaults to the type name.
    /// </summary>
    public virtual void AddAlgorithm<TAlgorithm>(string? name = null)
        where TAlgorithm : IOperationRateLimitingRule
    {
        AddAlgorithm(name ?? typeof(TAlgorithm).Name, typeof(TAlgorithm));
    }

    /// <summary>
    /// Registers a custom algorithm by type.
    /// </summary>
    public virtual void AddAlgorithm(string name, Type ruleType)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(ruleType, nameof(ruleType));

        if (!typeof(IOperationRateLimitingRule).IsAssignableFrom(ruleType))
        {
            throw new AbpException($"Type {ruleType.FullName} must implement {nameof(IOperationRateLimitingRule)}.");
        }

        Algorithms[name] = ruleType;
    }

    public virtual Type? GetAlgorithmType(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        return Algorithms.TryGetValue(name, out var ruleType) ? ruleType : null;
    }

    public virtual string? GetAlgorithmName(Type ruleType)
    {
        foreach (var algorithm in Algorithms)
        {
            if (algorithm.Value == ruleType)
            {
                return algorithm.Key;
            }
        }

        return null;
    }
}
