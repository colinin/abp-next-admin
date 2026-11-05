using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using LINGYUN.Abp.Dynamic.Definitions;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Merges the statically defined policies with the dynamic ones, using the same
/// <c>DynamicDefinitionStrategy</c> mechanism as the dynamic definition module.
/// </summary>
public class OperationRateLimitingPolicyManager :
    DynamicDefinitionManager<OperationRateLimitingPolicy>,
    IOperationRateLimitingPolicyManager,
    ITransientDependency
{
    protected IStaticOperationRateLimitingPolicyStore StaticStore { get; }

    protected IDynamicOperationRateLimitingPolicyStore DynamicStore { get; }

    public OperationRateLimitingPolicyManager(
        IOptions<AbpDynamicDefinitionsOptions> dynamicDefinitionsOptions,
        IStaticOperationRateLimitingPolicyStore staticStore,
        IDynamicOperationRateLimitingPolicyStore dynamicStore)
        : base(dynamicDefinitionsOptions)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public virtual async Task<OperationRateLimitingPolicy?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var staticPolicy = await StaticStore.GetOrNullAsync(name, cancellationToken);
        var dynamicPolicy = await DynamicStore.GetOrNullAsync(name, cancellationToken);

        // The merge mutates the target definition, never mutate a definition owned by a store.
        return await GetDefinitionAsync(staticPolicy?.Clone(), dynamicPolicy);
    }

    public virtual async Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(
        CancellationToken cancellationToken = default)
    {
        var staticPolicies = (await StaticStore.GetPoliciesAsync(cancellationToken))
            .Select(policy => policy.Clone())
            .ToList();

        var dynamicPolicies = await DynamicStore.GetPoliciesAsync(cancellationToken);

        return await GetDefinitionsAsync(staticPolicies, dynamicPolicies);
    }

    protected override string GetDefinitionKey(OperationRateLimitingPolicy definition)
    {
        return definition.Name;
    }

    protected override Task<OperationRateLimitingPolicy> MergeDefinitionAsync(
        OperationRateLimitingPolicy targetDefinition,
        OperationRateLimitingPolicy sourceDefinition)
    {
        targetDefinition.DisplayName = sourceDefinition.DisplayName ?? targetDefinition.DisplayName;
        targetDefinition.ErrorCode = sourceDefinition.ErrorCode ?? targetDefinition.ErrorCode;
        targetDefinition.IsEnabled = sourceDefinition.IsEnabled;

        if (sourceDefinition.Rules.Count > 0)
        {
            targetDefinition.Rules = sourceDefinition.Rules
                .Select(rule => rule.Clone())
                .ToList();
        }

        return Task.FromResult(targetDefinition);
    }
}
