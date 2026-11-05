using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingChecker : IOperationRateLimitingChecker, ITransientDependency
{
    protected IOperationRateLimitingPolicyProvider PolicyProvider { get; }

    protected IOperationRateLimitingStore Store { get; }

    protected IOperationRateLimitingPartitionKeyResolver PartitionKeyResolver { get; }

    protected IOperationRateLimitingFormatter Formatter { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IClock Clock { get; }

    protected IOptions<AbpOperationRateLimitingOptions> Options { get; }

    protected IServiceProvider ServiceProvider { get; }

    public OperationRateLimitingChecker(
        IOperationRateLimitingPolicyProvider policyProvider,
        IOperationRateLimitingStore store,
        IOperationRateLimitingPartitionKeyResolver partitionKeyResolver,
        IOperationRateLimitingFormatter formatter,
        ICurrentTenant currentTenant,
        IClock clock,
        IOptions<AbpOperationRateLimitingOptions> options,
        IServiceProvider serviceProvider)
    {
        PolicyProvider = policyProvider;
        Store = store;
        PartitionKeyResolver = partitionKeyResolver;
        Formatter = formatter;
        CurrentTenant = currentTenant;
        Clock = clock;
        Options = options;
        ServiceProvider = serviceProvider;
    }

    public virtual async Task<OperationRateLimitingStatus> CheckAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        var policy = await GetEnabledPolicyOrNullAsync(policyName, cancellationToken);
        if (policy == null)
        {
            return OperationRateLimitingStatus.Disabled(policyName);
        }

        context ??= new OperationRateLimitingContext();
        var now = Clock.Now;

        // A single rule does not need the pre-check phase: the consume call already returns
        // the denial details and never increments a counter that is already exhausted.
        if (policy.Rules.Count == 1)
        {
            var singleStatuses = await EvaluateRulesAsync(policy, context, consume: true, now, cancellationToken);
            ThrowIfDenied(policy, policyName, singleStatuses, context);

            return BuildStatus(policyName, singleStatuses);
        }

        // Phase 1: pre-check every rule without incrementing, so a request that is already
        // blocked does not consume quota of the other rules.
        var statuses = await EvaluateRulesAsync(policy, context, consume: false, now, cancellationToken);
        ThrowIfDenied(policy, policyName, statuses, context);

        // Phase 2: increment every counter. This phase is not an atomic transaction across
        // all counters: a concurrent request may exhaust a later rule, in which case the
        // counters incremented by the previous rules remain consumed.
        for (var index = 0; index < policy.Rules.Count; index++)
        {
            statuses[index] = await EvaluateRuleAsync(
                policy,
                policy.Rules[index],
                context,
                consume: true,
                now,
                cancellationToken);

            ThrowIfDenied(policy, policyName, statuses, context);
        }

        return BuildStatus(policyName, statuses);
    }

    public virtual async Task<bool> IsAllowedAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        var status = await GetStatusAsync(policyName, context, cancellationToken);

        return status.IsAllowed;
    }

    public virtual async Task<OperationRateLimitingStatus> GetStatusAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        var policy = await GetEnabledPolicyOrNullAsync(policyName, cancellationToken);
        if (policy == null)
        {
            return OperationRateLimitingStatus.Disabled(policyName);
        }

        context ??= new OperationRateLimitingContext();
        var now = Clock.Now;

        var statuses = await EvaluateRulesAsync(policy, context, consume: false, now, cancellationToken);

        return BuildStatus(policyName, statuses);
    }

    public virtual async Task ResetAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        if (!Options.Value.IsEnabled)
        {
            return;
        }

        var policy = await GetPolicyAsync(policyName, cancellationToken);
        context ??= new OperationRateLimitingContext();

        foreach (var rule in policy.Rules)
        {
            var key = await BuildKeyAsync(policy, rule, context, cancellationToken);
            await Store.ResetAsync(key, cancellationToken);
        }
    }

    public virtual async Task ReleaseAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        if (!Options.Value.IsEnabled)
        {
            return;
        }

        var policy = await GetPolicyAsync(policyName, cancellationToken);
        context ??= new OperationRateLimitingContext();

        foreach (var rule in policy.Rules)
        {
            var executor = ResolveRule(rule);
            if (executor is not IOperationRateLimitingRuleReleaser releaser)
            {
                continue;
            }

            var key = await BuildKeyAsync(policy, rule, context, cancellationToken);
            await releaser.ReleaseAsync(new OperationRateLimitingRuleContext(
                policy,
                rule,
                key,
                consume: false,
                Clock.Now,
                Store,
                context,
                ServiceProvider,
                cancellationToken));
        }
    }

    public virtual async Task<IOperationRateLimitingLease> AcquireAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        context ??= new OperationRateLimitingContext();

        await CheckAsync(policyName, context, cancellationToken);

        return new OperationRateLimitingLease(this, policyName, context);
    }

    protected virtual async Task<OperationRateLimitingPolicy?> GetEnabledPolicyOrNullAsync(
        string policyName,
        CancellationToken cancellationToken)
    {
        if (!Options.Value.IsEnabled)
        {
            return null;
        }

        var policy = await GetPolicyAsync(policyName, cancellationToken);

        return policy.IsEnabled ? policy : null;
    }

    protected virtual async Task<OperationRateLimitingPolicy> GetPolicyAsync(
        string policyName,
        CancellationToken cancellationToken)
    {
        Check.NotNullOrWhiteSpace(policyName, nameof(policyName));

        return await PolicyProvider.GetOrNullAsync(policyName, cancellationToken)
            ?? throw new AbpException($"Undefined operation rate limiting policy: {policyName}");
    }

    protected virtual async Task<OperationRateLimitingStoreResult> ExecuteRuleAsync(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingRuleDefinition rule,
        OperationRateLimitingContext context,
        bool consume,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var executor = ResolveRule(rule);
        var key = await BuildKeyAsync(policy, rule, context, cancellationToken);

        return await executor.ExecuteAsync(new OperationRateLimitingRuleContext(
            policy,
            rule,
            key,
            consume,
            now,
            Store,
            context,
            ServiceProvider,
            cancellationToken));
    }

    protected virtual async Task<OperationRateLimitingRuleStatus> EvaluateRuleAsync(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingRuleDefinition rule,
        OperationRateLimitingContext context,
        bool consume,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var result = await ExecuteRuleAsync(policy, rule, context, consume, now, cancellationToken);

        return CreateRuleStatus(rule, result);
    }

    protected virtual async Task<List<OperationRateLimitingRuleStatus>> EvaluateRulesAsync(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingContext context,
        bool consume,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var statuses = new List<OperationRateLimitingRuleStatus>(policy.Rules.Count);

        foreach (var rule in policy.Rules)
        {
            statuses.Add(await EvaluateRuleAsync(policy, rule, context, consume, now, cancellationToken));
        }

        return statuses;
    }

    protected virtual void ThrowIfDenied(
        OperationRateLimitingPolicy policy,
        string policyName,
        List<OperationRateLimitingRuleStatus> statuses,
        OperationRateLimitingContext context)
    {
        if (statuses.All(status => status.IsAllowed))
        {
            return;
        }

        throw CreateException(policy, BuildStatus(policyName, statuses), context);
    }

    protected virtual IOperationRateLimitingRule ResolveRule(OperationRateLimitingRuleDefinition rule)
    {
        var ruleType = rule.RuleType ?? Options.Value.GetAlgorithmType(rule.Algorithm);

        if (ruleType == null)
        {
            throw new AbpException($"Undefined operation rate limiting algorithm: {rule.Algorithm}");
        }

        var ruleService = ServiceProvider.GetService(ruleType);

        if (ruleService is not IOperationRateLimitingRule ruleExecutor)
        {
            throw new AbpException(
                $"The operation rate limiting rule type '{ruleType.FullName}' is not registered as a service implementing {nameof(IOperationRateLimitingRule)}. " +
                "Implement ITransientDependency on the rule type or register it explicitly.");
        }

        return ruleExecutor;
    }

    protected virtual async Task<string> BuildKeyAsync(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingRuleDefinition rule,
        OperationRateLimitingContext context,
        CancellationToken cancellationToken)
    {
        var partitionKey = await PartitionKeyResolver.ResolveAsync(rule, context, cancellationToken);
        var ruleKey = rule.GetRuleKeyDescriptor();
        var tenant = rule.IsMultiTenancyEnabled ? CurrentTenant.Id : null;

        return tenant.HasValue
            ? $"{AbpOperationRateLimitingConsts.CacheKeyPrefix}:t:{tenant}:{policy.Name}:{ruleKey}:{partitionKey}"
            : $"{AbpOperationRateLimitingConsts.CacheKeyPrefix}:{policy.Name}:{ruleKey}:{partitionKey}";
    }

    protected virtual OperationRateLimitingRuleStatus CreateRuleStatus(
        OperationRateLimitingRuleDefinition rule,
        OperationRateLimitingStoreResult result)
    {
        return new OperationRateLimitingRuleStatus
        {
            RuleName = rule.Name ?? rule.GetRuleKeyDescriptor(),
            IsAllowed = result.IsAllowed,
            MaxCount = result.MaxCount,
            CurrentCount = result.CurrentCount,
            RemainingCount = result.RemainingCount,
            RetryAfter = result.RetryAfter,
            WindowDuration = result.WindowDuration,
            IsPermanent = result.IsPermanent,
            ErrorCode = rule.ErrorCode
        };
    }

    protected virtual OperationRateLimitingStatus BuildStatus(
        string policyName,
        List<OperationRateLimitingRuleStatus> rules)
    {
        var isAllowed = rules.All(rule => rule.IsAllowed);
        var bindingRule = rules
            .OrderBy(rule => rule.RemainingCount)
            .ThenByDescending(rule => rule.RetryAfter ?? TimeSpan.Zero)
            .FirstOrDefault();

        TimeSpan? retryAfter;

        if (isAllowed)
        {
            retryAfter = bindingRule?.RetryAfter;
        }
        else
        {
            var deniedRetries = rules
                .Where(rule => !rule.IsAllowed && rule.RetryAfter.HasValue)
                .Select(rule => rule.RetryAfter!.Value)
                .ToList();

            retryAfter = deniedRetries.Count > 0 ? deniedRetries.Max() : null;
        }

        return new OperationRateLimitingStatus
        {
            PolicyName = policyName,
            IsAllowed = isAllowed,
            MaxCount = bindingRule?.MaxCount ?? 0,
            CurrentCount = bindingRule?.CurrentCount ?? 0,
            RemainingCount = rules.Count == 0 ? 0 : rules.Min(rule => rule.RemainingCount),
            RetryAfter = retryAfter,
            WindowDuration = bindingRule?.WindowDuration ?? TimeSpan.Zero,
            Rules = rules
        };
    }

    protected virtual AbpOperationRateLimitingException CreateException(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingStatus status,
        OperationRateLimitingContext context)
    {
        var ruleErrorCode = status.Rules
            .FirstOrDefault(rule => !rule.IsAllowed && !string.IsNullOrWhiteSpace(rule.ErrorCode))
            ?.ErrorCode;

        return new AbpOperationRateLimitingException(
            policy.Name,
            status,
            Formatter,
            context,
            ruleErrorCode ?? policy.ErrorCode);
    }
}
