using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.SignalR;

[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpAspNetCoreSignalRModule))]
public class AbpNotificationsSignalRModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<SignalRNotificationPublishProvider>();
            options.NotificationDataMappings
                   .MappingDefault(SignalRNotificationPublishProvider.ProviderName,
                   data => data.ToSignalRData());
        });
    }
}
