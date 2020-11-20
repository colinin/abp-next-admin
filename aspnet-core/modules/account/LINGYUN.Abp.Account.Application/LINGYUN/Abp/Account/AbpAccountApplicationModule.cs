using LINGYUN.Abp.Identity;
using LINGYUN.Abp.WeChat.MiniProgram;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(
        typeof(Volo.Abp.Account.AbpAccountApplicationModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpWeChatMiniProgramModule))]
    public class AbpAccountApplicationModule : AbpModule
    {

    }
}
