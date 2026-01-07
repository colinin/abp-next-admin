using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.PlatformService.MultiTenancy;

[IgnoreMultiTenancy]
public class TenantConfigurationCacheItem
{
    public List<TenantConfiguration> Tenants { get; set; }

    public TenantConfigurationCacheItem()
    {
        Tenants = new List<TenantConfiguration>();
    }

    public TenantConfigurationCacheItem(List<TenantConfiguration> tenants)
    {
        Tenants = tenants;
    }
}
