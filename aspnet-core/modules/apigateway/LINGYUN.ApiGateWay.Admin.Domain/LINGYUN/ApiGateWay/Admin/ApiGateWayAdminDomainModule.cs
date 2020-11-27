using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateWay.Admin
{
    [DependsOn(
        typeof(ApiGateWayAdminDomainSharedModule),
        typeof(AbpDddDomainModule))]
    public class ApiGateWayAdminDomainModule : AbpModule
    {
    }
}
