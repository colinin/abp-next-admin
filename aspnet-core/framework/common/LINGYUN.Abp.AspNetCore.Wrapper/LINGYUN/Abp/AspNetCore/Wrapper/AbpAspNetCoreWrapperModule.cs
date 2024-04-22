using LINGYUN.Abp.Wrapper;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Wrapper;

[DependsOn(
    typeof(AbpWrapperModule),
    typeof(AbpAspNetCoreModule))]
public class AbpAspNetCoreWrapperModule : AbpModule
{

}
