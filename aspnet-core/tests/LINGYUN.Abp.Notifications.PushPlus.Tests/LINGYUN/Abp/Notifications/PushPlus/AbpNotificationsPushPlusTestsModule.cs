using LINGYUN.Abp.PushPlus.Message;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.PushPlus;

[DependsOn(
    typeof(AbpNotificationsPushPlusModule),
    typeof(AbpNotificationsTestsModule),
    typeof(AbpTestsBaseModule))]
public class AbpNotificationsPushPlusTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 用捕获式假实现替换真实发送, 避免真正调用 PushPlus 接口
        context.Services.Replace(
            ServiceDescriptor.Singleton<IPushPlusMessageSender, FakePushPlusMessageSender>());
    }
}
