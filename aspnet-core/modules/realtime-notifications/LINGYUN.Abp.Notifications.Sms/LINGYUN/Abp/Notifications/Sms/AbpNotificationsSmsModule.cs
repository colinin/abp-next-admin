﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Notifications.Sms
{
    [DependsOn(
        typeof(AbpNotificationsModule),
        typeof(AbpSmsModule))]
    public class AbpNotificationsSmsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var preSmsActions = context.Services.GetPreConfigureActions<AbpNotificationsSmsOptions>();
            Configure<AbpNotificationsSmsOptions>(options =>
            {
                preSmsActions.Configure(options);
            });

            Configure<AbpNotificationsPublishOptions>(options =>
            {
                options.PublishProviders.Add<SmsNotificationPublishProvider>();

                var smsOptions = preSmsActions.Configure();

                options.NotificationDataMappings
                       .MappingDefault(
                            SmsNotificationPublishProvider.ProviderName,
                            data => NotificationData.ToStandardData(smsOptions.TemplateParamsPrefix, data));
            });
        }
    }
}
