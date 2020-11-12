using Volo.Abp.Modularity;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Notifications.Sms
{
    [DependsOn(
        typeof(AbpNotificationModule),
        typeof(AbpSmsModule))]
    public class AbpNotificationsSmsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNotificationOptions>(options =>
            {
                options.PublishProviders.Add<SmsNotificationPublishProvider>();
            });
        }
    }
}
