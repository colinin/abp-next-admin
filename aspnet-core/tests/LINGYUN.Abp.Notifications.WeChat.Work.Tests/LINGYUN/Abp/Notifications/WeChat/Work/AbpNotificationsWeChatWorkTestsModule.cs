using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

[DependsOn(
    typeof(AbpNotificationsWeChatWorkModule),
    typeof(AbpNotificationsTestsModule),
    typeof(AbpTestsBaseModule))]
public class AbpNotificationsWeChatWorkTestsModule : AbpModule
{
}
