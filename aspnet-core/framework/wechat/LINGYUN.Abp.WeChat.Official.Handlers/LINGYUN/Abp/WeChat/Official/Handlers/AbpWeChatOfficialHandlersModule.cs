using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Official.Handlers;

[DependsOn(typeof(AbpEventBusModule))]
[DependsOn(typeof(AbpWeChatOfficialModule))]
public class AbpWeChatOfficialHandlersModule : AbpModule
{
}
