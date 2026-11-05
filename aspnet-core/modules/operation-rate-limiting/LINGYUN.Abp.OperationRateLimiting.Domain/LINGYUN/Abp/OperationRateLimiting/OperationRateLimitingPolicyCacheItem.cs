using System.Collections.Generic;
using Volo.Abp.Caching;

namespace LINGYUN.Abp.OperationRateLimiting;

[CacheName(AbpOperationRateLimitingConsts.PolicyCacheName)]
public class OperationRateLimitingPolicyCacheItem
{
    public List<OperationRateLimitingPolicy> Policies { get; set; } = new();
}
