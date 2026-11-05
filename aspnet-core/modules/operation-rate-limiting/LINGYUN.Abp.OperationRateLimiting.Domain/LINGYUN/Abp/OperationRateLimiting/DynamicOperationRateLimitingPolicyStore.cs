using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Loads the dynamic policies from the database and caches them.
/// </summary>
/// <remarks>
/// The cache is invalidated explicitly with <see cref="InvalidateCacheAsync"/> (for example after
/// an administrative update) and expires automatically after a few minutes.
/// Because the cache key contains the current tenant, invalidation only affects the current tenant.
/// </remarks>
public class DynamicOperationRateLimitingPolicyStore :
    IDynamicOperationRateLimitingPolicyStore,
    ITransientDependency
{
    protected IOperationRateLimitingPolicyRecordRepository Repository { get; }

    protected IDistributedCache<OperationRateLimitingPolicyCacheItem> Cache { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public DynamicOperationRateLimitingPolicyStore(
        IOperationRateLimitingPolicyRecordRepository repository,
        IDistributedCache<OperationRateLimitingPolicyCacheItem> cache,
        ICurrentTenant currentTenant)
    {
        Repository = repository;
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    public virtual async Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(
        CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheKey();
        var cacheItem = await Cache.GetAsync(cacheKey, token: cancellationToken);

        if (cacheItem == null)
        {
            var records = await Repository.GetListAsync(
                includeDetails: true,
                cancellationToken: cancellationToken);

            cacheItem = new OperationRateLimitingPolicyCacheItem
            {
                Policies = records
                    .Select(record => record.ToPolicy())
                    .ToList()
            };

            await Cache.SetAsync(
                cacheKey,
                cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                token: cancellationToken);
        }

        return cacheItem.Policies;
    }

    public virtual async Task<OperationRateLimitingPolicy?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var policies = await GetPoliciesAsync(cancellationToken);

        return policies.FirstOrDefault(policy => string.Equals(policy.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public virtual async Task InvalidateCacheAsync(CancellationToken cancellationToken = default)
    {
        await Cache.RemoveAsync(GetCacheKey(), token: cancellationToken);
    }

    protected virtual string GetCacheKey()
    {
        return $"{CurrentTenant.Id?.ToString() ?? AbpOperationRateLimitingConsts.HostTenantName}:all";
    }
}
