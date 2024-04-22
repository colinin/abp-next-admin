using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Official;

[DependsOn(
    typeof(AbpWeChatOfficialApplicationContractsModule),
    typeof(AbpWeChatOfficialModule),
    typeof(AbpDddApplicationModule))]
public class AbpWeChatOfficialApplicationModule : AbpModule
{

}
