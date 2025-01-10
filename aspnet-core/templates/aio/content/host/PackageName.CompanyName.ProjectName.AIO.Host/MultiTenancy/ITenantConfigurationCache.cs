using Volo.Abp.MultiTenancy;

namespace PackageName.CompanyName.ProjectName.AIO.Host.MultiTenancy;

public interface ITenantConfigurationCache
{
    Task RefreshAsync();

    Task<List<TenantConfiguration>> GetTenantsAsync();
}
