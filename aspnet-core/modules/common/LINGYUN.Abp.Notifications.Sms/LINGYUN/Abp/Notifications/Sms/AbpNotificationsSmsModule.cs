using Microsoft.Extensions.DependencyInjection;
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
            Configure<AbpNotificationsSmsOptions>(options =>
            {
                context.Services.ExecutePreConfiguredActions(options);
            });

            Configure<AbpNotificationOptions>(options =>
            {
                options.PublishProviders.Add<SmsNotificationPublishProvider>();

                var smsOptions = context.Services.ExecutePreConfiguredActions<AbpNotificationsSmsOptions>();
                options.NotificationDataMappings
                       .MappingDefault(
                            SmsNotificationPublishProvider.ProviderName,
                            data => NotificationData.ToStandardData(smsOptions.TemplateParamsPrefix, data));
            });
        }
    }
}
