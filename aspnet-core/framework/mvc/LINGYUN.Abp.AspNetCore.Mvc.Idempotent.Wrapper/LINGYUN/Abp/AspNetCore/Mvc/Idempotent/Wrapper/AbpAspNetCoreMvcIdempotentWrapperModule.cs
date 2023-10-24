using LINGYUN.Abp.AspNetCore.Wrapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper;

[DependsOn(
    typeof(AbpAspNetCoreWrapperModule),
    typeof(AbpAspNetCoreMvcIdempotentModule))]
public class AbpAspNetCoreMvcIdempotentWrapperModule : AbpModule
{

}
