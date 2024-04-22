using LINGYUN.Abp.TuiJuhe;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.TuiJuhe;

[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpTuiJuheModule))]
public class AbpNotificationsTuiJuheModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<TuiJuheNotificationPublishProvider>();
        });
    }
}
