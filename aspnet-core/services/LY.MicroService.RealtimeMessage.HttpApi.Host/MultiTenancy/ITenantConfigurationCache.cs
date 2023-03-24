using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.RealtimeMessage.MultiTenancy;

public interface ITenantConfigurationCache
{
    Task RefreshAsync();

    Task<List<TenantConfiguration>> GetTenantsAsync();
}
