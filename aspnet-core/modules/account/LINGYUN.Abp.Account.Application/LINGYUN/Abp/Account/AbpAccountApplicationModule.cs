using LINGYUN.Abp.WeChat.Authorization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountDomainModule), 
        typeof(Volo.Abp.Account.AbpAccountApplicationModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpWeChatAuthorizationModule))]
    public class AbpAccountApplicationModule : AbpModule
    {

    }
}
