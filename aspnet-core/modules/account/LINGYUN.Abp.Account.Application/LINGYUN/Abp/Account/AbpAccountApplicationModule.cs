using LINGYUN.Abp.Identity;
using LINGYUN.Abp.WeChat.Authorization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(
        typeof(Volo.Abp.Account.AbpAccountApplicationModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpWeChatAuthorizationModule))]
    public class AbpAccountApplicationModule : AbpModule
    {

    }
}
