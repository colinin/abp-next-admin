using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdentityServer.EntityFrameworkCore;

[DependsOn(typeof(LINGYUN.Abp.IdentityServer.AbpIdentityServerDomainModule))]
[DependsOn(typeof(Volo.Abp.IdentityServer.EntityFrameworkCore.AbpIdentityServerEntityFrameworkCoreModule))]
public class AbpIdentityServerEntityFrameworkCoreModule : AbpModule
{
}
