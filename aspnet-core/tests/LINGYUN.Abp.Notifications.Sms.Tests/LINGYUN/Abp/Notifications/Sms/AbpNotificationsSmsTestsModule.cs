using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Sms
{
    [DependsOn(
        typeof(AbpNotificationsSmsModule),
        typeof(AbpNotificationsTestsModule),
        typeof(AbpTestsBaseModule))]
    public class AbpNotificationsSmsTestsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // 改变默认数据前缀方法
            PreConfigure<AbpNotificationsSmsOptions>(options =>
            {
                options.TemplateParamsPrefix = "[sms-override]";
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 自定义数据处理方法
            Configure<AbpNotificationOptions>(options =>
            {
                // 这条通知返回标准化的通知
                options.NotificationDataMappings
                    .Mapping(
                        SmsNotificationPublishProvider.ProviderName,
                        NotificationsTestsNames.Test2,
                        data => NotificationData.ToStandardData(data));

                // 这条通知不做任何处理
                options.NotificationDataMappings
                    .Mapping(
                        SmsNotificationPublishProvider.ProviderName,
                        NotificationsTestsNames.Test3,
                        data => data);
            });
        }
    }
}
