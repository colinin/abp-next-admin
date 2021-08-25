using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule))]
    public class AbpWeChatApplicationContractsModule : AbpModule
    {
    }
}
