using LINGYUN.Abp.Wrapper;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OpenApi.Authorization
{
    [DependsOn(
        typeof(AbpWrapperModule),
        typeof(AbpTimingModule),
        typeof(AbpOpenApiModule),
        typeof(AbpAspNetCoreModule))]
    public class AbpOpenApiAuthorizationModule : AbpModule
    {
    }
}
