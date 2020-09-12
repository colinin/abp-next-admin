using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(typeof(AbpAccountDomainSharedModule))]
    public class AbpAccountDomainModule : AbpModule
    {
    }
}
