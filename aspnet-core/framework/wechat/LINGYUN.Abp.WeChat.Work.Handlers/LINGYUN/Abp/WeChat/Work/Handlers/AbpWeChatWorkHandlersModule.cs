using LINGYUN.Abp.WeChat.Work.Common;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work.Handlers;

[DependsOn(typeof(AbpEventBusModule))]
[DependsOn(typeof(AbpWeChatWorkCommonModule))]
public class AbpWeChatWorkHandlersModule : AbpModule
{
}
