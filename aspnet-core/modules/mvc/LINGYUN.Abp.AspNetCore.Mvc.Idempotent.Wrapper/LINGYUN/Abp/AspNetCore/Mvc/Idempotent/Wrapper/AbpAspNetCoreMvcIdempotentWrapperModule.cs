using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper;

[DependsOn(
    typeof(AbpAspNetCoreMvcIdempotentModule),
    typeof(AbpAspNetCoreMvcWrapperModule))]
public class AbpAspNetCoreMvcIdempotentWrapperModule : AbpModule
{

}
