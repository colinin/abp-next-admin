using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkApplicationContractsModule),
    typeof(AbpWeChatWorkModule),
    typeof(AbpUiNavigationModule),
    typeof(AbpDddApplicationModule))]
public class AbpWeChatWorkApplicationModule : AbpModule
{

}
