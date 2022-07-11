using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(
        typeof(Volo.Abp.Account.AbpAccountApplicationContractsModule),
        typeof(AbpAccountDomainSharedModule))]
    public class AbpAccountApplicationContractsModule : AbpModule
    {
    }
}
