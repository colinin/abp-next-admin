using Volo.Abp.Modularity;
using VoloAbpAspNetCoreMultiTenancyModule = Volo.Abp.AspNetCore.MultiTenancy.AbpAspNetCoreMultiTenancyModule;

namespace LINGYUN.Abp.AspNetCore.MultiTenancy;

[DependsOn(typeof(VoloAbpAspNetCoreMultiTenancyModule))]
public class AbpAspNetCoreMultiTenancyModule : AbpModule
{

}
