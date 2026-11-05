using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default static store: creates the policies from <see cref="AbpOperationRateLimitingOptions"/>.
/// </summary>
public class OptionsOperationRateLimitingPolicyStore :
    IStaticOperationRateLimitingPolicyStore,
    ITransientDependency
{
    protected AbpOperationRateLimitingOptions Options { get; }

    public OptionsOperationRateLimitingPolicyStore(IOptions<AbpOperationRateLimitingOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<OperationRateLimitingPolicy>>(
            Options.Policies.Values.ToList());
    }

    public virtual Task<OperationRateLimitingPolicy?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Options.GetPolicyOrNull(name));
    }
}
