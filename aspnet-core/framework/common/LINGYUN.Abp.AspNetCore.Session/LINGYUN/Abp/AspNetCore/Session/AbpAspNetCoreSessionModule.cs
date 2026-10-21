using LINGYUN.Abp.Identity.Session;
using LINGYUN.Abp.IP.Location;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Session;

[DependsOn(
    typeof(AbpIdentitySessionModule),
    typeof(AbpIPLocationModule),
    typeof(AbpAspNetCoreModule))]
public class AbpAspNetCoreSessionModule : AbpModule
{

}
