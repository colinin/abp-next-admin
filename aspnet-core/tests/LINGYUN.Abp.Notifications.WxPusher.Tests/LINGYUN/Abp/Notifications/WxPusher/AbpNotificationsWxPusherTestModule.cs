using LINGYUN.Abp.WxPusher.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WxPusher;

[DependsOn(
    typeof(AbpNotificationsWxPusherModule),
    typeof(AbpNotificationsTestsModule))]
public class AbpNotificationsWxPusherTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 用捕获式假实现替换真实发送, 避免真正调用 WxPusher 接口
        context.Services.Replace(
            ServiceDescriptor.Singleton<IWxPusherMessageSender, FakeWxPusherMessageSender>());
    }
}
