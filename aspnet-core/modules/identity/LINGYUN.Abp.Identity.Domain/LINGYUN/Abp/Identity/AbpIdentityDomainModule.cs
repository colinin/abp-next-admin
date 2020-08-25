using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity
{
    [DependsOn(typeof(Volo.Abp.Identity.AbpIdentityDomainModule))]
    public class AbpIdentityDomainModule : AbpModule
    {
    }
}
