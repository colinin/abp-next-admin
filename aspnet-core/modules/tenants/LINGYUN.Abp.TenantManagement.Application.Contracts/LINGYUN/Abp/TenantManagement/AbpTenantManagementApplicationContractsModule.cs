using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpTenantManagementDomainSharedModule))]
    public class AbpTenantManagementApplicationContractsModule : AbpModule
    {

    }
}