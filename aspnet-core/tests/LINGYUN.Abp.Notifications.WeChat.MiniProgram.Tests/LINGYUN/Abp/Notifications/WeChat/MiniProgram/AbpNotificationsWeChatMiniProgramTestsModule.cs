using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    [DependsOn(
        typeof(AbpNotificationsWeChatMiniProgramModule),
        typeof(AbpNotificationsTestsModule),
        typeof(AbpTestsBaseModule))]
    public class AbpNotificationsWeChatMiniProgramTestsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpNotificationsWeChatMiniProgramOptions>(options =>
            {
                options.DefaultMsgPrefix = "[wmp-override]";
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 自定义数据处理方法
            Configure<AbpNotificationsPublishOptions>(options =>
            {
                // 这条通知返回标准化的通知
                options.NotificationDataMappings
                    .Mapping(
                        WeChatMiniProgramNotificationPublishProvider.ProviderName,
                        NotificationsTestsNames.Test2,
                        data => NotificationData.ToStandardData(data));

                // 这条通知不做任何处理
                options.NotificationDataMappings
                    .Mapping(
                        WeChatMiniProgramNotificationPublishProvider.ProviderName,
                        NotificationsTestsNames.Test3,
                        data => data);
            });
        }
    }
}
