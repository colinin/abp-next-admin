using Elsa;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Notifications;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpNotificationsModule))]
public class AbpElsaNotificationsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services
            .AddNotificationHandlersFrom<AbpElsaWorkflowNotificationHandler>();
    }
}
