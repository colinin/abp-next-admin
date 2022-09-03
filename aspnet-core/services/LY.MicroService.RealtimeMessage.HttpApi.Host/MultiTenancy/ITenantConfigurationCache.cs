using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.RealtimeMessage.MultiTenancy;

public interface ITenantConfigurationCache
{
    Task<List<TenantConfiguration>> GetTenantsAsync();
}
