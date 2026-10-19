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
            // 改变默认参数前缀设置
            PreConfigure<AbpNotificationsSmsOptions>(options =>
            {
                options.TemplateParamsPrefix = "[sms-override]";
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 自定义数据处理器设置
            Configure<AbpNotificationsPublishOptions>(options =>
            {
            });
        }
    }
}
