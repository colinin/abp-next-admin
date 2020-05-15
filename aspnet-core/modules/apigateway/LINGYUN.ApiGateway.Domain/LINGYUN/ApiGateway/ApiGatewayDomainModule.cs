using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateway
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class ApiGatewayDomainModule : AbpModule
    {
    }
}
