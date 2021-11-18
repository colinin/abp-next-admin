using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdentityServer
{
    [DependsOn(typeof(Volo.Abp.IdentityServer.AbpIdentityServerDomainModule))]
    public class AbpIdentityServerDomainModule : AbpModule
    {
    }
}
