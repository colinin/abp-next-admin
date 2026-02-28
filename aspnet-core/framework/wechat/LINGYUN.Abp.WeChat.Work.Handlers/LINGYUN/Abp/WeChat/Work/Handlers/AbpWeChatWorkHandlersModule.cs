using LINGYUN.Abp.WeChat.Work.Common;
using LINGYUN.Abp.WeChat.Work.Contacts;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work.Handlers;

[DependsOn(typeof(AbpEventBusModule))]
[DependsOn(typeof(AbpWeChatWorkCommonModule))]
[DependsOn(typeof(AbpWeChatWorkContactModule))]
public class AbpWeChatWorkHandlersModule : AbpModule
{
}
