using Volo.Abp.MultiTenancy;

namespace LY.AIO.Applications.Single.MultiTenancy;

public interface ITenantConfigurationCache
{
    Task RefreshAsync();

    Task<List<TenantConfiguration>> GetTenantsAsync();
}
