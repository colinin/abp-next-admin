using LINGYUN.Abp.WeChat;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.WeChat
{
    [DependsOn(
        typeof(AbpWeChatModule),
        typeof(AbpIdentityDomainModule))]
    public class AbpIdentityWeChatModule : AbpModule
    {
    }
}
