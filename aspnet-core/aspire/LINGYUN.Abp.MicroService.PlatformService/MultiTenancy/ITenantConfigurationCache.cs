using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.PlatformService.MultiTenancy;

public interface ITenantConfigurationCache
{
    Task<List<TenantConfiguration>> GetTenantsAsync();
}
