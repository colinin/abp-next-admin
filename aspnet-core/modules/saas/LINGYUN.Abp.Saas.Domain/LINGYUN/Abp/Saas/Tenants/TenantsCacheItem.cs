using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas.Tenants;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("AbpSaasTenantsAll")]
public class TenantsCacheItem
{
    private const string CacheKeyFormat = "id:{0}";

    public List<TenantConfiguration> Tenants { get; set; }
    public TenantsCacheItem()
    {

    }
    public TenantsCacheItem(List<TenantConfiguration> tenants)
    {
        Tenants = tenants;
    }
    public static string CalculateCacheKey(bool includeDetails = false)
    {
        return string.Format(CacheKeyFormat, includeDetails.ToString());
    }
}
