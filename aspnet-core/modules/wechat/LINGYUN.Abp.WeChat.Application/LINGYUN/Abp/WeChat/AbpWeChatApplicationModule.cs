using LINGYUN.Abp.WeChat.MiniProgram;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat
{
    [DependsOn(
        typeof(AbpWeChatMiniProgramModule),
        typeof(AbpWeChatApplicationContractsModule),
        typeof(AbpDddApplicationModule))]
    public class AbpWeChatApplicationModule : AbpModule
    {
    }
}
