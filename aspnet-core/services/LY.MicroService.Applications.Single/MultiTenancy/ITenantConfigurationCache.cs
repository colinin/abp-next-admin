using Volo.Abp.MultiTenancy;

namespace LY.MicroService.Applications.Single.MultiTenancy;

public interface ITenantConfigurationCache
{
    Task RefreshAsync();

    Task<List<TenantConfiguration>> GetTenantsAsync();
}
