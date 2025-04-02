using LINGYUN.Abp.Tests;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Templating;

[DependsOn(
    typeof(AbpNotificationsTemplatingModule),
    typeof(AbpJsonModule),
    typeof(AbpTestsBaseModule))]
public class AbpNotificationsTemplatingTestModule : AbpModule
{

}
