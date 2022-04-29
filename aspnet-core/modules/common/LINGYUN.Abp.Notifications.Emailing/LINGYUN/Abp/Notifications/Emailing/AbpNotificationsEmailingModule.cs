using LINGYUN.Abp.Identity;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Emailing;

[DependsOn(
    typeof(AbpNotificationModule),
    typeof(AbpEmailingModule),
    typeof(AbpIdentityDomainModule))]
public class AbpNotificationsEmailingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationOptions>(options =>
        {
            options.PublishProviders.Add<EmailingNotificationPublishProvider>();
            options.NotificationDataMappings
                   .MappingDefault(EmailingNotificationPublishProvider.ProviderName, data => data);
        });
    }
}
