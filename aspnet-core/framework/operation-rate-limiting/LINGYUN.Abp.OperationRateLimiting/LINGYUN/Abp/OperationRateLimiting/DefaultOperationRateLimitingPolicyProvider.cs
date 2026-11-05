using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

public class DefaultOperationRateLimitingPolicyProvider :
    IOperationRateLimitingPolicyProvider,
    ITransientDependency
{
    protected IOperationRateLimitingPolicyManager PolicyManager { get; }

    public DefaultOperationRateLimitingPolicyProvider(IOperationRateLimitingPolicyManager policyManager)
    {
        PolicyManager = policyManager;
    }

    public virtual Task<OperationRateLimitingPolicy?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return PolicyManager.GetOrNullAsync(name, cancellationToken);
    }

    public virtual Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(
        CancellationToken cancellationToken = default)
    {
        return PolicyManager.GetPoliciesAsync(cancellationToken);
    }
}
