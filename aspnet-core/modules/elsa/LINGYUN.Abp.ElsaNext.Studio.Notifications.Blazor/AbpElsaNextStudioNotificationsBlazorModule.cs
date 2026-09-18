using LINGYUN.Abp.ElsaNext.Notifications;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Studio.Notifications.Blazor;

[DependsOn(
    typeof(AbpElsaNextNotificationsModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioNotificationsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddNotificationsUIHintHandlers();
    }
}
