using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work;

[DependsOn(
    typeof(AbpDddApplicationContractsModule))]
public class AbpWeChatWorkApplicationContractsModule : AbpModule
{

}
